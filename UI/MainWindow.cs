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

            ulong appid = ulong.Parse(ConfigurationManager.AppSettings["AppIdForTest"]);

#if DEBUG
            string username = ConfigurationManager.AppSettings["UsernameForTest"];
            string password = ConfigurationManager.AppSettings["PasswordForTest"];
            textBoxLogin.Text = username;
            textBoxPassword.Text = password;
#endif
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            textBoxPostOnDayCount_TextChanged(null, null);
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
            DateTime startDate = dateTimePickerBeginDate.Value;
            if (startDate <= DateTime.Now.AddMinutes(1))
            {
                MessageBox.Show("Операция не может быть выполнена - дата публикации должна быть минимум + 1 минут к текущему времени/дате");
                return;
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

            double? longitude = double.Parse(textBoxLong.Text.Replace('.',','));
            double? latitude = double.Parse(textBoxLat.Text.Replace('.', ','));
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
                    string filename = $"{completedFolder}\\{contentInfo.Name}";
                    if (File.Exists(filename))
                        filename = $"{completedFolder}\\{contentInfo.NameWithoutExtension}-{Guid.NewGuid().ToString()}{contentInfo.Extension}";
                    else
                        filename = contentInfo.Name;

                    File.Move(contentInfo.FullName, filename);
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
