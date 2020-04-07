using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string username = ConfigurationManager.AppSettings["UsernameForTest"];
            string password = ConfigurationManager.AppSettings["PasswordForTest"];
            long appid = long.Parse( ConfigurationManager.AppSettings["AppIdForTest"]);
        }
    }
}
