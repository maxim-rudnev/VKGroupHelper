using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Common;
using VKGroupHelperSDK.Domain;
using VKGroupHelperSDK.Kernel;

namespace UI.Presenters
{
    public class MainFormPresenter: IPresenter
    {
        IMainFormView _view;
        Settings _settings;
        VKGroupHelperWorker _vk;
        ApplicationController _applicationController;

        public MainFormPresenter(ApplicationController applicationController, IMainFormView mainForm, Settings settings, VKGroupHelperWorker vk)
        {
            _view = mainForm;
            _settings = settings;
            _vk = vk;
            _applicationController = applicationController;

            _view.Login += () => Login();
            _view.Close += () => Close();
            _view.VKUpload += () => VKUpload();
        }

        private async void VKUpload()
        {
            await Task.Run(() =>
            {
                if (_view.Check())
                {
                    _view.UpdateSettings(_settings);
                    _settings.Save();

                    // хэштэги
                    string hashtags = _settings.Hashtags;

                    // одна картинка - один пост
                    // какой шаг между постами (часов)
                    int postTimeGap;
                    if (_settings.ThroughoutTheDay)
                    {
                        int minHour = _settings.TimeMin;
                        int maxHour = _settings.TimeMax;

                        postTimeGap = (maxHour - minHour) / _settings.MaxPostOnDay;
                    }
                    else
                    {
                        postTimeGap = _settings.PostStep;
                    }

                    double? longitude = _settings.Longitude;
                    double? latitude = _settings.Latitude;
                    Location initialLocation = null;
                    if (_settings.PlaceGeoPosition && longitude != null && latitude != null)
                    {
                        initialLocation = new Location()
                        {
                            Longitude = longitude.Value,
                            Latitude = latitude.Value
                        };
                    }
                    double? squareWidth = _settings.SquareWidth;
                    double? locationStep = _settings.LocationStep; // по умолчанию 0,0016 что примерно 550м

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
                    DateTime dailyFirstPostDate = _settings.StartDate;
                    int locX = 0;
                    int locY = 0;
                    int? maxSquarePosts = squareWidth != null ? (int?)squareWidth.Value / 550 : null;

                    string completedFolder = $"{_settings.ContentPath}\\Completed";
                    if (!Directory.Exists(completedFolder)) Directory.CreateDirectory(completedFolder);

                    foreach (var contentInfo in FSClient.GetContentFromFolder(_settings.ContentPath))
                    {
                        if (contentInfo.IsVideo() && !_settings.LoadVideo) continue;

                        if (contentInfo.IsPhoto() && !_settings.LoadPictures) continue;

                        if (postCounter == _settings.TotalPosts)
                        {
                            _view.ShowMessage("Операция выполнена");
                            break;
                        }

                        if (_settings.MaxPostOnDay != -1 && dailyPostCounter == _settings.MaxPostOnDay)
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
                            _vk.WallPost(_settings.GroupId, postDate, hashtags, contentInfo, poll, newLocation);
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Произошла ошибка - {ex.Message}");
                        }

                        if (_settings.DeleteAfterLoad)
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
                            if (locX > maxSquarePosts)
                            {
                                locX = 0;
                                locY++;
                            }
                            if (locY > maxSquarePosts)
                            {
                                locY = 0;
                            }
                        }
                    }

                    _view.ShowMessage("Операция выполнена");
                }
            });
        }

        private void Close()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();
        }

        private async void Login()
        {
            await Task.Run(() =>
            {
                _view.UpdateSettings(_settings);
                _settings.Save();

                try
                {
                    _vk.Login(_settings.GetUsername(), _settings.GetPassword());

                    _view.LoadGroups(_vk.GetGroupsWhereUserIsAdmin());
                    _view.EnableVKUploadGroupBox();
                }
                catch (Exception ex)
                {
                    _view.ShowMessage(ex.Message);
                }
            });
        }

        public void Run()
        {
            _view.LoadSettings(_settings);
            _view.Show();
        }
    }
}
