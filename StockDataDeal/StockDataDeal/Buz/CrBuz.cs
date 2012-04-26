using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class CrBuz:BuzBase
    {
        public static CrBuz Instance = new CrBuz();

        public override string GetTableName(string stockCode)
        {
            return "cr_info";
            //return "cr_info_" + stockCode.Substring(stockCode.Length - 2);
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
                list[i].Dic["mid"] = GetCrMid(dtKLine, i);
                list[i].Dic["cr"] = GetCr(dtKLine, list, i);
                list[i].Dic["ma1"] = GetCrMa1(dtKLine, list, i);
                list[i].Dic["ma2"] = GetCrMa2(dtKLine, list, i);
                list[i].Dic["ma3"] = GetCrMa3(dtKLine, list, i);
                list[i].Dic["ma4"] = GetCrMa4(dtKLine, list, i);
                list[i].Status = 1;
                //SaveData(crarr[i], stockCode, powerChange);
            }
            BatchSaveData(list, stockCode, powerChange);
        }

        public decimal GetCrMid(IList<Model.StockModel> dtKLine, int index)
        {
            if (index >= 1)
            {
                return ((decimal)dtKLine[index - 1].Dic["powerh"] + (decimal)dtKLine[index - 1].Dic["powerl"]) / 2;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetCr(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCr, int index)
        {
            decimal qiang = 0;
            decimal ruo = 0;
            int n = 26, m = 100;
            if (index >= n)
            {
                for (int j = 0; j < n; j++)
                {
                    qiang += Math.Max(0, (decimal)dtKLine[index - j].Dic["powerh"] - (decimal)dtCr[index - j].Dic["mid"]);
                    ruo += Math.Max(0, (decimal)dtCr[index - j].Dic["mid"] - (decimal)dtKLine[index - j].Dic["powerl"]);
                }
                return ruo > 0 ? qiang * m / ruo : 0;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetCrMa1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCr, int index)
        {
            int n = 10, m = 5;
            return Formula.RefMa(index, "cr", n, m, dtCr);
        }

        public decimal GetCrMa2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCr, int index)
        {
            int n = 20, m = 9;
            return Formula.RefMa(index, "cr", n, m, dtCr);
        }
        public decimal GetCrMa3(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCr, int index)
        {
            int n = 40, m = 17;
            return Formula.RefMa(index, "cr", n, m, dtCr);
        }
        public decimal GetCrMa4(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCr, int index)
        {
            int n = 62, m = 25;
            return Formula.RefMa(index, "cr", n, m, dtCr);
        }
    }
}
