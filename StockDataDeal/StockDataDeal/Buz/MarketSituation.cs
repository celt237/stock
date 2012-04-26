using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Text.RegularExpressions;

namespace StockDataDeal.Buz
{
    public class MarketSituation
    {
        public static MarketSituation Instance = new MarketSituation();

        private static readonly DataTable list = null;


        static MarketSituation()
        {
            list = GetMarketTypeList();
        }

        public static string GetTableName()
        {
            return "marketsituation";
        }

        private static DataTable GetMarketTypeList()
        {
            string sql = string.Format("select * from {0} with(nolock)", GetTableName());
            return Data.DB.DbInstance.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            
        }

        /// <summary>
        /// 获取当前市场类型
        /// </summary>
        /// <param name="stockType"></param>
        /// <param name="sampledate"></param>
        /// <param name="markettype_before"></param>
        /// <returns></returns>
        public int GetMarketType(int stockType, int sampledate,out int markettype_before)
        {
            int marketType = 2;
            markettype_before = 2;
            //string sql = string.Format("select top 1 * from {0} where sampledate <={1} order by sampledate desc",GetTableName(),sampledate);
            DataView dv = list.DefaultView;
            dv.RowFilter = string.Format("sampledate<={0}",sampledate);
            dv.Sort = "sampledate desc";
            DataTable dt = dv.ToTable();
            //list.Select(string.Format("sampledate <={1} order by sampledate desc", sampledate));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["sampledate"].ToString() == sampledate.ToString())
                {
                    int.TryParse(dt.Rows[0]["markettype"].ToString(), out marketType);
                    int.TryParse(dt.Rows[0]["markettype_before"].ToString(), out markettype_before);
                }
                else
                {
                    int.TryParse(dt.Rows[0]["markettype"].ToString(), out marketType);
                    markettype_before = marketType;
                }
            }
            
            return marketType;
        }
    }
}
