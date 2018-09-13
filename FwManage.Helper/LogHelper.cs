using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogQuery = Interop.MSUtil.LogQueryClassClass;
using IISInputFormat = Interop.MSUtil.COMIISW3CInputContextClassClass;
using LogRecordSet = Interop.MSUtil.ILogRecordset;
using System.Configuration;

namespace FwManage.Helper
{
    public class LogHelper
    {
        public static List<MaxRequestModel> GetTop20MaxRequest(string logPath)
        {
            string fireName = ConfigurationManager.AppSettings["fireName"];
            var listIps = FwHelper.GetList(fireName);

            List<MaxRequestModel> listModel = new List<MaxRequestModel>();
            LogRecordSet oRecordSet = null;

            try
            {
                LogQuery oLogQuery = new LogQuery();
                IISInputFormat oIISInputFormat = new IISInputFormat();

                string query = string.Format(@"Select Top 100
                            c-ip as [CIP],
                            COUNT(*) AS Hits 
                            FROM {0}
                            GROUP BY [CIP]
                            ORDER BY Hits DESC", logPath);

                oRecordSet = oLogQuery.Execute(query, oIISInputFormat);


                for (; !oRecordSet.atEnd(); oRecordSet.moveNext())
                {
                    var cip = oRecordSet.getRecord().getValue("CIP") as string;
                    var hits = (int)oRecordSet.getRecord().getValue("Hits");
                    if (!listIps.Contains(cip))
                    {
                        listModel.Add(new MaxRequestModel() { IP = cip ?? string.Empty, Count = hits });
                    }
                }

                oRecordSet.close();
                oRecordSet = null;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                System.IO.File.WriteAllText("D:\\error.log", exc.ToString());
            }
            catch (Exception exc)
            {
                System.IO.File.WriteAllText("D:\\error.log", exc.ToString());
            }
            finally
            {
                if (oRecordSet != null)
                {
                    oRecordSet.close();
                    oRecordSet = null;
                }
            }
            return listModel;

        }

        public static List<HightRequestPage> GetTop100RequestDetail(string logPath, string ip)
        {
            List<HightRequestPage> listModel = new List<HightRequestPage>();
            LogRecordSet oRecordSet = null;

            try
            {
                LogQuery oLogQuery = new LogQuery();
                IISInputFormat oIISInputFormat = new IISInputFormat();

                string query = string.Format(@"Select Top 100
                                cs-uri-stem as [Request URI],
                                cs-uri-query as [Request Param],
                                COUNT(*) AS Hits 
                            FROM {0}
                            WHERE c-ip='{1}'
                            GROUP BY cs-uri-stem,cs-uri-query
                            ORDER BY Hits DESC", logPath, ip);

                oRecordSet = oLogQuery.Execute(query, oIISInputFormat);

                for (; !oRecordSet.atEnd(); oRecordSet.moveNext())
                {
                    var uri = oRecordSet.getRecord().getValue("Request URI") as string;
                    var param = oRecordSet.getRecord().getValue("Request Param") as string;
                    var hits = (int)oRecordSet.getRecord().getValue("Hits");

                    listModel.Add(new HightRequestPage() { Url = uri ?? string.Empty, Param = param ?? string.Empty, Count = hits });
                }

                oRecordSet.close();
                oRecordSet = null;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {

            }
            finally
            {
                if (oRecordSet != null)
                {
                    oRecordSet.close();
                    oRecordSet = null;
                }
            }
            return listModel;

        }

        public static List<string> GetHackIps(string logPath, int rCount)
        {
            LogRecordSet oRecordSet = null;
            List<string> listIps = new List<string>();
            var nowDataTime = DateTime.Now.AddHours(-8);
            try
            {
                LogQuery oLogQuery = new LogQuery();
                IISInputFormat oIISInputFormat = new IISInputFormat();

                string query = string.Format(@"Select 
                            c-ip as [CIP],Count(*) AS Hits 
                            FROM {0}
                            WHERE date='{1}' and time>'{2}'
                            GROUP BY [CIP]", logPath, nowDataTime.ToString("yyyy-MM-dd"), nowDataTime.AddMinutes(-10).ToString("HH:mm:ss"));

                oRecordSet = oLogQuery.Execute(query, oIISInputFormat);

                for (; !oRecordSet.atEnd(); oRecordSet.moveNext())
                {
                    var hit = (int)oRecordSet.getRecord().getValue("Hits");
                    if (hit > rCount)
                    {
                        var ip = oRecordSet.getRecord().getValue("CIP") as string;
                        listIps.Add(ip);
                    }
                }

                oRecordSet.close();
                oRecordSet = null;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {

            }
            finally
            {
                if (oRecordSet != null)
                {
                    oRecordSet.close();
                    oRecordSet = null;
                }
            }
            return listIps;
        }
    }
}
