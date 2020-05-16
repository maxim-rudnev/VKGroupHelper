using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VKGroupHelperSDK.Domain;
using VKGroupHelperSDK.Kernel;

namespace UI
{
    public partial class MainWindow : Form
    {
        ulong _appid = ulong.Parse(ConfigurationManager.AppSettings["AppIdForTest"]);
        VKGroupHelperWorker _vkHelper = null;

        string _folderForCompletedContent = "Completed";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _loadSettings()
        {
            textBoxLogin.Text = Globals.Settings.GetUsername();
            textBoxPassword.Text = Globals.Settings.GetPassword();

            textBoxContentPath.Text = Globals.Settings.ContentPath;
            textBoxMaxPostCount.Text = Globals.Settings.TotalPosts.ToString();
            textBoxPostOnDayCount.Text = Globals.Settings.MaxPostOnDay.ToString();
            textBoxPostTimeStep.Text = Globals.Settings.PostStep.ToString();
            checkBoxUploadVideo.Checked = Globals.Settings.LoadVideo;
            checkBoxUploadPhoto.Checked = Globals.Settings.LoadPictures;
            checkBoxThroughoutTheDay.Checked = Globals.Settings.ThroughoutTheDay;
            checkBoxDeleteFiles.Checked = Globals.Settings.DeleteAfterLoad;
            textBoxPostHashtags.Text = Globals.Settings.Hashtags;
            textBoxTimeMin.Text = Globals.Settings.TimeMin.ToString();
            textBoxTimeMax.Text = Globals.Settings.TimeMax.ToString();

            textBoxQuestion.Text = Globals.Settings.Poll.Question;

            checkBoxPlaceGeoPosition.Checked = Globals.Settings.PlaceGeoPosition;
            textBoxLatitude.Text = Globals.Settings.Latitude.ToString();
            textBoxLongitude.Text = Globals.Settings.Longitude.ToString();
            textBoxLocationStep.Text = Globals.Settings.LocationStep.ToString();
            textBoxSquareWidth.Text = Globals.Settings.SquareWidth.ToString();

            checkBoxStartFromSelectedDate.Checked = Globals.Settings.StartFromSelectedDate;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            textBoxPostOnDayCount_TextChanged(null, null);

            _loadSettings();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _saveSettings();
        }

        private void _saveSettings()
        {
            Globals.Settings.SetUsername(textBoxLogin.Text);
            Globals.Settings.SetPassword(textBoxPassword.Text);

            Globals.Settings.ContentPath = textBoxContentPath.Text;
            Globals.Settings.TotalPosts = int.Parse( textBoxMaxPostCount.Text);
            Globals.Settings.MaxPostOnDay  = int.Parse( textBoxPostOnDayCount.Text);
            Globals.Settings.PostStep  = int.Parse( textBoxPostTimeStep.Text);
            Globals.Settings.LoadVideo = checkBoxUploadVideo.Checked;
            Globals.Settings.LoadPictures  = checkBoxUploadPhoto.Checked;
            Globals.Settings.ThroughoutTheDay  = checkBoxThroughoutTheDay.Checked;
            Globals.Settings.DeleteAfterLoad  = checkBoxDeleteFiles.Checked;
            Globals.Settings.Hashtags = textBoxPostHashtags.Text;
            Globals.Settings.TimeMin  = int.Parse( textBoxTimeMin.Text);
            Globals.Settings.TimeMax  = int.Parse( textBoxTimeMax.Text);

            Globals.Settings.Poll.Question = textBoxQuestion.Text;

            Globals.Settings.PlaceGeoPosition  = checkBoxPlaceGeoPosition.Checked;
            Globals.Settings.Latitude  = double.Parse( textBoxLatitude.Text);
            Globals.Settings.Longitude  = double.Parse( textBoxLongitude.Text);
            Globals.Settings.LocationStep = double.Parse( textBoxLocationStep.Text );
            Globals.Settings.SquareWidth = int.Parse( textBoxSquareWidth.Text);

            Globals.Settings.StartFromSelectedDate = checkBoxStartFromSelectedDate.Checked;

            Globals.Settings.Save();
        }

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            try
            {
                _vkHelper = new VKGroupHelperWorker(
                    _appid, 
                    textBoxLogin.Text, 
                    textBoxPassword.Text);

                groupBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            

            List<Group> groupLst = _vkHelper.GetGroupsWhereUserIsAdmin();

            List<KeyValuePair<string, long>> groupsCBData = new List<KeyValuePair<string, long>>();
            groupsCBData.Add(new KeyValuePair<string, long>("Не выбрано", -1));
            foreach (var grp in groupLst)
            {
                groupsCBData.Add(new KeyValuePair<string, long>(grp.ToString(), grp.Id));
            }

            comboBoxGroups.DataSource = groupsCBData;
            comboBoxGroups.DisplayMember = "Key";
            comboBoxGroups.ValueMember = "Value";

            dateTimePickerBeginDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerBeginDate.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            #region Check
            string contentFolder = textBoxContentPath.Text;
            if ( String.IsNullOrEmpty( contentFolder) ||  !Directory.Exists(contentFolder))
            {
                MessageBox.Show("Операция не может быть выполнена - не выбран каталог загрузки");
                return;
            }
            string completedFolder = $"{contentFolder}\\{_folderForCompletedContent}";
            if (!Directory.Exists(completedFolder)) Directory.CreateDirectory(completedFolder);
            
            
            long selectedGroupId = (long)comboBoxGroups.SelectedValue;
            if (selectedGroupId == -1)
            {
                MessageBox.Show("Операция не может быть выполнена - не выбрана группа");
                return;
            }

            // алгоритм создания отложенных постов
            var contentLst = FSClient.GetContentFromFolder(contentFolder);
            if (contentLst.Count == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - отсутствуют файлы для загрузки");
                return;
            }
            
            // максимальное количество постов
            int maxPostCount = int.Parse( textBoxMaxPostCount.Text);
            if (maxPostCount == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - не установлено максимально возможное количетсво постов");
                return;
            }

            // сколько постов на день
            int maxPostOnDay = int.Parse(textBoxPostOnDayCount.Text);
            if (maxPostOnDay == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - не установлено максимально возможное количетсво постов в день");
                return;
            }

            // дата начала
            DateTime startDate;
            if (checkBoxStartFromSelectedDate.Checked)
            {
                if (dateTimePickerBeginDate.Value <= DateTime.Now.AddMinutes(1))
                {
                    MessageBox.Show("Операция не может быть выполнена - дата публикации должна быть минимум + 1 минут к текущему времени/дате");
                    return;
                }
                startDate = dateTimePickerBeginDate.Value;
            }
            else
            {
                startDate = DateTime.Now.AddMinutes(1);
            }

            #endregion

            // хэштэги
            string hashtags = textBoxPostHashtags.Text;
            
            // одна картинка - один пост
            // какой шаг между постами (часов)
            int postTimeGap;
            if (checkBoxThroughoutTheDay.Enabled && checkBoxThroughoutTheDay.Checked)
            {
                int minHour = int.Parse(textBoxTimeMin.Text);
                int maxHour = int.Parse(textBoxTimeMax.Text);

                postTimeGap = (maxHour - minHour) / maxPostOnDay;
            }
            else
            {
                postTimeGap = int.Parse(textBoxPostTimeStep.Text);
            }

            double? longitude = double.Parse(textBoxLongitude.Text.Replace('.',','));
            double? latitude = double.Parse(textBoxLatitude.Text.Replace('.', ','));
            Location initialLocation = null;
            if (checkBoxPlaceGeoPosition.Checked && longitude != null && latitude != null)
            {
                initialLocation = new Location()
                {
                    Longitude = longitude.Value,
                    Latitude = latitude.Value
                };
            }
            double? squareWidth = double.Parse(textBoxSquareWidth.Text.Replace('.', ','));
            double? locationStep = double.Parse(textBoxLocationStep.Text.Replace('.', ',')); // по умолчанию 0,0016 что примерно 550м

            // в каждом посте один анонимный опрос
            Poll poll = new Poll()
            {
                Question = "...",
                Answers = new List<string>()
                {
                    "ЗАШЛО",
                    "НЕ ЗАШЛО"
                }
            };





            int postCounter = 0;
            int dayCounter = 0;
            int dailyPostCounter = 0;
            DateTime dailyFirstPostDate = startDate;
            int locX = 0;
            int locY = 0;
            int? maxSquarePosts = squareWidth != null ? (int?)squareWidth.Value / 550 : null;

            foreach (var contentInfo in contentLst)
            {
                if (contentInfo.IsVideo() && !checkBoxUploadVideo.Checked) continue;

                if (contentInfo.IsPhoto() && !checkBoxUploadPhoto.Checked) continue;

                if ( postCounter == maxPostCount )
                {
                    MessageBox.Show("Операция выполнена");
                    break;
                }

                if (maxPostOnDay != -1 && dailyPostCounter == maxPostOnDay)
                {
                    dailyPostCounter = 0;
                    dayCounter++;
                }


                // вычисление даты
                DateTime postDate = dailyFirstPostDate.AddDays(dayCounter).AddHours(postTimeGap * dailyPostCounter);

                // вычисление геолокации
                Location newLocation = null;
                if (initialLocation != null && locationStep != null && squareWidth != null && maxSquarePosts != null)
                {
                    newLocation = new Location();
                    newLocation.Latitude = initialLocation.Latitude + locationStep.Value * locX;
                    newLocation.Longitude = initialLocation.Longitude - locationStep.Value * locY;
                }

                try
                {
                    _vkHelper.WallPost(selectedGroupId, postDate, hashtags, contentInfo, poll, newLocation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка - {ex.Message}");
                }

                if (checkBoxDeleteFiles.Checked)
                {
                    File.Delete(contentInfo.FullName);
                }
                else
                {
                    string dstFilename = $"{completedFolder}\\{contentInfo.Name}";
                    if (File.Exists(dstFilename))
                        dstFilename = $"{completedFolder}\\{contentInfo.NameWithoutExtension}-{Guid.NewGuid().ToString()}{contentInfo.Extension}";
                    else
                        dstFilename = $"{completedFolder}\\{contentInfo.Name}";

                    File.Move(contentInfo.FullName, dstFilename);
                }

                dailyPostCounter++;
                postCounter++;
                locX++;
               
                if (maxSquarePosts != null)
                {
                    if (locX > maxSquarePosts) {
                        locX = 0;
                        locY++;
                    }
                    if (locY > maxSquarePosts)
                    {
                        locY = 0;
                    }
                }
            }

            if (maxPostCount == -1)
            {
                MessageBox.Show("Операция выполнена");
            }
        }

        private void buttonSelectContentPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textBoxContentPath.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxContentPath.Text = fbd.SelectedPath;
            }
        }

        private void textBoxPostOnDayCount_TextChanged(object sender, EventArgs e)
        {
            int postOnDay;

            try
            {
                postOnDay = int.Parse(textBoxPostOnDayCount.Text);
            }
            catch
            {
                postOnDay = 0;
            }

            if (postOnDay <=0) checkBoxThroughoutTheDay.Enabled = false;
            else checkBoxThroughoutTheDay.Enabled = true;
        }

        private void checkBoxThroughoutTheDay_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxThroughoutTheDay.Checked) textBoxPostTimeStep.Enabled = false;
            else textBoxPostTimeStep.Enabled = true;
        }

        
    }
}
