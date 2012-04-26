using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Text.RegularExpressions;
namespace StockDataDeal.Buz
{
    public class WeekKLine : BuzWeekBase
    {
        public static WeekKLine Instance = new WeekKLine();
        public override string GetTableName(string stockCode)
        {
            return "weekkline_info";
            //return "weekkline_info_" + stockCode.Substring(stockCode.Length - 2);
        }

        

        public IList<Model.WeekStockModel> GetWeekKLineList(string stockcode, IList<Model.StockModel> kLineList, bool powerchange)
        {
            IList<Model.WeekStockModel> oldweekkarr = new List<Model.WeekStockModel>();
            IList<Model.WeekStockModel> weekkarr = GetWeekKLineInfo(stockcode);//从数据抓取获取
            if (!powerchange)
            {
                oldweekkarr = GetListFromDb(stockcode);
                if (oldweekkarr != null && oldweekkarr.Count > 0)
                {
                    for (int i = 0; i < oldweekkarr.Count; i++)
                    {
                        if (weekkarr[i].EndDate == oldweekkarr[i].EndDate)
                        {
                            weekkarr[i] = oldweekkarr[i];
                        }
                    }
                }
            }
            IList<Model.WeekStockModel> eList = weekkarr.Where<Model.WeekStockModel>(m => m.Status == 1).ToList<Model.WeekStockModel>();
            foreach (Model.WeekStockModel model in eList)
            {
                IList<Model.StockModel> ke = kLineList.Where<Model.StockModel>(m => m.Sampledate >= model.BeginDate && m.Sampledate <= model.EndDate).ToList<Model.StockModel>();
                if (ke != null && ke.Count > 0)
                {
                    model.Dic["powero"] = ke[0].Dic["powero"];
                    model.Dic["powerh"] = ke[0].Dic["powerh"];
                    model.Dic["powerl"] = ke[0].Dic["powerl"];
                    model.Dic["powerc"] = ke[ke.Count - 1].Dic["powerc"];
                }
                foreach(Model.StockModel kModel in ke)
                {
                    model.Dic["powerh"] = Math.Max(model.Dic["powerh"], kModel.Dic["powerh"]);
                    model.Dic["powerl"] = Math.Min(model.Dic["powerl"], kModel.Dic["powerl"]);
                }
            }
            
            BatchSaveData(weekkarr, stockcode, powerchange);
            return weekkarr;
        }

        private IList<Model.WeekStockModel> GetWeekKLineInfo(string stockcode)
        {
            //获取周期数据
            string url = string.Format("http://hq.stock.sohu.com/cn/{0}/cn_{1}-7.html?uid={2}", stockcode.Substring(stockcode.Length - 3), stockcode, new Random().Next(1000000, 99999999));
            string content = Common.GetGeneralContent(url);
            List<string> karr = new List<string>();
            List<Model.WeekStockModel> kLineList = new List<Model.WeekStockModel>();
            var collection = Regex.Matches(content, @"PEAK_ODIA\(([^\)]*)\)");
            for (int i = 0; i < collection.Count; i++)
            {
                string value = collection[i].Value;

                if (value.Contains("quote_wk_all") && !value.Contains("init") && !value.Contains("end"))
                {
                    //k线信息
                    //$karr = array_merge($karr, explode(';', KLineHelper::dealKLineStr($arr[1][$i])));
                    value = value.Replace("PEAK_ODIA(", "").Replace("\"", "").Replace("'", "").Replace("[quote_wk_all,", "").Replace("[", "").Replace("],", ";").Replace("]", "").Replace(")", "");
                    karr.AddRange(value.Split(';').ToList<string>());
                }
            }
            karr.Reverse();
            for (int i = 0; i < karr.Count; i++)
            {
                Model.WeekStockModel model = ConvertToWeekKLineObject(karr[i]);
                kLineList.Add(model);
            }
            return kLineList;
        }

        public Model.WeekStockModel ConvertToWeekKLineObject(string val)
        {
            string[] ret = val.Split(',');
            Model.WeekStockModel model = null;
            if (ret.Length == 11)
            {
                model = new Model.WeekStockModel();
                model.EndDate = int.Parse(ret[0]);//结束日期
                model.BeginDate = int.Parse(ret[10]);//开始日期
                model.Dic["powero"] = 0;
                model.Dic["powerc"] = 0;
                model.Dic["powerh"] = 0;
                model.Dic["powerl"] = 0;
                model.Status = 1;
            }
            return model;
        }


    }
}
