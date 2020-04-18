namespace UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonAuth = new System.Windows.Forms.Button();
            this.groupBoxAuth = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxLong = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxLat = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridViewAnswers = new System.Windows.Forms.DataGridView();
            this.textBoxQuestion = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxTimeMax = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTimeMin = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePickerBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxPostHashtags = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPostTimeStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPostOnDayCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMaxPostCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSelectContentPath = new System.Windows.Forms.Button();
            this.textBoxContentPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxGroups = new System.Windows.Forms.ComboBox();
            this.checkBoxDeleteFiles = new System.Windows.Forms.CheckBox();
            this.groupBoxAuth.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnswers)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(97, 19);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(189, 20);
            this.textBoxLogin.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(97, 44);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(189, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // buttonAuth
            // 
            this.buttonAuth.Location = new System.Drawing.Point(292, 19);
            this.buttonAuth.Name = "buttonAuth";
            this.buttonAuth.Size = new System.Drawing.Size(475, 45);
            this.buttonAuth.TabIndex = 4;
            this.buttonAuth.Text = "Авторизация";
            this.buttonAuth.UseVisualStyleBackColor = true;
            this.buttonAuth.Click += new System.EventHandler(this.buttonAuth_Click);
            // 
            // groupBoxAuth
            // 
            this.groupBoxAuth.Controls.Add(this.textBoxLogin);
            this.groupBoxAuth.Controls.Add(this.buttonAuth);
            this.groupBoxAuth.Controls.Add(this.label1);
            this.groupBoxAuth.Controls.Add(this.textBoxPassword);
            this.groupBoxAuth.Controls.Add(this.label2);
            this.groupBoxAuth.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAuth.Name = "groupBoxAuth";
            this.groupBoxAuth.Size = new System.Drawing.Size(776, 78);
            this.groupBoxAuth.TabIndex = 5;
            this.groupBoxAuth.TabStop = false;
            this.groupBoxAuth.Text = "Авторизация";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxDeleteFiles);
            this.groupBox1.Controls.Add(this.comboBoxGroups);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.textBoxTimeMax);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBoxTimeMin);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dateTimePickerBeginDate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxPostHashtags);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxPostTimeStep);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxPostOnDayCount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxMaxPostCount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.buttonSelectContentPath);
            this.groupBox1.Controls.Add(this.textBoxContentPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 580);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загрузка постов из картинок и видео";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "ID Группы";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(15, 517);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(746, 57);
            this.buttonLoad.TabIndex = 24;
            this.buttonLoad.Text = "Загрузить";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxLong);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBoxLat);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(15, 429);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(746, 82);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Геопозиция";
            // 
            // textBoxLong
            // 
            this.textBoxLong.Location = new System.Drawing.Point(136, 45);
            this.textBoxLong.Name = "textBoxLong";
            this.textBoxLong.Size = new System.Drawing.Size(181, 20);
            this.textBoxLong.TabIndex = 25;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Долгота (Longitude)";
            // 
            // textBoxLat
            // 
            this.textBoxLat.Location = new System.Drawing.Point(136, 19);
            this.textBoxLat.Name = "textBoxLat";
            this.textBoxLat.Size = new System.Drawing.Size(181, 20);
            this.textBoxLat.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(92, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Широта (Latitude)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.dataGridViewAnswers);
            this.groupBox2.Controls.Add(this.textBoxQuestion);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(15, 284);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(752, 138);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Опрос";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Ответы";
            // 
            // dataGridViewAnswers
            // 
            this.dataGridViewAnswers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAnswers.Location = new System.Drawing.Point(126, 45);
            this.dataGridViewAnswers.Name = "dataGridViewAnswers";
            this.dataGridViewAnswers.Size = new System.Drawing.Size(620, 79);
            this.dataGridViewAnswers.TabIndex = 25;
            // 
            // textBoxQuestion
            // 
            this.textBoxQuestion.Location = new System.Drawing.Point(126, 19);
            this.textBoxQuestion.Name = "textBoxQuestion";
            this.textBoxQuestion.Size = new System.Drawing.Size(620, 20);
            this.textBoxQuestion.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Текст опроса";
            // 
            // textBoxTimeMax
            // 
            this.textBoxTimeMax.Location = new System.Drawing.Point(141, 239);
            this.textBoxTimeMax.Name = "textBoxTimeMax";
            this.textBoxTimeMax.Size = new System.Drawing.Size(37, 20);
            this.textBoxTimeMax.TabIndex = 21;
            this.textBoxTimeMax.Text = "23";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 242);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Максимальный час";
            // 
            // textBoxTimeMin
            // 
            this.textBoxTimeMin.Location = new System.Drawing.Point(141, 213);
            this.textBoxTimeMin.Name = "textBoxTimeMin";
            this.textBoxTimeMin.Size = new System.Drawing.Size(37, 20);
            this.textBoxTimeMin.TabIndex = 19;
            this.textBoxTimeMin.Text = "9";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Минимальный час";
            // 
            // dateTimePickerBeginDate
            // 
            this.dateTimePickerBeginDate.Location = new System.Drawing.Point(141, 187);
            this.dateTimePickerBeginDate.Name = "dateTimePickerBeginDate";
            this.dateTimePickerBeginDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerBeginDate.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Дата начала";
            // 
            // textBoxPostHashtags
            // 
            this.textBoxPostHashtags.Location = new System.Drawing.Point(141, 157);
            this.textBoxPostHashtags.Name = "textBoxPostHashtags";
            this.textBoxPostHashtags.Size = new System.Drawing.Size(626, 20);
            this.textBoxPostHashtags.TabIndex = 15;
            this.textBoxPostHashtags.Text = "#огонь #юмор #прикол #смешно #смех #жиза #позитив #хорошеенастроение #улыбнись #я" +
    " #улыбка #поставьлайк #подписка #фоллоуми #коммент #напиши";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Хэштэги";
            // 
            // textBoxPostTimeStep
            // 
            this.textBoxPostTimeStep.Location = new System.Drawing.Point(141, 131);
            this.textBoxPostTimeStep.Name = "textBoxPostTimeStep";
            this.textBoxPostTimeStep.Size = new System.Drawing.Size(37, 20);
            this.textBoxPostTimeStep.TabIndex = 13;
            this.textBoxPostTimeStep.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Шаг между постами (ч.)";
            // 
            // textBoxPostOnDayCount
            // 
            this.textBoxPostOnDayCount.Location = new System.Drawing.Point(141, 105);
            this.textBoxPostOnDayCount.Name = "textBoxPostOnDayCount";
            this.textBoxPostOnDayCount.Size = new System.Drawing.Size(37, 20);
            this.textBoxPostOnDayCount.TabIndex = 11;
            this.textBoxPostOnDayCount.Text = "-1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Постов в день";
            // 
            // textBoxMaxPostCount
            // 
            this.textBoxMaxPostCount.Location = new System.Drawing.Point(141, 79);
            this.textBoxMaxPostCount.Name = "textBoxMaxPostCount";
            this.textBoxMaxPostCount.Size = new System.Drawing.Size(37, 20);
            this.textBoxMaxPostCount.TabIndex = 9;
            this.textBoxMaxPostCount.Text = "-1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Максимум постов";
            // 
            // buttonSelectContentPath
            // 
            this.buttonSelectContentPath.Location = new System.Drawing.Point(659, 51);
            this.buttonSelectContentPath.Name = "buttonSelectContentPath";
            this.buttonSelectContentPath.Size = new System.Drawing.Size(108, 23);
            this.buttonSelectContentPath.TabIndex = 7;
            this.buttonSelectContentPath.Text = "Выбрать";
            this.buttonSelectContentPath.UseVisualStyleBackColor = true;
            this.buttonSelectContentPath.Click += new System.EventHandler(this.buttonSelectContentPath_Click);
            // 
            // textBoxContentPath
            // 
            this.textBoxContentPath.Location = new System.Drawing.Point(97, 53);
            this.textBoxContentPath.Name = "textBoxContentPath";
            this.textBoxContentPath.ReadOnly = true;
            this.textBoxContentPath.Size = new System.Drawing.Size(556, 20);
            this.textBoxContentPath.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Путь";
            // 
            // comboBoxGroups
            // 
            this.comboBoxGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGroups.Location = new System.Drawing.Point(97, 21);
            this.comboBoxGroups.Name = "comboBoxGroups";
            this.comboBoxGroups.Size = new System.Drawing.Size(556, 21);
            this.comboBoxGroups.TabIndex = 26;
            // 
            // checkBoxDeleteFiles
            // 
            this.checkBoxDeleteFiles.AutoSize = true;
            this.checkBoxDeleteFiles.Location = new System.Drawing.Point(401, 81);
            this.checkBoxDeleteFiles.Name = "checkBoxDeleteFiles";
            this.checkBoxDeleteFiles.Size = new System.Drawing.Size(180, 17);
            this.checkBoxDeleteFiles.TabIndex = 27;
            this.checkBoxDeleteFiles.Text = "Удалять файл после загрузки";
            this.checkBoxDeleteFiles.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 710);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxAuth);
            this.Name = "MainWindow";
            this.Text = "VK Group Helper";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.groupBoxAuth.ResumeLayout(false);
            this.groupBoxAuth.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnswers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonAuth;
        private System.Windows.Forms.GroupBox groupBoxAuth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSelectContentPath;
        private System.Windows.Forms.TextBox textBoxContentPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMaxPostCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPostOnDayCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPostTimeStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPostHashtags;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxTimeMax;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxTimeMin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePickerBeginDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dataGridViewAnswers;
        private System.Windows.Forms.TextBox textBoxQuestion;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.TextBox textBoxLong;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxLat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxGroups;
        private System.Windows.Forms.CheckBox checkBoxDeleteFiles;
    }
}

