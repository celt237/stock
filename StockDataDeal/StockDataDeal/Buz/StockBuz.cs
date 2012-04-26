using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class StockBuz
    {
        public static IList<Model.stockinfo> GetList()
        {
            string sql = "select top 3 * from stockinfo where stockcode='600997'";
            return ToList(Data.DB.DbInstance.ExecuteDataSet(CommandType.Text, sql).Tables[0]);
        }

        private static IList<Model.stockinfo> ToList(DataTable dt)
        {
            IList<Model.stockinfo> list = new List<Model.stockinfo>();
            foreach (DataRow dr in dt.Rows)
            {
                Model.stockinfo model = new Model.stockinfo();
                model.begindate = int.Parse(dr["begindate"].ToString());
                model.fullshare = decimal.Parse(dr["fullshare"].ToString());
                model.industry = dr["industry"].ToString();
                model.isnewstock = int.Parse(dr["isnewstock"].ToString());
                model.pyforshort = dr["pyforshort"].ToString();
                model.risktype = int.Parse(dr["risktype"].ToString());
                model.status = int.Parse(dr["status"].ToString());
                model.stockcode = dr["stockcode"].ToString();
                model.stockname = dr["stockname"].ToString();
                model.stocktype = int.Parse(dr["stocktype"].ToString());
                model.tradableshare = decimal.Parse(dr["tradableshare"].ToString());
                list.Add(model);
            }
            return list;
        }
    }
}
