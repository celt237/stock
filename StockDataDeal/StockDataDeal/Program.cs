using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace StockDataDeal
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ZhuaQu();
            Analyse();
            //Buz.ImportStock.Import();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalSeconds);


        }


        /// <summary>
        /// 抓数据
        /// </summary>
        static void ZhuaQu()
        {
            //取出股票列表
            IList<Model.stockinfo> dtStock = Buz.StockBuz.GetList();
            //遍历
            for (int i = 0; i < dtStock.Count; i++)
            {
                FetchData(dtStock[i]);
            }
            //

        }

        static void FetchData(Model.stockinfo dr)
        {
            string stockcode = dr.stockcode;
            bool powerchange = false;

            Model.StockModel lastKLineModel = Buz.KLineBuz.Instance.GetLastKLineDataFromPage(stockcode);

            //获取历史k线数据
            string url = string.Format("http://hq.stock.sohu.com/cn/{0}/cn_{1}-6_2.html?uid={2}", stockcode.Substring(stockcode.Length - 3), stockcode, new Random().Next(1000000, 99999999));
            string content = Common.GetGeneralContent(url);
            var collection = Regex.Matches(content, @"PEAK_ODIA\(([^\)]*)\)");
            List<string> powerarr = new List<string>();
            List<string> karr = new List<string>();
            List<Model.StockModel> kLineList = new List<Model.StockModel>();
            List<Model.power_info> powerList = new List<Model.power_info>();
            for (int i = 0; i < collection.Count; i++)
            {
                string value = collection[i].Value;
                if (value.Contains("dividend"))
                {
                    //复权信息
                    //$powerarr = array_merge($powerarr, explode(';', PowerHelper::dealPowerStr($arr[1][$i])));
                    powerarr.AddRange(Buz.PowerBuz.DealPowerStr(value).Split(';').ToList<string>());
                }
                else if (value.Contains("quote_k_all") && !value.Contains("init") && !value.Contains("end"))
                {
                    //k线信息
                    //$karr = array_merge($karr, explode(';', KLineHelper::dealKLineStr($arr[1][$i])));
                    karr.AddRange(Buz.KLineBuz.Instance.DealKLineStr(value).Split(';').ToList<string>());
                }
            }
            powerarr.Reverse();
            karr.Reverse();
            int lastpowerdate = Buz.PowerBuz.GetLastDate(stockcode);
            //转为实体list
            for (int i = 0; i < powerarr.Count; i++)
            {
                Model.power_info model = Buz.PowerBuz.ConvertToPowerObject(powerarr[i], stockcode);
                powerList.Add(model);

                if (model.sampledate > lastpowerdate)
                {
                    Buz.PowerBuz.SaveData(model);
                    powerchange = true;
                }
            }
            IList<Model.StockModel> oldkarr = null;
            if (!powerchange)
            {
                oldkarr = Buz.KLineBuz.Instance.GetListFromDb(stockcode);
            }


            for (int i = 0; i < karr.Count; i++)
            {
                Model.StockModel model = Buz.KLineBuz.Instance.ConvertToKLineObject(karr[i]);
                kLineList.Add(model);

            }
            if (kLineList[kLineList.Count - 1].Sampledate != lastKLineModel.Sampledate)
            {
                kLineList.Add(lastKLineModel);
            }

            for (int i = 0; i < kLineList.Count; i++)
            {
                if (powerchange || oldkarr == null || i > oldkarr.Count - 1)
                {
                    for (int k = 0; k < powerList.Count; k++)
                    {
                        if (kLineList[i].Sampledate < powerList[k].sampledate)
                        {
                            kLineList[i].Dic["powero"] = Buz.PowerBuz.Fuquan(kLineList[i].Dic["powero"], powerList[k]);
                            kLineList[i].Dic["powerc"] = Buz.PowerBuz.Fuquan(kLineList[i].Dic["powerc"], powerList[k]);
                            kLineList[i].Dic["powerh"] = Buz.PowerBuz.Fuquan(kLineList[i].Dic["powerh"], powerList[k]);
                            kLineList[i].Dic["powerl"] = Buz.PowerBuz.Fuquan(kLineList[i].Dic["powerl"], powerList[k]);
                        }
                        kLineList[i].Dic["powermid"] = (kLineList[i].Dic["powerh"] + kLineList[i].Dic["powerl"]) / 2;
                    }
                    kLineList[i].Status = 1;

                }
                else if (kLineList[i].Sampledate == oldkarr[i].Sampledate)
                {
                    kLineList[i] = oldkarr[i];
                }
            }
            Buz.KLineBuz.Instance.BatchSaveData(kLineList, stockcode, powerchange);

            Buz.CrBuz.Instance.DataDeal(kLineList, powerchange, dr);
            Buz.PsyBuz.Instance.DataDeal(kLineList, powerchange, dr);
            Buz.CfblBuz.Instance.DataDeal(kLineList, powerchange, dr);
            Buz.TqCr.Instance.DataDeal(kLineList, powerchange, dr);
            Buz.HcwhBuz.Instance.DataDeal(kLineList, powerchange, dr);
            Buz.XlygBuz.Instance.DataDeal(kLineList, powerchange, dr);

            //周数据
            IList<Model.WeekStockModel> weekKLineList = Buz.WeekKLine.Instance.GetWeekKLineList(stockcode, kLineList, powerchange);

            //周线psy
            Buz.WeekPsy.Instance.DataDeal(weekKLineList, powerchange, dr);

            //周线降龙有轨
            Buz.WeekXlyg.Instance.DataDeal(weekKLineList, powerchange, dr);
            //月数据
            IList<Model.WeekStockModel> monthKLineList = Buz.MonthKLine.Instance.GetMonthKLineList(stockcode, kLineList, powerchange);
            //月线psy
            Buz.MonthPsy.Instance.DataDeal(monthKLineList, powerchange, dr);

            //月线降龙有轨
            Buz.MonthXlyg.Instance.DataDeal(monthKLineList, powerchange, dr);
        }

        static void Analyse()
        {
            //取出股票列表
            IList<Model.stockinfo> dtStock = Buz.StockBuz.GetList();
            //遍历
            for (int i = 0; i < dtStock.Count; i++)
            {
                AnalyseStock(dtStock[i]);
            }
        }

        

        static void AnalyseStock(Model.stockinfo dr)
        {
            //int begindate = 20110101;
            //int enddate = 20120416;
            string stockcode = dr.stockcode;
            IList<Model.StockModel> kLineList = Buz.KLineBuz.Instance.GetListFromDb(stockcode);
            //if (kLineList.Count > 0)
            //{
            //    enddate = kLineList[kLineList.Count - 1].Sampledate;
            //}
            for (int i = 1; i < kLineList.Count; i++)
            {
                int markeyType_before;
                int marketType = Buz.MarketSituation.Instance.GetMarketType(dr.stocktype, kLineList[i].Sampledate, out markeyType_before);
                switch (marketType)
                {
                    case 0:
                        //牛市
                        break;
                    case 1:
                        //鹿市
                        break;
                    case 2:
                        Buz.StockAnalyse.Instance.AnalyseBear(stockcode,markeyType_before, kLineList[i].Sampledate);
                        //熊市
                        break;
                    default:
                        break;
                }
                //if (kLineList[i].Sampledate > begindate)
                //{
                //    //echo $klinearr[$i]->stockcode;
                //    if (kLineList[i].Sampledate >= 20120107)
                //    {
                //        //Buz.StockAnalyse.Instance.AnalyseBear(stockcode,kLineList[i].Sampledate);
                //    }
                //    else
                //    {
                //        Buz.StockAnalyse.Instance.AnalyseBear(stockcode, kLineList[i].Sampledate);
                //    }

                //}
            }
        }
    }
}
