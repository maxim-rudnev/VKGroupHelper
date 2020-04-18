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
            string contentFolder = textBoxContentPath.Text;
            if ( String.IsNullOrEmpty( contentFolder) ||  !Directory.Exists(contentFolder))
            {
                MessageBox.Show("Операция не может быть выполнена - не выбран каталог загрузки");
                return;
            }
            
            
            long selectedGroupId = (long)comboBoxGroups.SelectedValue;
            if (selectedGroupId == -1)
            {
                MessageBox.Show("Операция не может быть выполнена - не выбрана группа");
                return;
            }

            // алгоритм создания отложенных постов
            var picLst = FSClient.GetPicturesFromFolder(contentFolder);
            if (picLst.Count == 0)
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
            if (startDate <= DateTime.Now)
            {
                MessageBox.Show("Операция не может быть выполнена - дата публикации должна быть минимум + 5 минут к текущему времени/дате");
                return;
            }

            // хэштэги
            string hashtags = textBoxPostHashtags.Text;
            
            // одна картинка - один пост
            // какой шаг между постами (часов)
            int postTimeGap = int.Parse( textBoxPostTimeStep.Text);

            

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

            foreach (var picPath in picLst)
            {
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

                _vkHelper.WallPost(selectedGroupId, postDate, hashtags, picPath, poll);

                if (checkBoxDeleteFiles.Checked)
                    File.Delete(picPath);

                dailyPostCounter++;
                postCounter++;
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
    }
}
