using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIPS
{
    class Program
    {
        static void Main(string[] args)
        {
            FwManage.Helper.FwHelper.AddLocalIp("HttpRequestBan", "192.168.1.214");
            Console.ReadKey();
        }
    }
}
