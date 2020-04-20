using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKGroupHelperSDK.Domain;

namespace VKGroupHelperSDK.Kernel
{
    public class FSClient
    {
        public static List<ContentForUploadInfo> GetContentFromFolder(string contentFolder)
        {
            var res = Directory.GetFiles(contentFolder, "*.*")
                .Where(x=>x.EndsWith(".jpg") || x.EndsWith(".mp4"))
                .Select(x => new ContentForUploadInfo(x));

            return res.ToList();
        }
    }
}
