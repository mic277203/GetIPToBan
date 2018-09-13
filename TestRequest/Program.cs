using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;
            while (true)
            {
                string request = HttpHelper.Helper.Get("http://888.ka20.com/contact.html", Encoding.UTF8);

                Console.WriteLine("成功{0}次", i);
                i++;

                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
