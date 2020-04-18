using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKGroupHelperSDK.Domain
{
    public class Group
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} ({ScreenName})";
        }
    }
}
