using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace StockDataDeal.Buz
{
    public class PowerBuz
    {
        private static string GetTableName(string stockCode)
        {
            return "power_info";
            //return "power_info_" + stockCode.Substring(stockCode.Length - 2);
        }

        public static int GetLastDate(string stockCode)
        {
            string sql = "select max(sampledate) as sampledate from " + GetTableName(stockCode) + " where stockcode=" + stockCode;
            int retVal = 0;
            int.TryParse(Data.DB.DbInstance.ExecuteScalar(CommandType.Text, sql).ToString(), out retVal);
            return retVal;
        }

        public static DataTable GetListFromDb(string stockCode)
        {
            string sql = "select * from " + GetTableName(stockCode) + " where stockcode=" + stockCode + " order by sampledate";
            return Data.DB.DbInstance.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        public static void SaveData(Model.power_info model)
        {
            string sql = "insert into " + GetTableName(model.stockcode) + " (stockcode,sampledate,song,pai,zhuan,pei,peiprice,updatetime) values ('"
                + model.stockcode + "',"
                + model.sampledate + ","
                + model.song + ","
                + model.pai + ","
                + model.zhuan + ","
                + model.pei + ","
                + model.peiprice + ",getdate())";
            Data.DB.DbInstance.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static string DealPowerStr(string value)
        {
            return value.Replace("PEAK_ODIA(","").Replace("\"", "").Replace("'", "").Replace("[dividend,", "").Replace("[", "").Replace("],", ";").Replace("]", "").Replace(")", "");
        }

        public static Model.power_info ConvertToPowerObject(string val,string stockcode){
            string[] ret = val.Split(',');
            Model.power_info model = new Model.power_info();
            model.stockcode = stockcode;
            model.sampledate = int.Parse(ret[0]);//日期
            model.song = decimal.Parse(ret[1]);//
            model.pai = decimal.Parse(ret[2]);//
            model.zhuan = decimal.Parse(ret[3]);//
            model.pei = decimal.Parse(ret[4]);//
            model.peiprice = decimal.Parse(ret[5]);//
            return model;
        }

        public static decimal Fuquan(decimal value, Model.power_info powerObj)
        {
            return (value - powerObj.pai / 10 + powerObj.peiprice * (powerObj.pei / 10)) / (1 + (powerObj.song + powerObj.zhuan) / 10 + powerObj.pei / 10);
        }
    }
}
