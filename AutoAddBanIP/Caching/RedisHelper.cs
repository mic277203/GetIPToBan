using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAddBanIP.Caching
{
    public partial class RedisHelper
    {
        #region Redis
        private readonly static string _redisStr;
        private static ICacheManager _redisManager;

        private static readonly object redisObj = new object();

        public static ICacheManager GetInstance()
        {
            if (_redisManager == null)
            {
                lock (redisObj)
                {
                    if (_redisManager == null)
                    {
                        IRedisConnectionWrapper wrapper = new RedisConnectionWrapper(_redisStr);
                        _redisManager = new RedisCacheManager(wrapper);
                    }
                }
            }

            return _redisManager;
        }

        public static IRedisMessage RedisMessage
        {
            get
            {
                return GetInstance() as IRedisMessage;
            }
        }
        #endregion

        static RedisHelper()
        {
            _redisStr = ConfigurationManager.AppSettings["redisStr"];

            if (string.IsNullOrEmpty(_redisStr))
            {
                throw new ArgumentNullException("redis connection string is empty");
            }
        }
    }
}
