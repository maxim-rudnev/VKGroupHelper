using System;
using System.Collections.Generic;
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

        public void WallPost(long groupid, DateTime postDate, string hashtags, string picPath, Poll poll)
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
            var uploadServer = _api.Photo.GetWallUploadServer(groupid);

            var wc = new WebClient();

            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, picPath));

            System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Attachments.Photo> photos = _api.Photo.SaveWallPhoto(responseFile, null, (ulong)groupid);

            foreach (var element in photos)
            {
                attList.Add(element);
            }



            _api.Wall.Post(new WallPostParams()
            {
                OwnerId = -groupid,
                Message = hashtags,
                PublishDate = postDate,
                Attachments = attList
            });
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
