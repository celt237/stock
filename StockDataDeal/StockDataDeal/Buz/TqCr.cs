using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class TqCr:BuzBase
    {
        public static TqCr Instance = new TqCr();

        public override string GetTableName(string stockCode)
        {
            return "tqcr_info";
            //return "tqcr_info_" + stockCode.Substring(stockCode.Length - 2);
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
                list[i].Dic["mid"] = GetTqCrMid(dtKLine, i);
                list[i].Dic["cr"] = GetTqCr(dtKLine, list, i);
                list[i].Dic["ma1"] = GetTqCrMa1(dtKLine, list, i);
                list[i].Dic["ma2"] = GetTqCrMa2(dtKLine, list, i);
                list[i].Dic["ma3"] = GetTqCrMa3(dtKLine, list, i);
                list[i].Dic["ma4"] = GetTqCrMa4(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);
        }
        public decimal GetTqCrMid(IList<Model.StockModel> dtKLine, int i)
        {
            if (i >= 1)
            {
                return ((decimal)dtKLine[i - 1].Dic["powerh"] + (decimal)dtKLine[i - 1].Dic["powerc"]) / 2;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetTqCr(IList<Model.StockModel> dtKLine, IList<Model.StockModel> tqcrarr, int i)
        {
            //CR:SUM(MAX(0,HIGH-MID),13)/SUM(MAX(0,MID-L),13)*233,COLORFFFFFF;
            decimal qiang = 0, ruo = 0;
            int n = 13, m = 233;
            if (i >= n)
            {
                for (int j = 0; j < n; j++)
                {
                    qiang += Math.Max(0, (decimal)dtKLine[i - j].Dic["powerh"] - (decimal)tqcrarr[i - j].Dic["mid"]);
                    ruo += Math.Max(0, (decimal)tqcrarr[i - j].Dic["mid"] - (decimal)dtKLine[i - j].Dic["powerl"]);
                }
                return ruo > 0 ? qiang * m / ruo : 0;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetTqCrMa1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> tqcrarr, int i)
        {
            //MA1:REF(MA(CR,8),3),COLOR00FFFF;
            int n = 8, m = 3;
            return Formula.RefMa(i, "cr", n, m, tqcrarr);
        }
        public decimal GetTqCrMa2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> tqcrarr, int i)
        {
            //MA2:REF(MA(CR,13),5),COLORFF00FF;
            int n = 13, m = 5;
            return Formula.RefMa(i, "cr", n, m, tqcrarr);
        }
        public decimal GetTqCrMa3(IList<Model.StockModel> dtKLine, IList<Model.StockModel> tqcrarr, int i)
        {
            //MA3:REF(MA(CR,21),8),COLOR00FF00;
            int n = 21, m = 8;
            return Formula.RefMa(i, "cr", n, m, tqcrarr);
        }
        public decimal GetTqCrMa4(IList<Model.StockModel> dtKLine, IList<Model.StockModel> tqcrarr, int i)
        {
            //MA4:REF(MA(CR,34),13),COLOR0066CC;
            int n = 34, m = 13;
            return Formula.RefMa(i, "cr", n, m, tqcrarr);
        }
    }
}
