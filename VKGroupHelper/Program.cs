using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Polls;

namespace VKGroupHelper
{
    class Program
    {
        static void Main(string[] args)
        {
			long groupid = 164513489;
			ulong groupidULONG = 164513489;
			var api = new VkApi();

			api.Authorize(new ApiAuthParams
			{
				ApplicationId = 7395016,
				Login = "",
				Password = "",
				Settings = Settings.All
			});
			Console.WriteLine(api.Token);
			var res = api.Groups.Get(new GroupsGetParams() { 
				Fields = GroupsFields.All,
				UserId = api.UserId,
				Extended = true
			});

			

			var polls = api.PollsCategory.Create(new PollsCreateParams { Question = "Ну какой вопрос?", AddAnswers = new List<string> { "asdf", "fdas" } , OwnerId = -groupid });

			List<VkNet.Model.Attachments.MediaAttachment> attList = new List<VkNet.Model.Attachments.MediaAttachment>();

			attList.Add(polls);

			var uploadServer = api.Photo.GetWallUploadServer(groupid);

			var wc = new WebClient();

			var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, @"C:\qwer.jpg"));

			System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Attachments.Photo> photos = api.Photo.SaveWallPhoto(responseFile, null, (ulong)groupid);

			foreach (var element in photos)
			{
				attList.Add(element);
			}

			


			api.Wall.Post(new WallPostParams()
			{
				OwnerId = -groupid,
				Message = "asdffffffff",
				PublishDate = new DateTime(2020,5,11,11,12,12),
				Attachments = attList
			});

			Console.WriteLine(res.TotalCount);

			Console.ReadLine();
        }
    }
}
