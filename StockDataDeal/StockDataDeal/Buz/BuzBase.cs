using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;

namespace StockDataDeal.Buz
{
    public class BuzBase
    {
        public virtual string GetTableName(string stockCode)
        {
            return "";
        }

        public int GetLastDate(string stockCode)
        {
            string sql = "select max(sampledate) as sampledate from " + GetTableName(stockCode) + " where stockcode='" + stockCode+"'";
            int retVal = 0;
            int.TryParse(Data.DB.DbInstance.ExecuteScalar(CommandType.Text, sql).ToString(), out retVal);
            return retVal;
        }

        public IList<Model.StockModel> GetListFromDb(string stockCode)
        {
            string sql = "select * from " + GetTableName(stockCode) + " where stockcode='" + stockCode + "' order by sampledate";
            return DtToList(Data.DB.DbInstance.ExecuteDataSet(CommandType.Text, sql).Tables[0]);
        }

        public IList<Model.StockModel> DtToList(DataTable dt)
        {
            IList<Model.StockModel> list = new List<Model.StockModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.StockModel model = new Model.StockModel();
                foreach (DataColumn column in dt.Columns)
                {
                    string columnName = column.ColumnName.ToLower();
                    if (columnName == "id" || columnName == "stockcode")
                    {
                        continue;
                    }
                    else if (columnName == "sampledate")
                    {
                        model.Sampledate = int.Parse(dt.Rows[i]["sampledate"].ToString());
                    }
                    else
                    {
                        try
                        {
                            decimal temp = decimal.Parse(dt.Rows[i][columnName].ToString());
                            model.Dic.Add(columnName, temp);
                        }
                        catch
                        {
                            model.Dic.Add(columnName, 0);
                        }

                    }
                }

                list.Add(model);
            }
            return list;
        }

        public void SaveData(Model.StockModel model, string stockCode, bool isChange)
        {
            string sql;
            if (isChange)
            {
                sql = "delete from " + GetTableName(stockCode) + " where stockcode='" + stockCode + "' and sampledate=" + model.Sampledate;
                Data.DB.DbInstance.ExecuteNonQuery(CommandType.Text, sql);
            }
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            columns.Append("stockcode,sampledate,");
            values.AppendFormat("'{0}','{1}',",stockCode,model.Sampledate);
            foreach (string key in model.Dic.Keys)
            {
                columns.AppendFormat("{0},", key);
                values.AppendFormat("'{0}',", model.Dic[key]);
            }
            sql = string.Format("insert into {0} ({1}) values ({2})", GetTableName(stockCode), columns.ToString().Trim(','), values.ToString().Trim(','));

            Data.DB.DbInstance.ExecuteNonQuery(CommandType.Text, sql);
        }
        public void BatchSaveData(IList<Model.StockModel> list, string stockCode, bool isChange)
        {
            string sql;
            if (isChange)
            {
                sql = "delete from " + GetTableName(stockCode) + " where stockcode='" + stockCode + "'";
                Data.DB.DbInstance.ExecuteNonQuery(CommandType.Text, sql);
            }
            
            if (list.Count > 0)
            {
                string connectionString = Data.DB.DbInstance.ConnectionString;

                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString);
                sqlBulkCopy.DestinationTableName = GetTableName(stockCode);
                sqlBulkCopy.BatchSize = list.Count;
                SqlBulkCopyColumnMappingCollection collection = sqlBulkCopy.ColumnMappings;
                
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("stockcode");
                collection.Add("stockcode", "stockcode");
                dataTable.Columns.Add("sampledate");
                collection.Add("sampledate", "sampledate");
                foreach (string key in list[0].Dic.Keys)
                {
                    dataTable.Columns.Add(key);
                    collection.Add(key, key);
                    
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Status == 1)
                    {
                        DataRow newRow = dataTable.NewRow();
                        newRow["stockcode"] = stockCode;
                        newRow["sampledate"] = list[i].Sampledate;
                        foreach (string key in list[i].Dic.Keys)
                        {
                            newRow[key] = list[i].Dic[key];
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }
                if (dataTable != null && dataTable.Rows.Count != 0)
                {
                    sqlBulkCopy.WriteToServer(dataTable);
                }
                sqlBulkCopy.Close();
                sqlConnection.Close();
            }
            
        }
       
    }
}
