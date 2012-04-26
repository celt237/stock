using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class MonthPsy: BuzWeekBase
    {
        public static MonthPsy Instance = new MonthPsy();

        public override string GetTableName(string stockCode)
        {
            return "monthpsy_info";
            //return "psy_info_" + stockCode.Substring(stockCode.Length - 2);
        }


        public void DataDeal(IList<Model.WeekStockModel> dtKLine, bool powerChange, Model.stockinfo stock)
        {
            IList<Model.WeekStockModel> oldList = null;
            IList<Model.WeekStockModel> list = null;
            string stockCode = stock.stockcode;
            if (!powerChange)
            {
                oldList = GetListFromDb(stockCode);//	

            }
            if (oldList != null && oldList.Count > 0)
            {
                list = oldList;
            }
            else
            {
                list = new List<Model.WeekStockModel>();
            }
            int count = list.Count;
            for (int i = count; i < dtKLine.Count; i++)
            {
                Model.WeekStockModel model = new Model.WeekStockModel();
                list.Add(model);
                list[i].BeginDate = dtKLine[i].BeginDate;
                list[i].EndDate = dtKLine[i].EndDate;
                list[i].Dic["psy"] = GetPsy(dtKLine, i);
                list[i].Dic["psyma"] = GetPsyMa(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);
        }
        public decimal GetPsy(IList<Model.WeekStockModel> dtKLine, int i)
        {
            //PSY:COUNT(CLOSE>REF(CLOSE,1),N)/N*100;   N=12
            int count = 0, n = 12, m = 100;
            if (i >= n)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((decimal)dtKLine[i - j].Dic["powerc"] > (decimal)dtKLine[i - j - 1].Dic["powerc"])
                    {
                        count++;
                    }
                }
                return count * m / n;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetPsyMa(IList<Model.WeekStockModel> dtKLine, IList<Model.WeekStockModel> dtPsy, int i)
        {
            //PSYMA:MA(PSY,M);    M=6
            int m = 6;
            return Formula.Ma(i, "psy", m, dtPsy);
        }
    }
}
