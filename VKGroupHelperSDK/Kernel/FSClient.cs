using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKGroupHelperSDK.Kernel
{
    public class FSClient
    {
        public static List<string> GetPicturesFromFolder(string picFolder)
        {
            return Directory.GetFiles(picFolder, "*.jpg").ToList();
        }
    }
}
