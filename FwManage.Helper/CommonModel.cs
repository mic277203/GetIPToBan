using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwManage.Helper
{
    public class MaxRequestModel
    {
        public string IP { get; set; }
        public int Count { get; set; }
    }

    public class HightRequestPage
    {
        public string Url { get; set; }
        public string Param { get; set; }
        public int Count { get; set; }
    }

}
