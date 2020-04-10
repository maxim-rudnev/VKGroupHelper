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

            string username = ConfigurationManager.AppSettings["UsernameForTest"];
            string password = ConfigurationManager.AppSettings["PasswordForTest"];
            ulong appid = ulong.Parse( ConfigurationManager.AppSettings["AppIdForTest"]);
            string picFolder = @"C:\Users\admin\Desktop\Контент 1";

            

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
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string contentFolder = textBoxContentPath.Text;

            var picLst = FSClient.GetPicturesFromFolder(contentFolder);

            // алгоритм создания отложенных постов
            long groupid = long.Parse(textBoxGroupId.Text);

            // максимальное количество постов
            int? maxPostCount = int.Parse( textBoxMaxPostCount.Text);
            // хэштэги
            string hashtags = textBoxPostHashtags.Text;
            // сколько постов на день
            int postOnDayCount = int.Parse( textBoxPostOnDayCount.Text);
            // одна картинка - один пост
            // какой шаг между постами (часов)
            int postTimeGap = int.Parse( textBoxPostTimeStep.Text);

            // дата начала
            DateTime startDate = dateTimePickerBeginDate.Value;

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
                if (maxPostCount != null && postCounter == maxPostCount)
                {
                    break;
                }

                if (dailyPostCounter == postOnDayCount)
                {
                    dailyPostCounter = 0;
                    dayCounter++;
                }


                // вычисление даты
                DateTime postDate = dailyFirstPostDate.AddDays(dayCounter).AddHours(postTimeGap * dailyPostCounter);

                _vkHelper.WallPost(groupid, postDate, hashtags, picPath, poll);

                File.Delete(picPath);

                dailyPostCounter++;
                postCounter++;
            }

        }
    }
}
