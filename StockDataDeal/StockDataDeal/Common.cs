using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace StockDataDeal
{
    public class Common
    {
        public static string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            for (int i = 0; i < 5; i++)    
            {
                try
                {
                    WebRequest request = WebRequest.Create(strUrl);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                    strMsg = reader.ReadToEnd();

                    reader.Close();
                    reader.Dispose();
                    response.Close();
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
                finally
                { }
            }
            
            return strMsg;


        }
    }
}
