using FwManage.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace FwManage.Controllers
{
    public class HomeController : Controller
    {

        private string _logPathStr;

        public HomeController()
        {
            _logPathStr = System.Configuration.ConfigurationManager.AppSettings["logpath"];
        }

        // GET: Home
        public ActionResult Index()
        {
            var date = DateTime.Now.ToString("yyMMdd");
            var logPath = string.Format(@"{0}\u_ex{1}.log", _logPathStr, date);

            List<MaxRequestModel> listModel = new List<MaxRequestModel>();

            if (System.IO.File.Exists(logPath))
            {
                listModel = LogHelper.GetTop20MaxRequest(logPath);
            }
            return View(listModel);
        }

        public ActionResult Detail(string ip)
        {
            var date = DateTime.Now.ToString("yyMMdd");
            var logPath = string.Format(@"{0}\u_ex{1}.log", _logPathStr, date);

            List<HightRequestPage> listModel = new List<HightRequestPage>();

            if (System.IO.File.Exists(logPath))
            {
                listModel = LogHelper.GetTop100RequestDetail(logPath, ip);
            }

            return View(listModel);
        }
    }
}