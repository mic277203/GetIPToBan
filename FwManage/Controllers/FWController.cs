using FwManage.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FwManage.Controllers
{
    public class FWController : Controller
    {
        private string _fireName;

        public FWController()
        {
            _fireName = System.Configuration.ConfigurationManager.AppSettings["fireName"];
        }
        // GET: FW
        public ActionResult Index()
        {
            var listModel = FwHelper.GetList(_fireName);
            listModel.Remove("11.11.11.11");
            return View(listModel);
        }
        [HttpPost]
        public ActionResult Remove(string ip, string password)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return Json(new { Success = false, Message = "IP不能为空" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new { Success = false, Message = "密码不能为空" });
            }
            if (string.Compare(password, "1234abcd,", true) != 0)
            {
                return Json(new { Success = false, Message = "密码不正确" });
            }

            FwHelper.RemoveRemoteIp(_fireName, ip);

            return Json(new { Success = true, Message = "成功" });
        }

        [HttpPost]
        public ActionResult Add(string ip, string password)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return Json(new { Success = false, Message = "IP不能为空" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new { Success = false, Message = "密码不能为空" });
            }
            if (string.Compare(password, "1234abcd,", true) != 0)
            {
                return Json(new { Success = false, Message = "密码不正确" });
            }

            FwHelper.AddRemoteIp(_fireName, ip);

            return Json(new { Success = true, Message = "成功" });
        }
    }
}