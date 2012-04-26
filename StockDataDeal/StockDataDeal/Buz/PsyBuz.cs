using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class PsyBuz : BuzBase
    {
        public static PsyBuz Instance = new PsyBuz();

        public override string GetTableName(string stockCode)
        {
            return "psy_info";
            //return "psy_info_" + stockCode.Substring(stockCode.Length - 2);
        }


        public void DataDeal(IList<Model.StockModel> dtKLine, bool powerChange, Model.stockinfo stock)
        {
            IList<Model.StockModel> oldList = null;
            IList<Model.StockModel> list = null;
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
                list = new List<Model.StockModel>();
            }
            int count = list.Count;
            for (int i = count; i < dtKLine.Count; i++)
            {
                Model.StockModel model = new Model.StockModel();
                list.Add(model);
                list[i].Sampledate = dtKLine[i].Sampledate;
                list[i].Dic["psy"] = GetPsy(dtKLine, i);
                list[i].Dic["psyma"] = GetPsyMa(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);
        }
        public decimal GetPsy(IList<Model.StockModel> dtKLine, int i)
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
        public decimal GetPsyMa(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtPsy, int i)
        {
            //PSYMA:MA(PSY,M);    M=6
            int m = 6;
            return Formula.Ma(i, "psy", m, dtPsy);
        }
    }
}
