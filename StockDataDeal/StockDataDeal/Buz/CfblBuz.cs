using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class CfblBuz:BuzBase
    {
        public static CfblBuz Instance = new CfblBuz();

        public override string GetTableName(string stockCode)
        {
            return "cfbl_info";
            //return "cfbl_info_" + stockCode.Substring(stockCode.Length - 2);
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
                list[i].Dic["rsv"] = GetRsv(dtKLine, list, i);
                list[i].Dic["k"] = GetK(dtKLine, list, i);
                list[i].Dic["d"] = GetD(dtKLine, list, i);
                list[i].Dic["j"] = GetJ(dtKLine, list, i);
                list[i].Dic["var1"] = GetVar1(dtKLine, list, i);
                list[i].Dic["var2"] = GetVar2(dtKLine, list, i);
                list[i].Dic["var3"] = GetVar3(dtKLine, list, i);
                decimal low = Formula.LLV(i, "powerl", 27, dtKLine);
                decimal high = Formula.HHV(i, "powerh", 27, dtKLine);
                list[i].Dic["var4sma1"] = GetVar4Sma1(low, high, dtKLine, list, i);
                list[i].Dic["var4sma2"] = GetVar4Sma2(low, high, dtKLine, list, i);
                list[i].Dic["var4sma3"] = GetVar4Sma3(low, high, dtKLine, list, i);
                list[i].Dic["var4"] = GetVar4(dtKLine, list, i);
                list[i].Dic["var5"] = GetVar5(dtKLine, list, i);
                list[i].Dic["var6"] = GetVar6(dtKLine, list, i);
                list[i].Dic["var7"] = GetVar7(dtKLine, list, i);
                list[i].Dic["var8"] = GetVar8(dtKLine, list, i);
                list[i].Dic["var9"] = GetVar9(dtKLine, list, i);
                
                list[i].Dic["vara"] = GetVarA(dtKLine, list, i);
                list[i].Dic["varb"] = GetVarB(dtKLine, list, i);
                list[i].Dic["varc"] = GetVarC(dtKLine, list, i, stock.tradableshare);
                list[i].Dic["vard"] = GetVarD(dtKLine, list, i);
                list[i].Dic["vare"] = GetVarE(dtKLine, list, i);
                list[i].Dic["varf"] = GetVarF(dtKLine, list, i);
                list[i].Dic["var10"] = GetVar10(dtKLine, list, i);
                list[i].Dic["buy"] = GetBuy(dtKLine, list, i);
                list[i].Dic["high"] = 80;
                list[i].Dic["middle"] = 50;
                list[i].Dic["bottom"] = 10;
                list[i].Dic["waveline"] = GetWaveLine(dtKLine, list, i);
                list[i].Dic["referline"] = GetReferLine(dtKLine, list, i);
                list[i].Dic["openeye"] = GetOpenEye(dtKLine, list, i);
                list[i].Dic["a1"] = GetA1(dtKLine, list, i);
                list[i].Dic["a2"] = GetA2(dtKLine, list, i);
                list[i].Dic["a3"] = GetA3(dtKLine, list, i);
                list[i].Dic["a4"] = GetA4(dtKLine, list, i);
                list[i].Dic["a5"] = GetA5(dtKLine, list, i);
                list[i].Dic["i"] = GetI(dtKLine, list, i);
                list[i].Dic["r"] = GetR(dtKLine, list, i);
                list[i].Status = 1;
            }
            BatchSaveData(list, stockCode, powerChange);
            
        }

        public decimal GetRsv(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            var low = Formula.LLV(index, "powerl", 9, dtKLine);
            var fm = Formula.HHV(index, "powerh", 9, dtKLine) - low;
            return fm != 0 ? (dtKLine[index].Dic["powerc"] - low) * 100 / fm : 0;
        }

        public decimal GetK(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["rsv"], "k", 3, 1, dtCfbl);
        }

        public decimal GetD(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["k"], "d", 3, 1, dtCfbl);
        }

        public decimal GetJ(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return 3 * dtCfbl[index].Dic["k"] - 2 * dtCfbl[index].Dic["d"];
        }

        public decimal GetVar1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            var low = Formula.LLV(index, "powerl", 45, dtKLine);
            var fm = Formula.HHV(index, "powerh", 45, dtKLine) - low;
            return fm != 0 ? (dtKLine[index].Dic["powerc"] - low) * 100 / fm : 0;
        }

        public decimal GetVar2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["var1"], "var2", 3, 1, dtCfbl);
        }

        public decimal GetVar3(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["var2"], "var3", 3, 1, dtCfbl);
        }

        public decimal GetVar4Sma1(decimal low, decimal high, IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            var x1 = high == low ? 0 : (dtKLine[index].Dic["powerc"] - low) / (high - low * 100);
            return Formula.Sma(index, x1, "var4sma1", 5, 1, dtCfbl);
        }

        public decimal GetVar4Sma2(decimal low, decimal high, IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            var x2 = high == low ? 0 : (dtKLine[index].Dic["powerc"] - low) / (high - low * 100);
            return Formula.Sma(index, x2, "var4sma2", 5, 1, dtCfbl);
        }

        public decimal GetVar4Sma3(decimal low, decimal high, IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["var4sma2"], "var4sma3", 3, 1, dtCfbl);
        }

        public decimal GetVar4(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return Formula.Sma(index, dtCfbl[index].Dic["var4sma2"], "var4sma3", 3, 1, dtCfbl);
        }

        public decimal GetVar5(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return dtCfbl[index].Dic["var4"] > 10 && dtCfbl[index - 1].Dic["var4"] < 10 && dtCfbl[index].Dic["var3"] < 12 ? 1 : 0;
        }

        public decimal GetBuy(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            if (index <= 0)
            {
                return 0;
            }
            else 
            {
                return dtCfbl[index].Dic["var4"] < 10 && dtCfbl[index - 1].Dic["var4"] > 10 ? 1 : 0;
            }
            

        }

        public decimal GetVar6(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            return (2 * dtKLine[index].Dic["powerc"] + dtKLine[index].Dic["powerh"] + dtKLine[index].Dic["powerl"]) / 4;
        }

        public decimal GetVar7(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::llv(i,dtKLine,'powerl',20);
            return Formula.LLV(index, "powerl", 20, dtKLine);

        }

        public decimal GetVar8(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::hhv(i,dtKLine,'powerh',20);
            return Formula.HHV(index, "powerh", 20, dtKLine);
        }

        public decimal GetVar9(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //$fm = (cfblarr.Rows[i]["var8-cfblarr.Rows[i]["var7);
            //$x = $fm!=0?(cfblarr.Rows[i]["var6-cfblarr.Rows[i]["var7)/$fm * 100:0;

            //return Formula::ema(i,$x,'var9',13,cfblarr);

            var fm = dtCfbl[index].Dic["var8"] - dtCfbl[index].Dic["var7"];
            var x = fm != 0 ? (dtCfbl[index].Dic["var6"] - dtCfbl[index].Dic["var7"]) / fm * 100 : 0;
            return Formula.Ema(index, x, "var9", 13, dtCfbl);
        }

        public decimal GetVarA(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //$x = 0.667*Formula::ref(i,'var9',1,cfblarr)+0.333*cfblarr.Rows[i]["var9;
            //return Formula::ema(i,$x,'vara',2,cfblarr);
            decimal x = (decimal)((decimal)0.667 * Formula.Ref(index, "var9", 1, dtCfbl) + (decimal)0.333 * dtCfbl[index].Dic["var9"]);
            return Formula.Ema(index, x, "vara", 2, dtCfbl);
        }

        public decimal GetWaveLine(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return cfblarr.Rows[i]["var9;
            return dtCfbl[index].Dic["var9"];
        }

        public decimal GetReferLine(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //$x = 0.382*Formula::ref(i,'var9',2,cfblarr)+0.618*cfblarr.Rows[i]["var9;
            //return Formula::ema(i,$x,'referline',12,cfblarr);
            var x = (decimal)((decimal)0.382 * Formula.Ref(index, "var9", 2, dtCfbl) + (decimal)0.618 * dtCfbl[index].Dic["var9"]);
            return Formula.Ema(index, x, "referline", 12, dtCfbl);
        }

        public decimal GetVarB(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //    $refV = Formula::ref(i,'powerc',7,dtKLine);
            //$x = dtKLine[i]->powerc<$refV?0-(dtKLine[i]->cjl/100):0;
            //$x = dtKLine[i]->powerc>$refV?dtKLine[i]->cjl/100:$x;
            //if(i>0){
            //    return $x+cfblarr[i-1]->varb;
            //}
            //else{
            //    return $x;
            //}
            var refV = Formula.Ref(index, "powerc", 7, dtKLine);
            var x = dtKLine[index].Dic["powerc"] < refV ? 0 - (dtKLine[index].Dic["cjl"] / 100) : 0;
            x = dtKLine[index].Dic["powerc"] > refV ? dtKLine[index].Dic["cjl"] / 100 : x;
            if (index > 0)
            {
                return x + dtCfbl[index - 1].Dic["varb"];
            }
            else
            {
                return x;
            }
        }

        public int GetVarC(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index, decimal capital)
        {
            //    $x = dtKLine[i]->cjl;

            //if(i>0){
            //    if(cfblarr[i-1]->varc>0){
            //        cfblarr.Rows[i]["vol = cfblarr[i-1]->vol;
            //        return cfblarr[i-1]->varc+1;
            //    }
            //    else{
            //        cfblarr.Rows[i]["vol = cfblarr[i-1]->vol + $x;
            //        return cfblarr[i-1]->vol>$capital?1:0; 

            //    }
            //}
            //else{
            //    cfblarr.Rows[i]["vol = $x;
            //    return 0;
            //}
            var x = (decimal)dtKLine[index].Dic["cjl"];
            if (index > 0)
            {
                if ((decimal)dtCfbl[index - 1].Dic["varc"] > 0)
                {
                    dtCfbl[index].Dic["vol"] = dtCfbl[index - 1].Dic["vol"];
                    return (int)dtCfbl[index - 1].Dic["varc"] + 1;
                }
                else
                {
                    dtCfbl[index].Dic["vol"] = dtCfbl[index - 1].Dic["vol"] + x;
                    return (int)dtCfbl[index - 1].Dic["vol"] > capital ? 1 : 0;
                }
            }
            else
            {
                dtCfbl[index].Dic["vol"] = x;
                return 0;
            }
        }

        public  int GetVarD(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return dtKLine[i]->powerc > Formula::llv(i,dtKLine,'powerc',cfblarr.Rows[i]["varc)?1:-1;
            return dtKLine[index].Dic["powerc"] > Formula.LLV(index, "powerc", (int)dtCfbl[index].Dic["varc"], dtKLine) ? 1 : -1;
        }

        public  int GetVarE(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return cfblarr.Rows[i]["varb > Formula::llv(i,cfblarr,'varb',cfblarr.Rows[i]["varc)?1:-1;
            return dtCfbl[index].Dic["varb"] > Formula.LLV(index, "varb", (int)dtCfbl[index].Dic["varc"], dtCfbl) ? 1 : -1;
        }

        public  int GetVarF(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return cfblarr.Rows[i]["vard*cfblarr.Rows[i]["vare;
            return (int)dtCfbl[index].Dic["vard"] * (int)dtCfbl[index].Dic["vare"];
        }

        public  int GetVar10(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return cfblarr.Rows[i]["varf==-1?1:0;
            return (int)dtCfbl[index].Dic["varf"] == -1 ? 1 : 0;
        }

        public  int GetOpenEye(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return cfblarr.Rows[i]["var10 && dtKLine[i]->powerc==Formula::llv(i,dtKLine,'powerc',20) && cfblarr.Rows[i]["var3<12?70:1;
            return (int)dtCfbl[index].Dic["var10"] == 1 && dtKLine[index].Dic["powerc"] == Formula.LLV(index, "powerc", 20, dtKLine) && dtCfbl[index].Dic["var3"] < 12 ? 70 : 1;
        }

        public  decimal GetA1(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::ema(i,dtKLine[i]->powerc,'a1',5,cfblarr);
            return Formula.Ema(index, dtKLine[index].Dic["powerc"], "a1", 5, dtCfbl);
        }

        public  decimal GetA2(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::ema(i,dtKLine[i]->powerc,'a2',10,cfblarr);
            return Formula.Ema(index, dtKLine[index].Dic["powerc"], "a2", 10, dtCfbl);
        }

        public  decimal GetA3(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::ema(i,dtKLine[i]->powerc,'a3',20,cfblarr);
            return Formula.Ema(index, dtKLine[index].Dic["powerc"], "a3", 20, dtCfbl);
        }

        public decimal GetA4(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::ema(i,dtKLine[i]->powerc,'a4',40,cfblarr);
            return Formula.Ema(index, dtKLine[index].Dic["powerc"], "a4", 40, dtCfbl);
        }

        public decimal GetA5(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //return Formula::ema(i,dtKLine[i]->powerc,'a5',60,cfblarr);
            return Formula.Ema(index, dtKLine[index].Dic["powerc"], "a5", 60, dtCfbl);
        }

        public decimal GetI(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //    cfblarr.Rows[i]["highandhhv = (dtKLine[i]->powerh==Formula::hhv(i,dtKLine,'powerh',16))?1:0;
            //if(i>0){
            //    if(cfblarr[i-1]->highandhhv==1){
            //        cfblarr.Rows[i]["highandhhvnum = 1;
            //    }
            //    elseif(cfblarr[i-1]->highandhhvnum>0){
            //        cfblarr.Rows[i]["highandhhvnum = cfblarr[i-1]->highandhhvnum+1;
            //    }
            //    else{
            //        cfblarr.Rows[i]["highandhhvnum = 0;	
            //    }
            //}
            //else{
            //    cfblarr.Rows[i]["highandhhvnum = 0;
            //}
            //cfblarr.Rows[i]["mai = cfblarr.Rows[i]["highandhhvnum * (dtKLine[i]->powerc<dtKLine[i]->powero?dtKLine[i]->cjl/100:1);
            //return Formula::ma(i,'mai',3,cfblarr);
            dtCfbl[index].Dic["highandhhv"] = (dtKLine[index].Dic["powerh"] == Formula.HHV(index, "powerh", 16, dtKLine)) ? 1 : 0;
            if (index > 0)
            {
                if ((int)dtCfbl[index - 1].Dic["highandhhv"] == 1)
                {
                    dtCfbl[index].Dic["highandhhvnum"] = 1;
                }
                else if ((int)dtCfbl[index - 1].Dic["highandhhvnum"] > 0)
                {
                    dtCfbl[index].Dic["highandhhvnum"] = (int)dtCfbl[index - 1].Dic["highandhhvnum"] + 1;
                }
                else
                {
                    dtCfbl[index].Dic["highandhhvnum"] = 0;
                }
            }
            else
            {
                dtCfbl[index].Dic["highandhhvnum"] = 0;
            }
            dtCfbl[index].Dic["mai"] = (int)dtCfbl[index].Dic["highandhhvnum"] * (dtKLine[index].Dic["powerc"] < dtKLine[index].Dic["powero"] ? dtKLine[index].Dic["cjl"] / 100 : 1);
            return Formula.Ma(index, "mai", 3, dtCfbl);
        }

        public decimal GetR(IList<Model.StockModel> dtKLine, IList<Model.StockModel> dtCfbl, int index)
        {
            //    cfblarr.Rows[i]["lowandllv = dtKLine[i]->powerl==Formula::llv(i,dtKLine,'powerl',16)?1:0;
            //if(i>0){
            //    if(cfblarr[i-1]->lowandllv==1){
            //        cfblarr.Rows[i]["lowandllvnum = 1;
            //    }
            //    elseif(cfblarr[i-1]->lowandllvnum>0){
            //        cfblarr.Rows[i]["lowandllvnum = cfblarr[i-1]->lowandllvnum+1;
            //    }
            //    else{
            //        cfblarr.Rows[i]["lowandllvnum = 0;	
            //    }
            //}
            //else{
            //    cfblarr.Rows[i]["lowandllvnum = 0;
            //}
            //cfblarr.Rows[i]["mar = cfblarr.Rows[i]["lowandllvnum * (dtKLine[i]->powerc>dtKLine[i]->powero?dtKLine[i]->cjl/100:1);
            //return Formula::ma(i,'mar',4,cfblarr);
            dtCfbl[index].Dic["lowandllv"] = dtKLine[index].Dic["powerl"] == Formula.LLV(index, "powerl", 16, dtKLine) ? 1 : 0;
            if (index > 0)
            {
                if ((int)dtCfbl[index - 1].Dic["lowandllv"] == 1)
                {
                    dtCfbl[index].Dic["lowandllvnum"] = 1;
                }
                else if ((int)dtCfbl[index - 1].Dic["lowandllvnum"] > 0)
                {
                    dtCfbl[index].Dic["lowandllvnum"] = (int)dtCfbl[index - 1].Dic["lowandllvnum"] + 1;
                }
                else
                {
                    dtCfbl[index].Dic["lowandllvnum"] = 0;
                }
            }
            else
            {
                dtCfbl[index].Dic["lowandllvnum"] = 0;
            }
            dtCfbl[index].Dic["mar"] = (int)dtCfbl[index].Dic["lowandllvnum"] * (dtKLine[index].Dic["powerc"] > dtKLine[index].Dic["powero"] ? dtKLine[index].Dic["cjl"] / 100 : 1);
            return Formula.Ma(index, "mar", 4, dtCfbl);
        }
    }
}
