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
        public MainWindow()
        {
            InitializeComponent();

            string username = ConfigurationManager.AppSettings["UsernameForTest"];
            string password = ConfigurationManager.AppSettings["PasswordForTest"];
            ulong appid = ulong.Parse( ConfigurationManager.AppSettings["AppIdForTest"]);
            string picFolder = @"C:\Users\admin\Desktop\Контент 1";

            //Если вы являетесь владельцем группы, перейдите на страницу — «Рекламировать страницу».
            //Ссылка в адресной строке браузера будет иметь следующий вид:
            //http://vk.com/adscreate?page_id=xxxxxxxx, где xxxxxxxx — id вашего сообщества.
            long groupid = 188488349;

            var helper = new VKGroupHelperWorker(appid, username, password);

            var picLst = FSClient.GetPicturesFromFolder(picFolder);

            // алгоритм создания отложенных постов
            // максимальное количество постов
            int? maxPostCount = null;
            // хэштэги
            string hashtags = "#огонь #юмор #прикол #смешно #смех #жиза #позитив #хорошеенастроение #улыбнись #я #улыбка #поставьлайк #подписка #фоллоуми #коммент #напиши";
            // сколько постов на день
            int postOnDayCount = 5;
            // одна картинка - один пост
            // какой шаг между постами (часов)
            int postTimeGap = 2;

            // дата начала
            DateTime startDate = new DateTime(2020, 04, 08, 9,0,0);

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
                if ( maxPostCount!= null && postCounter == maxPostCount)
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

                helper.WallPost(groupid, postDate, hashtags, picPath, poll);

                File.Delete(picPath);

                dailyPostCounter++;
                postCounter++;
            }

            Console.ReadLine();
        }
    }
}
