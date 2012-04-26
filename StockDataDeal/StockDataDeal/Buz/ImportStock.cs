using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace StockDataDeal.Buz
{
    public class ImportStock
    {
        ///<summary>
        /// Get Data From Csv File 
        /// (Through StreamReader)
        ///</summary>
        ///<returns></returns>
        public static bool GetData(Stream inputStream, out string errMessage, out DataTable dtFile)
        {
            errMessage = String.Empty;
            dtFile = new DataTable();


            //this.FileUploadImport.PostedFile.InputStream.CanSeek;
            //StreamReader sr = new StreamReader(this.FileUploadImport.PostedFile.InputStream); //update by hwx 11/12/2010
            StreamReader sr = new StreamReader(inputStream);
            int iColumnCount = 19;//the column count in the file
            int iRow = 1;// the row number is being read
            int iColumn = 0;// the column number is being read
            string strStandardTitle = string.Empty; // the title as it is

            //read title row
            string strTitle = sr.ReadLine();

            //string[] strRowTitle = strTitle.Split(new char[] { ',' });
            string[] strRowTitle = new string[iColumnCount];
            string strCharTitle;
            int iCellNumberTitle = 0;
            bool blnQuoteTitle = false;
            for (int i = 0; i < strTitle.Length; i++)
            {
                strCharTitle = strTitle.Substring(i, 1);
                if (strCharTitle == ",")// "," is the seperation symbol of csv file,
                {
                    if (!blnQuoteTitle)
                    {
                        iCellNumberTitle++;// out of the "" range, "," is the seperation symbol
                        if (iCellNumberTitle >= iColumnCount)// too many column in this line
                        {
                            break;
                        }
                    }
                    else
                    {
                        strRowTitle[iCellNumberTitle] += strCharTitle;
                    }
                }
                else if (strCharTitle == "\"")// "\"" is the transfer symbol of csv file,
                {
                    blnQuoteTitle = !blnQuoteTitle;
                    if (blnQuoteTitle && i > 0 && strTitle.Substring(i - 1, 1) == "\"")//in the "" range and there is an transfer symbol before
                    {
                        strRowTitle[iCellNumberTitle] += strCharTitle;
                    }
                }
                else
                {
                    strRowTitle[iCellNumberTitle] += strCharTitle;
                }
            }
            //read the content
            if (strRowTitle.Length == iColumnCount)
            {
                foreach (string strCell in strRowTitle)
                {
                    iColumn++;
                    if (strCell.Trim() != string.Empty)
                    {
                        dtFile.Columns.Add(strCell);//add new column with name to the data table

                    }
                    else //file error:blank title
                    {

                        errMessage += "The cell " + iColumn.ToString() + " is blank in the header row.\r\n";
                    }
                }
                if (dtFile.Columns.Count == iColumnCount)//make sure that no blank header or error header
                {
                    //read content row
                    string strLine;
                    while (!sr.EndOfStream)
                    {
                        iRow++;
                        iColumn = 0;
                        DataRow dr = dtFile.NewRow();
                        strLine = sr.ReadLine();

                        //read csv file line by line
                        string[] strRow = new string[iColumnCount];
                        string strChar;
                        int iCellNumber = 0;
                        bool blnQuote = false;//whether in the "" range
                        for (int i = 0; i < strLine.Length; i++)
                        {
                            strChar = strLine.Substring(i, 1);
                            if (strChar == ",")// "," is the seperation symbol of csv file,
                            {
                                if (!blnQuote)
                                {
                                    iCellNumber++;// out of the "" range, "," is the seperation symbol
                                    if (iCellNumber >= iColumnCount)// too many column in this line
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    strRow[iCellNumber] += strChar;
                                }
                            }
                            else if (strChar == "\"")// "\"" is the transfer symbol of csv file,
                            {
                                blnQuote = !blnQuote;
                                if (blnQuote && i > 0 && strLine.Substring(i - 1, 1) == "\"")//in the "" range and there is an transfer symbol before
                                {
                                    strRow[iCellNumber] += strChar;
                                }
                            }
                            else
                            {
                                strRow[iCellNumber] += strChar;
                            }
                        }
                        if (iCellNumber + 1 == iColumnCount)
                        {
                            foreach (string strCell in strRow)
                            {
                                iColumn++;
                                if (strCell != null && strCell.Trim() != string.Empty)
                                {
                                    dr[strRowTitle[iColumn - 1]] = strCell.Trim();
                                }
                                else//file error:blank cell
                                {
                                    dr[strRowTitle[iColumn - 1]] = String.Empty;
                                    //errMessage += "The column \"" + strRowTitle[iColumn - 1] + "\" is blank in row " + iRow.ToString() + ".\r\n";
                                }
                            }
                        }
                        else// file error:the column count of current row do not equal to title's
                        {
                            errMessage += "There are more or less cells than title row in the row " + iRow.ToString() + ".\r\n";
                        }
                        dtFile.Rows.Add(dr);
                    }
                }
            }
            else //file error:the count of columns in the file don't equal it should be
            {
                errMessage += "There are an incorrect number of columns in the header row compared to the template file.\r\n";
            }
            sr.Close();
            sr.Dispose();

            errMessage = errMessage.Replace("\r\n", "<br>");
            return errMessage == String.Empty ? true : false;
        }


        ///<summary>
        /// Get dataset from csv file.
        ///</summary>
        ///<param name="filepath"></param>
        ///<param name="filename"></param>
        ///<returns>Data Set</returns>
        public static DataSet GetDatasetFromCsv(string filepath, string filename)
        {
            string strconn = @"driver={microsoft text driver (*.txt; *.csv)};dbq=";
            strconn += filepath;                                                        //filepath, for example: c:\
            strconn += ";extensions=asc,csv,tab,txt;";
            OdbcConnection objconn = new OdbcConnection(strconn);
            DataSet dscsv = new DataSet();
            try
            {
                string strsql = "select * from " + filename;                     //filename, for example: 1.csv
                OdbcDataAdapter odbccsvdataadapter = new OdbcDataAdapter(strsql, objconn);

                odbccsvdataadapter.Fill(dscsv);
                return dscsv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Import()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            DataSet ds = Buz.ImportStock.GetDatasetFromCsv(@"D:\", @"stock2.csv");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        switch (j)
                        {
                            case 1:
                                //股票代码
                                dic.Add("stockcode", Convert.ToInt32(dr[j].ToString()).ToString("000000"));
                                break;
                            case 2:
                                //名称
                                dic.Add("stockname", dr[j].ToString());
                                break;
                            case 3:
                                //缩写
                                dic.Add("pyforshort", dr[j].ToString());
                                break;
                            case 4:
                                //所属行业
                                dic.Add("industry", dr[j].ToString());
                                break;
                            case 5:
                                //庄股属性
                                dic.Add("stocktype", GetStockType(dr[j].ToString()));
                                break;
                            case 6:
                                //风险属性
                                dic.Add("risktype", GetRiskType(dr[j].ToString()));
                                break;
                            case 7:
                                //是否新股
                                dic.Add("isnewstock", GetNewStockType(dr[j].ToString()));
                                break;
                            case 8:
                                //流通股本
                                dic.Add("tradableshare", dr[j].ToString());
                                break;
                            case 9:
                                //总股本(万)
                                dic.Add("fullshare", dr[j].ToString());
                                break;
                            case 13:
                                //上市日期
                                dic.Add("begindate", dr[j].ToString());
                                break;
                            default:
                                break;
                        }
                        
                    }
                    dic.Add("status", 0);
                    list.Add(dic);
                }
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("default");
                for (int i = 0; i < list.Count; i++)
                {
                    
                    string columns = string.Empty,values=string.Empty;
                    foreach (string key in list[i].Keys)
                    {
                        columns += key + ",";
                        values += "'" + list[i][key] + "',";
                    }
                    string sql = string.Format("insert into stockinfo ({0}) values ({1})", columns.Trim(','), values.Trim(','));
                    db.ExecuteNonQuery(CommandType.Text,sql);
                    //Console.WriteLine(sql);
                    
                }
            }
        }

        private void test()
        {
            
            //db.ExecuteNonQuery();
        }

        private static int GetStockType(string str)
        {
            switch (str.Trim())
            {
                case "大盘股":
                    return 1;
                case "创业板":
                    return 2;
                default:
                    return 0;
            }
        }

        private static int GetRiskType(string str)
        {
            switch (str.Trim())
            {
                case "ST股":
                    return 1;
                case "ＳＴ股":
                    return 1;
                default:
                    return 0;
            }
        }

        private static int GetNewStockType(string str)
        {
            switch (str.Trim())
            {
                case "新股":
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
