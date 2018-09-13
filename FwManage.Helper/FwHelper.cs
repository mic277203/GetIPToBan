using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwManage.Helper
{
    /// <summary>
    /// 防火墙添加远程IP
    /// </summary>
    public class FwHelper
    {
        public static List<string> GetList(string ruleName)
        {
            List<string> listModel = new List<string>();

            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            var role = fwPolicy.Rules.Item(ruleName);

            if (role != null)
            {
                var ips = role.RemoteAddresses;

                if (ips.Length > 0)
                {
                    var arrIp = ips.Split(',');

                    foreach (var ip in arrIp)
                    {
                        listModel.Add(ip.Split('/')[0] ?? string.Empty);
                    }
                }
            }

            return listModel;
        }
        /// <summary>
        /// 添加远程IP
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ip"></param>
        public static void AddRemoteIp(string ruleName, string ip)
        {
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            var role = fwPolicy.Rules.Item(ruleName);

            if (role != null)
            {
                var ips = role.RemoteAddresses;

                if (!ips.Contains(ip))
                {
                    if(ips.Length>0)
                    {
                        role.RemoteAddresses += string.Format(",{0}/255.255.255.255", ip);
                    }
                    else
                    {
                        role.RemoteAddresses += string.Format("{0}/255.255.255.255", ip);
                    }
                    
                }
            }
        }

        /// <summary>
        /// 移除远程IP
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ip"></param>
        public static void RemoveRemoteIp(string ruleName, string ip)
        {
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            var role = fwPolicy.Rules.Item(ruleName);

            if (role != null)
            {
                var ips = role.RemoteAddresses;

                if (ips.Contains(ip))
                {
                    var arrIp = ips.Split(',');

                    var len = arrIp.Length;
                    StringBuilder strBuild = new StringBuilder();

                    for (int i = 0; i < len; i++)
                    {
                        if (!arrIp[i].Contains(ip))
                        {
                            strBuild.AppendFormat("{0},", arrIp[i]);
                        }
                    }

                    if (strBuild.Length > 0)
                    {
                        strBuild.Remove(strBuild.Length - 1, 1);
                    }

                    role.RemoteAddresses = strBuild.ToString();
                }
            }
        }


        /// <summary>
        /// 添加远程IP
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ip"></param>
        public static void AddLocalIp(string ruleName, string ip)
        {
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            var role = fwPolicy.Rules.Item(ruleName);

            if (role != null)
            {
                var ips = role.LocalAddresses;

                if (!ips.Contains(ip))
                {
                    if (ips.Length > 0)
                    {
                        role.LocalAddresses += string.Format(",{0}/255.255.255.255", ip);
                    }
                    else
                    {
                        role.LocalAddresses += string.Format("{0}/255.255.255.255", ip);
                    }

                }
            }
        }

        /// <summary>
        /// 移除远程IP
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ip"></param>
        public static void RemoveLocalIp(string ruleName, string ip)
        {
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            var role = fwPolicy.Rules.Item(ruleName);

            if (role != null)
            {
                var ips = role.LocalAddresses;

                if (ips.Contains(ip))
                {
                    var arrIp = ips.Split(',');

                    var len = arrIp.Length;
                    StringBuilder strBuild = new StringBuilder();

                    for (int i = 0; i < len; i++)
                    {
                        if (!arrIp[i].Contains(ip))
                        {
                            strBuild.AppendFormat("{0},", arrIp[i]);
                        }
                    }

                    if (strBuild.Length > 0)
                    {
                        strBuild.Remove(strBuild.Length - 1, 1);
                    }

                    role.LocalAddresses = strBuild.ToString();
                }
            }
        }
    }
}
