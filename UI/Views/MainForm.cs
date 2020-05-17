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
using UI.Common;
using VKGroupHelperSDK.Domain;
using VKGroupHelperSDK.Kernel;

namespace UI
{
    public partial class MainForm : Form, IMainFormView
    {
        protected ApplicationContext _context;

        
        public MainForm(ApplicationContext context)
        {
            _context = context;
            InitializeComponent();

            dateTimePickerBeginDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerBeginDate.CustomFormat = "MM/dd/yyyy hh:mm:ss";

            buttonAuth.Click += (sender, args) => Invoke(Login);
            this.FormClosing += (sender, args) => Invoke(Close);
            buttonLoad.Click += (sender, args) => Invoke(VKUpload);
        }

        private void Invoke(Action action)
        {
            if (action != null) action();
        }

        public event Action Login;

        public event Action Close;

        public event Action VKUpload;

        public new void Show()
        {
            _context.MainForm = this;
            Application.Run(_context);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


        public void LoadSettings(Settings settings)
        {
            textBoxLogin.Text = settings.GetUsername();
            textBoxPassword.Text = settings.GetPassword();

            textBoxContentPath.Text = settings.ContentPath;
            textBoxMaxPostCount.Text = settings.TotalPosts.ToString();
            textBoxPostOnDayCount.Text = settings.MaxPostOnDay.ToString();
            textBoxPostTimeStep.Text = settings.PostStep.ToString();
            checkBoxUploadVideo.Checked = settings.LoadVideo;
            checkBoxUploadPhoto.Checked = settings.LoadPictures;
            checkBoxThroughoutTheDay.Checked = settings.ThroughoutTheDay;
            checkBoxDeleteFiles.Checked = settings.DeleteAfterLoad;
            textBoxPostHashtags.Text = settings.Hashtags;
            textBoxTimeMin.Text = settings.TimeMin.ToString();
            textBoxTimeMax.Text = settings.TimeMax.ToString();

            textBoxQuestion.Text = settings.Poll.Question;

            checkBoxPlaceGeoPosition.Checked = settings.PlaceGeoPosition;
            textBoxLatitude.Text = settings.Latitude.ToString();
            textBoxLongitude.Text = settings.Longitude.ToString();
            textBoxLocationStep.Text = settings.LocationStep.ToString();
            textBoxSquareWidth.Text = settings.SquareWidth.ToString();

            checkBoxStartFromSelectedDate.Checked = settings.StartFromSelectedDate;
            dateTimePickerBeginDate.Value = settings.StartDate == DateTime.MinValue ? DateTime.Now: settings.StartDate;
        }

        public void UpdateSettings(Settings settings)
        {
            settings.SetUsername(textBoxLogin.Text);
            settings.SetPassword(textBoxPassword.Text);

            settings.ContentPath = textBoxContentPath.Text;
            settings.TotalPosts = int.Parse(textBoxMaxPostCount.Text);
            settings.MaxPostOnDay = int.Parse(textBoxPostOnDayCount.Text);
            settings.PostStep = int.Parse(textBoxPostTimeStep.Text);
            settings.LoadVideo = checkBoxUploadVideo.Checked;
            settings.LoadPictures = checkBoxUploadPhoto.Checked;
            settings.ThroughoutTheDay = checkBoxThroughoutTheDay.Checked;
            settings.DeleteAfterLoad = checkBoxDeleteFiles.Checked;
            settings.Hashtags = textBoxPostHashtags.Text;
            settings.TimeMin = int.Parse(textBoxTimeMin.Text);
            settings.TimeMax = int.Parse(textBoxTimeMax.Text);

            if (comboBoxGroups.SelectedValue == null)
                settings.GroupId = -1;
            else
                settings.GroupId = (long)comboBoxGroups.SelectedValue;
            settings.Poll.Question = textBoxQuestion.Text;

            settings.PlaceGeoPosition = checkBoxPlaceGeoPosition.Checked;
            settings.Latitude = double.Parse(textBoxLatitude.Text.Replace('.', ','));
            settings.Longitude = double.Parse(textBoxLongitude.Text.Replace('.', ','));
            settings.LocationStep = double.Parse(textBoxLocationStep.Text.Replace('.', ','));
            settings.SquareWidth = int.Parse(textBoxSquareWidth.Text.Replace('.', ','));

            settings.StartFromSelectedDate = checkBoxStartFromSelectedDate.Checked;
            if (checkBoxStartFromSelectedDate.Checked)
            {
                settings.StartDate = dateTimePickerBeginDate.Value;
            }
            else
            {
                settings.StartDate = DateTime.Now.AddMinutes(1);
            }
        }

        
        private void MainWindow_Load(object sender, EventArgs e)
        {
            textBoxPostOnDayCount_TextChanged(null, null);
        }

        public void LoadGroups(List<Group> groups)
        {
            List<KeyValuePair<string, long>> groupsCBData = new List<KeyValuePair<string, long>>();
            groupsCBData.Add(new KeyValuePair<string, long>("Не выбрано", -1));
            foreach (var grp in groups)
            {
                groupsCBData.Add(new KeyValuePair<string, long>(grp.ToString(), grp.Id));
            }

            comboBoxGroups.DataSource = groupsCBData;
            comboBoxGroups.DisplayMember = "Key";
            comboBoxGroups.ValueMember = "Value";
        }

        public void EnableVKUploadGroupBox()
        {
            groupBox1.Enabled = true;
        }

        public bool Check()
        {
            string contentFolder = textBoxContentPath.Text;
            if (String.IsNullOrEmpty(contentFolder) || !Directory.Exists(contentFolder))
            {
                MessageBox.Show("Операция не может быть выполнена - не выбран каталог загрузки");
                return false;
            }
            


            long selectedGroupId = (long)comboBoxGroups.SelectedValue;
            if (selectedGroupId == -1)
            {
                MessageBox.Show("Операция не может быть выполнена - не выбрана группа");
                return false;
            }

            // алгоритм создания отложенных постов
            var contentLst = FSClient.GetContentFromFolder(contentFolder);
            if (contentLst.Count == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - отсутствуют файлы для загрузки");
                return false;
            }

            // максимальное количество постов
            int maxPostCount = int.Parse(textBoxMaxPostCount.Text);
            if (maxPostCount == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - не установлено максимально возможное количетсво постов");
                return false;
            }

            // сколько постов на день
            int maxPostOnDay = int.Parse(textBoxPostOnDayCount.Text);
            if (maxPostOnDay == 0)
            {
                MessageBox.Show("Операция не может быть выполнена - не установлено максимально возможное количетсво постов в день");
                return false;
            }

            // дата начала
            DateTime startDate;
            if (checkBoxStartFromSelectedDate.Checked)
            {
                if (dateTimePickerBeginDate.Value <= DateTime.Now.AddMinutes(1))
                {
                    MessageBox.Show("Операция не может быть выполнена - дата публикации должна быть минимум + 1 минут к текущему времени/дате");
                    return false;
                }
                startDate = dateTimePickerBeginDate.Value;
            }
            else
            {
                startDate = DateTime.Now.AddMinutes(1);
            }

            return true;
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
