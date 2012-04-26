using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class XlygBuz : BuzBase
    {
        public static XlygBuz Instance = new XlygBuz();

        public override string GetTableName(string stockCode)
        {
            return "xlyg_info";
            //return "hcwh_info_" + stockCode.Substring(stockCode.Length - 2);
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
                list[i].Dic["xlyg2"] = Get2(dtKLine, list, i);
                list[i].Dic["xlyg3"] = Get3(dtKLine, list, i);
                list[i].Dic["xlyg5"] = Get5(dtKLine, list, i);
                list[i].Dic["xlyg8"] = Get8(dtKLine, list, i);
                list[i].Dic["xlyg13"] = Get13(dtKLine, list, i);
                list[i].Dic["xlyg21"] = Get21(dtKLine, list, i);
                list[i].Dic["xlygup"] = GetUp(dtKLine, list, i);
                list[i].Dic["xlygdown"] = GetDown(dtKLine, list, i);
                list[i].Dic["xlygupgui"] = GetUpGui(dtKLine, list, i);
                list[i].Dic["xlygdowngui"] = GetDownGui(dtKLine, list, i);
                list[i].Dic["xlygstock"] = GetStock(dtKLine, list, i);
                list[i].Dic["xlygmoney"] = GetMoney(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);

        }


        public decimal Get2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //二:EMA(MA(C,5),2),COLOR00FF00;
            //若Y=EMA(X，N)，则Y=[2*X+(N-1)*Y’]/(N+1)，其中Y’表示上一周期的Y值
            int n = 5, m = 2;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg2", m, xlygarr);
        }


        public decimal Get3(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //三:EMA(MA(C,8),3),COLORFF00FF;
            int n = 8, m = 3;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg3", m, xlygarr);

        }

        public decimal Get5(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //五:EMA(MA(C,21),5),COLORYELLOW,LINETHICK2;
            int n = 21, m = 5;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg5", m, xlygarr);

        }

        public decimal Get8(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //八:EMA(MA(C,34),8),COLORWHITE;
            int n = 34, m = 8;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg8", m, xlygarr);

        }

        public decimal Get13(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //十三:EMA(MA(C,55),13),COLORRED;
            int n = 55, m = 13;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg13", m, xlygarr);
        }

        public decimal Get21(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //二十一:EMA(MA(C,89),21),COLORF0F000;
            int n = 89, m = 21;
            decimal ma = Formula.Ma(i, "powerc", n, dtKLine);
            return Formula.Ema(i, ma, "xlyg21", m, xlygarr);

        }

        public decimal GetUp(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //上:= 五+ 2*STD(CLOSE,20);
            int n = 20;
            decimal std = Formula.STD(i, "powerc", n, dtKLine);
            //$std = self::std($karr, $xlygarr, $i);
            return xlygarr[i].Dic["xlyg5"] + 2 * std;
        }
        public decimal GetDown(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //下:= 五- 2*STD(CLOSE,20);
            int n = 20;
            decimal std = Formula.STD(i, "powerc", n, dtKLine);
            //$std = self::std($karr, $xlygarr, $i);
            return xlygarr[i].Dic["xlyg5"] - 2 * std;
        }
        public decimal GetUpGui(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //上轨:EMA(上,3),COLOR0080FF,POINTDOT,LINETHICK2;
            return Formula.Ema(i, xlygarr[i].Dic["xlygup"], "xlygupgui", 3, xlygarr);
        }
        public decimal GetDownGui(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //下轨:EMA(下,3),COLOR0080FF,POINTDOT,LINETHICK2;
            return Formula.Ema(i, xlygarr[i].Dic["xlygdown"], "xlygdowngui", 3, xlygarr);
        }
        public decimal GetStock(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //持股:IF(C>=三 AND 三>REF(三,1),五,DRAWNULL),COLORRED,LINETHICK2;
            if (i > 0)
            {
                return dtKLine[i].Dic["powerc"] >= xlygarr[i].Dic["xlyg3"] && xlygarr[i].Dic["xlyg3"] > xlygarr[i - 1].Dic["xlyg3"] ? xlygarr[i].Dic["xlyg5"] : 0;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetMoney(IList<Model.StockModel> dtKLine, IList<Model.StockModel> xlygarr, int i)
        {
            //持币:IF(C<五 AND C<三 AND 三<REF(三,1),五,DRAWNULL),COLORGREEN,LINETHICK2;
            if (i > 0)
            {
                return dtKLine[i].Dic["powerc"] < xlygarr[i].Dic["xlyg5"] && dtKLine[i].Dic["powerc"] < xlygarr[i].Dic["xlyg3"] && xlygarr[i].Dic["xlyg3"] < xlygarr[i - 1].Dic["xlyg3"] ? xlygarr[i].Dic["xlyg5"] : 0;
            }
            else
            {
                return 0;
            }
        }
    }
}
