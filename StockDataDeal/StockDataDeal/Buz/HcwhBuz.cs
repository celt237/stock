using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class HcwhBuz:BuzBase
    {
        public static HcwhBuz Instance = new HcwhBuz();

        public override string GetTableName(string stockCode)
        {
            return "hcwh_info";
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
                list[i].Dic["upline"] = GetUpLine(dtKLine, list, i);
                list[i].Dic["downline"] = GetDownLine(dtKLine, list, i);
                list[i].Dic["upline1"] = GetUpLine1(dtKLine, list, i);
                list[i].Dic["downline1"] = GetDownLine1(dtKLine, list, i);
                list[i].Dic["bbi"] = GetBbi(dtKLine, i);
                list[i].Dic["upr"] = GetUpr(dtKLine, list, i);
                list[i].Dic["dwn"] = GetDwn(dtKLine, list, i);
                list[i].Dic["safe"] = GetSafe(dtKLine, i);
                list[i].Dic["lc"] = GetLc(dtKLine, i);
                list[i].Dic["rsifz"] = GetRsiFz(dtKLine, list, i);
                list[i].Dic["rsifm"] = GetRsiFm(dtKLine, list, i);
                list[i].Dic["rsi"] = GetRsi(dtKLine, list, i);
                list[i].Dic["a7"] = GetA7(dtKLine, list, i);
                list[i].Dic["opt"] = GetOpt(dtKLine, list, i);
                list[i].Dic["opt1"] = GetOpt1(dtKLine, list, i);
                list[i].Dic["opt2"] = GetOpt2(dtKLine, list, i);
                list[i].Dic["var1"] = GetVar1(dtKLine, list, i);
                list[i].Dic["var2"] = GetVar2(dtKLine, list, i);
                list[i].Dic["sk"] = GetSk(dtKLine, list, i);
                list[i].Dic["sd"] = GetSd(dtKLine, list, i);
                list[i].Dic["d"] = GetD(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);
            
        }

        public decimal GetUpLine(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SMA(C,6.5,1);
            return Formula.Sma(i, (decimal)dtKLine[i].Dic["powerc"], "upline", (decimal)6.5, (decimal)1, hcwharr);
        }
        public decimal GetDownLine(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SMA(C,13.5,1);
            return Formula.Sma(i, (decimal)dtKLine[i].Dic["powerc"], "downline", (decimal)13.5, (decimal)1, hcwharr);
        }
        public decimal GetUpLine1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SMA(C,3,1);
            return Formula.Sma(i, (decimal)dtKLine[i].Dic["powerc"], "upline1", 3, 1, hcwharr);
        }
        public decimal GetDownLine1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SMA(C,8,1);
            return Formula.Sma(i, (decimal)dtKLine[i].Dic["powerc"], "downline1", 8, 1, hcwharr);
        }

        public decimal GetBbi(IList<Model.StockModel> dtKLine, int i)
        {
            //BBI:=(MA(CLOSE,3)+MA(CLOSE,6)+MA(CLOSE,12)+MA(CLOSE,24))/4;
            return (Formula.Ma(i, "powerc", 3, dtKLine) + Formula.Ma(i, "powerc", 6, dtKLine) + Formula.Ma(i, "powerc", 12, dtKLine) + Formula.Ma(i, "powerc", 24, dtKLine)) / 4;
        }

        public decimal GetUpr(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //UPR:=BBI+3*STD(BBI,13),LINETHICK2;
            return (decimal)hcwharr[i].Dic["bbi"] + 3 * (decimal)Formula.STD(i, "bbi", 13, hcwharr);
        }
        public decimal GetDwn(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //DWN:=BBI-3*STD(BBI,13);
            return (decimal)hcwharr[i].Dic["bbi"] - 3 * (decimal)Formula.STD(i, "bbi", 13, hcwharr);
        }
        public decimal GetSafe(IList<Model.StockModel> dtKLine, int i)
        {
            //安全:=MA(CLOSE,60),LINETHICK2;
            return Formula.Ma(i, "powerc", 60, dtKLine);
        }
        public decimal GetLc(IList<Model.StockModel> dtKLine, int i)
        {
            //LC:=REF(CLOSE,1);
            return Formula.Ref(i, "powerc", 1, dtKLine);
        }
        public decimal GetRsiFz(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            decimal x1 = Math.Max((decimal)dtKLine[i].Dic["powerc"] - (decimal)hcwharr[i].Dic["lc"], 0);
            return Formula.Sma(i, x1, "rsifz", 6, 1, hcwharr);
        }
        public decimal GetRsiFm(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            decimal x2 = Math.Abs((decimal)dtKLine[i].Dic["powerc"] - (decimal)hcwharr[i].Dic["lc"]);
            return Formula.Sma(i, x2, "rsifm", 6, 1, hcwharr);
        }
        public decimal GetRsi(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //RSI:=SMA(MAX(CLOSE-LC,0),6,1)/SMA(ABS(CLOSE-LC),6,1)*100; 
            return (decimal)hcwharr[i].Dic["rsifz"] * 100 / (decimal)hcwharr[i].Dic["rsifm"];
        }
        public decimal GetA7(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //A7:=(2*C+H+L)/4;
            return (2 * (decimal)dtKLine[i].Dic["powerc"] + (decimal)dtKLine[i].Dic["powerh"] + (decimal)dtKLine[i].Dic["powerl"]) / 4;
        }
        public decimal GetOpt(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //操作:MA(A7,5),LINETHICK2;
            return Formula.Ma(i, "a7", 5, hcwharr);
        }
        public decimal GetOpt1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //操作1:=MA(A7,5)*1.03,LINETHICK2;
            return (decimal)((decimal)hcwharr[i].Dic["opt"] * (decimal)1.03);
        }
        public decimal GetOpt2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //操作2:=MA(A7,5)*0.97,LINETHICK2;
            return (decimal)((decimal)hcwharr[i].Dic["opt"] * (decimal)0.97);
        }
        public decimal GetVar1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //VAR1:LLV(A7,21);
            return (decimal)Formula.LLV(i, "a7", 21, hcwharr);
        }
        public decimal GetVar2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //VAR2:HHV(A7,21);
            return (decimal)Formula.HHV(i, "a7", 21, hcwharr);
        }
        public decimal GetSk(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SK:=EMA((A7-VAR1)/(VAR2-VAR1)*100,7);
            decimal fm = ((decimal)hcwharr[i].Dic["var2"] - (decimal)hcwharr[i].Dic["var1"]);
            decimal x = fm != 0 ? ((decimal)hcwharr[i].Dic["a7"] - (decimal)hcwharr[i].Dic["var1"]) * 100 / fm : 0;
            return Formula.Ema(i, x, "sk", 7, hcwharr);
        }
        public decimal GetSd(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //SD:=EMA(0.667*REF(SK,1)+0.333*SK,5);
            decimal x = (decimal)((decimal)0.667 * Formula.Ref(i, "sk", 1, hcwharr) + (decimal)0.333 * (decimal)hcwharr[i].Dic["sk"]);
            return Formula.Ema(i, x, "sd", 5, hcwharr);
        }
        public decimal GetD(IList<Model.StockModel> dtKLine, IList<Model.StockModel> hcwharr, int i)
        {
            //D:=MA(CLOSE,80)-MA(CLOSE,10)/3; 
            return Formula.Ma(i, "powerc", 80, dtKLine) - (Formula.Ma(i, "powerc", 10, dtKLine) / 3);
        }
    }
}