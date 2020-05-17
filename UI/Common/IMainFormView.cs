using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKGroupHelperSDK.Domain;

namespace UI.Common
{
    public interface IMainFormView: IView
    {
        void LoadSettings(Settings settings);

        void UpdateSettings(Settings settings);

        void ShowMessage(string message);

        void LoadGroups(List<Group> groups);

        void EnableVKUploadGroupBox();

        bool Check();

        event Action Login;

        new event Action Close;

        event Action VKUpload;
    }
}
