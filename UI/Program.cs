using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using UI.Presenters;
using VKGroupHelperSDK.Kernel;

namespace UI
{
    static class Program
    {
        public static readonly ApplicationContext Context = new ApplicationContext();


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);

            ulong appid = ulong.Parse(ConfigurationManager.AppSettings["AppIdForTest"]);
            VKGroupHelperWorker vk = new VKGroupHelperWorker(appid);


            ServiceContainer container = new ServiceContainer();
            container.RegisterInstance<VKGroupHelperWorker>(vk);
            container.RegisterInstance<Settings>(Globals.Settings);
            container.RegisterInstance<ApplicationContext>(Context);
            container.Register<IMainFormView,MainForm>();
            container.Register<MainFormPresenter>();

            ApplicationController controller = new ApplicationController(container);
            controller.Run<MainFormPresenter>();
        }
    }
}
