using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class StockAnalyse
    {
        public static StockAnalyse Instance = new StockAnalyse();

        public void AnalyseDeer(string stockcode, int currentDate)
        { 
        }

        public void AnalyseBear(string stockcode,int markettype_before, int currentDate)
        {
            object[] array = { stockcode,markettype_before, currentDate };
            Data.DB.DbInstance.ExecuteNonQuery("AnalyseBear", array);

        }

        public void AnalyseOx(string stockcode, int currentDate)
        {
        }
    }
}
