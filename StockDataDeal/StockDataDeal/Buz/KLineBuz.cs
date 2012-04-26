using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Text.RegularExpressions;

namespace StockDataDeal.Buz
{
    public class KLineBuz:BuzBase
    {
        public static KLineBuz Instance = new KLineBuz();

        public override string GetTableName(string stockCode)
        {
            return "kline_info";
            //return "kline_info_" + stockCode.Substring(stockCode.Length - 2);
        }

        //public IList<Model.StockModel> GetKLineInfo(string stockcode, out IList<Model.power_info> powerList)
        //{
        //    string url = string.Format("http://hq.stock.sohu.com/cn/{0}/cn_{1}-6_2.html?uid={2}", stockcode.Substring(stockcode.Length - 3), stockcode, new Random().Next(1000000, 99999999));
        //    string content = Common.GetGeneralContent(url);
        //    var collection = Regex.Matches(content, @"PEAK_ODIA\(([^\)]*)\)");
        //    List<string> powerarr = new List<string>();
        //    List<string> karr = new List<string>();
        //    List<Model.StockModel> kLineList = new List<Model.StockModel>();
        //    powerList = new List<Model.power_info>();
        //    for (int i = 0; i < collection.Count; i++)
        //    {
        //        string value = collection[i].Value;
        //        if (value.Contains("dividend"))
        //        {
        //            //复权信息
        //            //$powerarr = array_merge($powerarr, explode(';', PowerHelper::dealPowerStr($arr[1][$i])));
        //            powerarr.AddRange(Buz.PowerBuz.DealPowerStr(value).Split(';').ToList<string>());
        //        }
        //        else if (value.Contains("quote_k_all") && !value.Contains("init") && !value.Contains("end"))
        //        {
        //            //k线信息
        //            //$karr = array_merge($karr, explode(';', KLineHelper::dealKLineStr($arr[1][$i])));
        //            karr.AddRange(Buz.KLineBuz.Instance.DealKLineStr(value).Split(';').ToList<string>());
        //        }
        //    }
        //    powerarr.Reverse();
        //    karr.Reverse();
        //    return karr;
        //}

        public string DealKLineStr(string value)
        {
            return value.Replace("PEAK_ODIA(","").Replace("\"", "").Replace("'", "").Replace("[quote_k_all,", "").Replace("[", "").Replace("],", ";").Replace("]", "").Replace(")", "");

        }

        public Model.StockModel GetLastKLineDataFromPage(string stockcode)
        {
            //获取最近一天的k线数据
            string url = string.Format("http://hq.stock.sohu.com/cn/{0}/cn_{1}-9_60m.html?uid={2}", stockcode.Substring(stockcode.Length - 3), stockcode, new Random().Next(1000000, 99999999));
            string content = Common.GetGeneralContent(url);
            var collection = Regex.Matches(content, @"PEAK_ODIA\(([^\)]*)\)");
            if (collection.Count > 0)
            {
                string value = collection[0].Value.Replace("PEAK_ODIA(", "").Replace("\"", "").Replace("'", "").Replace("[quote_60mk,", "").Replace("[", "").Replace("],", ";").Replace("]", "").Replace(")", "");
                string[] split = { ";" };
                string[] strList = value.Split(split, StringSplitOptions.RemoveEmptyEntries);

                return ConvertToWeekKLineObject(strList);
                
            }
            else 
            {
                return null;
            }
        }

        private Model.StockModel ConvertToWeekKLineObject(string[] strList)
        {
            Model.StockModel model = new Model.StockModel();

            if (strList.Length > 4)
            {
                #region model init
                model.Dic["h"] = 0;
                model.Dic["l"] = 0;
                model.Dic["cjl"] = 0;
                model.Dic["jye"] = 0;
                model.Dic["hsl"] = 0;
                model.Dic["je"] = 0;
                model.Dic["fd"] = 0;
                model.Dic["mid"] = 0;
                model.Dic["powerh"] = 0;
                model.Dic["powerl"] = 0;
                model.Dic["powermid"] = 0;
                #endregion
                for (int j = 0; j < 4; j++)
                {
                    string[] ret = strList[strList.Length - 1 - j].Split(',');
                    model.Sampledate = int.Parse("20" + ret[0].Substring(0, 6));
                    decimal tempD = 0;
                    
                    if (j == 0)
                    {
                        model.Dic["c"] = decimal.Parse(ret[2]);//收盘价
                        model.Dic["powerc"] = model.Dic["c"];
                    }
                    else if (j == 3)
                    {
                        model.Dic["o"] = decimal.Parse(ret[1]);//开盘价
                        model.Dic["powero"] = model.Dic["o"];
                    }

                    model.Dic["h"] = Math.Max(decimal.Parse(ret[3]), model.Dic["h"]);//最高价
                    if (model.Dic["l"] == 0)
                    {
                        model.Dic["l"] = decimal.Parse(ret[4]);//最低价
                    }
                    else
                    {
                        model.Dic["l"] = Math.Min(decimal.Parse(ret[4]), model.Dic["l"]); ;//最低价
                    }
                    
                    model.Dic["mid"] = (model.Dic["h"] + model.Dic["l"]) / 2;
                    model.Dic["powerh"] = model.Dic["h"];
                    model.Dic["powerl"] = model.Dic["l"];
                    model.Dic["powermid"] = model.Dic["mid"];

                    model.Dic["cjl"] += decimal.Parse(ret[5]);//成交量
                    model.Dic["jye"] += decimal.Parse(ret[6]);//交易额(万)

                    decimal.TryParse(ret[7].Replace("%", ""), out tempD);//换手率
                    model.Dic["hsl"] += tempD / 100;
                    model.Dic["je"] += decimal.Parse(ret[9]);//涨跌金额
                    decimal.TryParse(ret[8].Replace("%", ""), out tempD);//涨跌幅度
                    model.Dic["fd"] += tempD / 100;}
            }
            
            return model;
        }

        public Model.StockModel ConvertToKLineObject(string val)
        {
            string[] ret = val.Split(',');
            decimal tempD = 0;
            Model.StockModel model = new Model.StockModel();
            model.Sampledate = int.Parse(ret[0]);
            
            model.Dic["o"] = decimal.Parse(ret[1]);//开盘价
            model.Dic["c"] = decimal.Parse(ret[2]);//收盘价
            model.Dic["h"] = decimal.Parse(ret[3]);//最高价
            model.Dic["l"] = decimal.Parse(ret[4]);//最低价
            model.Dic["cjl"] = decimal.Parse(ret[5]);//成交量
            model.Dic["jye"] = decimal.Parse(ret[6]);//交易额(万)

            decimal.TryParse(ret[7].Replace("%",""),out tempD);//换手率
            model.Dic["hsl"] = tempD/100;
            model.Dic["je"] = decimal.Parse(ret[8]);//涨跌金额
            decimal.TryParse(ret[9].Replace("%", ""), out tempD);//换手率
            model.Dic["fd"] = tempD / 100;
            model.Dic["mid"] = (model.Dic["h"]+model.Dic["l"])/2;
            model.Dic["powerc"] = model.Dic["c"];
            model.Dic["powerh"] = model.Dic["h"];
            model.Dic["powerl"] = model.Dic["l"];
            model.Dic["powero"] = model.Dic["o"];
            model.Dic["powermid"] = model.Dic["mid"];
            return model;
        }
    }
}
