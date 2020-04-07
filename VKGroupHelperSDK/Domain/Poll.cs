using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKGroupHelperSDK.Domain
{
    public class Poll
    {
        public Poll()
        {
            Answers = new List<string>();
        }

        public string Question { get; set; }

        public List<string> Answers { get; set; }
    }
}
