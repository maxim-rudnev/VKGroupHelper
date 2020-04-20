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
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Polls;

namespace VKGroupHelperSDK.Kernel
{
    public class VKGroupHelperWorker
    {
        VkApi _api = null;

        public VKGroupHelperWorker(ulong appid, string username, string password)
        {
            _api = new VkApi();

            _api.Authorize(new ApiAuthParams
            {
                ApplicationId = appid,
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
                //Location locationApi = null;
                //if (location != null)
                //{
                //    locationApi = new Location()
                //    {
                //        Latitude = location.Latitude,
                //        Longitude = location.Longitude
                //    };
                //}
                System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Attachments.Photo> photos = _api.Photo.SaveWallPhoto(responseFile,
                                                                                                                null,
                                                                                                                (ulong)groupid);
                foreach (var element in photos)
                {
                    attList.Add(element);
                }
            }

            // загрузка видео
            if (contentInfo.IsVideo())
            {
                var video = _api.Video.Save(new VkNet.Model.RequestParams.VideoSaveParams
                {
                    IsPrivate = false,
                    Repeat = false,
                    Description = hashtags,
                    Name = contentInfo.FullName
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
