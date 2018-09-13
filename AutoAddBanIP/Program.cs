using AutoAddBanIP.Caching;
using FwManage.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAddBanIP
{
    class Program
    {
        private static ICacheManager _cacheManager;
        /// <summary>
        /// 例外IP集合
        /// </summary>
        private static List<string> _listExceptips;
        //IISLog日志路径
        private static string _logPath;
        //10分钟内警线
        private static int _mCount;

        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));

            try
            {
                try
                {
                    _cacheManager = RedisHelper.GetInstance();
                }
                catch (Exception ex)
                {
                    log.Error("Redis初始化异常", ex);
                }

                try
                {
                    _listExceptips = (System.Configuration.ConfigurationManager.AppSettings["exceptip"] ?? string.Empty).Split(',').ToList();
                    _logPath = System.Configuration.ConfigurationManager.AppSettings["logpath"];
                }
                catch (Exception ex)
                {
                    log.Error("屏蔽IP集合或日志路径初始化异常", ex);
                }

                try
                {
                    _mCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mcount"]);
                }
                catch (Exception ex)
                {
                    log.Error("警戒线初始化异常", ex);
                }

                var date = DateTime.Now.ToString("yyMMdd");
                var logPath = @"C:\Users\Rockey\Desktop\u_ex17010.log";// string.Format(@"{0}\u_ex{1}.log", _logPath, date);
                var listIps = LogHelper.GetHackIps(logPath, _mCount);

                listIps.ForEach(p =>
                {
                    if (!_listExceptips.Contains(p))
                    {
                        if (!_cacheManager.IsSet("pb_" + p))
                        {
                            _cacheManager.Set("pb_" + p, "true", 1440);
                            log.Info(p + "加入屏蔽名单");
                        }
                    }
                });

                log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "检查完成");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
    }
}
