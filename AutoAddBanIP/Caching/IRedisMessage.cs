using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAddBanIP.Caching
{
    public interface IRedisMessage
    {
        long Publish<T>(string channel, T model);
        void Subscribe<T>(string channel, Action<T> handler, bool queue = true);
    }
}
