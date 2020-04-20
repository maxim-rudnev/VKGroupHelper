using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKGroupHelperSDK.Domain
{
    public class ContentForUploadInfo
    {
        FileInfo _fileInfo;

        public ContentForUploadInfo(string fullfilename)
        {
            if (!File.Exists(fullfilename)) throw new Exception($"Файл {fullfilename} не существует");


            _fileInfo = new FileInfo(fullfilename);
        }

        public string Name { get { return _fileInfo.Name; } }

        public string FullName { get { return _fileInfo.FullName; } }

        public bool IsPhoto()
        {
            bool res = false;
            if (_fileInfo.Extension == ".jpg") res = true;

            return res;
        }

        public bool IsVideo()
        {
            bool res = false;
            if (_fileInfo.Extension == ".mp4") res = true;

            return res;
        }
    }
}
