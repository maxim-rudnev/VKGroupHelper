using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VKGroupHelperSDK.Domain;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Polls;

namespace VKGroupHelperSDK.Kernel
{
    public class VKGroupHelperWorker
    {
        VkApi _api = null;
        ulong _appid;

        public VKGroupHelperWorker(ulong appid)
        {
            _api = new VkApi();
            _appid = appid;
        }

        public void Login(string username, string password)
        {
            _api.Authorize(new ApiAuthParams
            {
                ApplicationId = _appid,
                Login = username,
                Password = password,
                Settings = Settings.All
            });
        }

        public void WallPost(long groupid, DateTime postDate, string hashtags, ContentForUploadInfo contentInfo, Poll poll, Location location)
        {
            List<VkNet.Model.Attachments.MediaAttachment> attList = new List<VkNet.Model.Attachments.MediaAttachment>();

            // добавление опроса в аттач
            var polls = _api.PollsCategory.Create(
                            new PollsCreateParams { 
                                Question = poll.Question, 
                                AddAnswers = poll.Answers, 
                                OwnerId = -groupid });

            attList.Add(polls);


            // загрузка картинки
            if (contentInfo.IsPhoto())
            {
                var uploadServer = _api.Photo.GetWallUploadServer(groupid);
                var wc = new WebClient();
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, contentInfo.FullName));

                if (location != null)
                {
                    var jsonResponseFile = JsonConvert.DeserializeObject<dynamic>(responseFile);

                    VkNet.Utils.VkParameters param = new VkNet.Utils.VkParameters 
                    { 
                        { "group_id", groupid },
                        { "photo", jsonResponseFile["photo"] },
                        { "server", jsonResponseFile["server"] },
                        { "hash", jsonResponseFile["hash"] },
                        { "latitude", location.Latitude },
                        { "longitude", location.Longitude },
                    };

                    var saveWallPhotoResponse = _api.Call("photos.saveWallPhoto", param);

                    var jsonSaveWallPhotoResponse = JsonConvert.DeserializeObject<dynamic>(saveWallPhotoResponse.RawJson)["response"][0];
                    VkNet.Model.Attachments.Photo photo = new VkNet.Model.Attachments.Photo()
                    {
                        Id = jsonSaveWallPhotoResponse["id"],
                        OwnerId = jsonSaveWallPhotoResponse["owner_id"],
                        AccessKey = jsonSaveWallPhotoResponse["access_key"]
                    };

                    List<PhotoSize> psLst = new List<PhotoSize>();
                    foreach (var photoSize in jsonSaveWallPhotoResponse["sizes"])
                    {
                        PhotoSize ps = new PhotoSize()
                        {
                            Type = _getPhotoSizeType(photoSize.type.ToString()),
                            Height = photoSize.height,
                            Url = photoSize.url,
                            Width = photoSize.width,
                        };

                        psLst.Add(ps);
                    }

                    photo.Sizes = new System.Collections.ObjectModel.ReadOnlyCollection<PhotoSize>(psLst);
                    photo.Latitude = location.Latitude;
                    photo.Longitude = location.Longitude;

                    attList.Add(photo);
                }
                else
                {
                    System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Attachments.Photo> photos = _api.Photo.SaveWallPhoto(responseFile,
                                                                                                                                    null,
                                                                                                                                    (ulong)groupid);

                    foreach (var element in photos)
                    {
                        attList.Add(element);
                    }
                }
            }
            // 
            // загрузка видео
            if (contentInfo.IsVideo())
            {
                var video = _api.Video.Save(new VkNet.Model.RequestParams.VideoSaveParams
                {
                    IsPrivate = false,
                    Repeat = false,
                    Description = hashtags,
                    Name = contentInfo.Name
                });
                var wc = new WebClient();
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(video.UploadUrl, contentInfo.FullName));

                attList.Add(video);
            }

            var postParams = new WallPostParams()
            {
                OwnerId = -groupid,
                Message = hashtags,
                PublishDate = postDate,
                Attachments = attList
            };
            if (location != null)
            {
                postParams.Long = location.Longitude;
                postParams.Lat = location.Latitude;
            }

            _api.Wall.Post(postParams);
        }

        private PhotoSizeType _getPhotoSizeType(string type)
        {
            switch (type)
            {
                case "s": return PhotoSizeType.S;
                case "m": return PhotoSizeType.M;
                case "x": return PhotoSizeType.X;
                case "o": return PhotoSizeType.O;
                case "p": return PhotoSizeType.P;
                case "q": return PhotoSizeType.Q;
                case "r": return PhotoSizeType.R;
                case "y": return PhotoSizeType.Y;
                case "z": return PhotoSizeType.Z;
                case "w": return PhotoSizeType.W;
                default: return null;
            }
        }

        public List<Domain.Group> GetGroupsWhereUserIsAdmin()
        {
            List<Domain.Group> res = new List<Domain.Group>();

            var respGroups = _api.Groups.Get(new GroupsGetParams()
            {
                Fields = GroupsFields.All,
                Extended = true,
                Filter = GroupsFilters.Administrator | GroupsFilters.Editor | GroupsFilters.Moderator
            });

            foreach (var group in respGroups)
            {
                res.Add(new Domain.Group() { Id = group.Id, Name = group.Name, ScreenName = group.ScreenName});
            }

            return res;
        }

        public void GetPostsFromGroup(long groupid)
        {
            var posts = _api.Wall.Get(new WallGetParams()
            {
                OwnerId = -groupid
            });
        }
    }
}
