using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace StockDataDeal.Buz
{
    public class Formula
    {
        public static decimal Ma(int index, string name, int n, IList<Model.StockModel> list)
        {
            decimal ret = 0;
            if (index > n)
            {
                for (int i = 0; i < n; i++)
                {
                    ret += list[index - i].Dic[name];
                }
            }
            return ret/n;
        }

        public static decimal Sma(int index, decimal x, string name, decimal n, decimal m, IList<Model.StockModel> list)
        {
            if (index > 0)
            {
                decimal o = list[index-1].Dic[name];
                return (m * x + (n - m) * o) / n;
            }
            else
            {
                return x;
            }
            
        }

        public static decimal Ema(int index, decimal x, string xname, int m, IList<Model.StockModel> list)
        {
            if (index > 0)
            {
                decimal o = list[index-1].Dic[xname];
                return (2 * x + (m - 1) * o) / (m + 1);
            }
            else
            {
                return x;
            }
        }

        public static decimal RefMa(int index, string xname, int n, int m, IList<Model.StockModel> list)
        {
            decimal crm1 = 0;
            if (index >= n + m)
            {
                for (int j = 0; j < n; j++)
                {
                    crm1 += list[index - j - m].Dic[xname];
                }
                return crm1 / n;
            }
            else 
            {
                return 0;
            }
        }

        public static decimal Ref(int index, string name, int n, IList<Model.StockModel> list)
        {
            if (index >= n)
            {
                return list[index - n].Dic[name];
            }
            else
            {
                return 0;
            }
        }

        public static decimal LLV(int index, string name, int n, IList<Model.StockModel> list)
        {
            if (index >= n)
            {
                decimal low = list[index].Dic[name];
                for (int j = 0; j < n; j++)
                {
                    decimal temp = list[index - j].Dic[name];
                    if (low > temp)
                    {
                        low = temp;
                    }
                }
                return low;
            }
            else
            {
                return list[0].Dic[name];
            }
        }

        public static decimal HHV(int index, string name, int n, IList<Model.StockModel> list)
        {
            if (index >= n)
            {
                decimal high = list[index].Dic[name];
                for (int j = 0; j < n; j++)
                {
                    decimal temp = list[index - j].Dic[name];
                    if (high < temp)
                    {
                        high = temp;
                    }
                }
                return high;
            }
            else
            {
                return list[0].Dic[name];
            }
        }

        public static decimal STD(int index, string name, int n, IList<Model.StockModel> list)
        {
            decimal std = 0, avg = 0;
            if (index > 0)
            {
                for (int j = 0; j < n && j<index; j++)
                {
                    avg += list[index - j].Dic[name];
                }
                avg = avg / n;
            }
            else
            {
                avg = 0;
            }
            if (index > 0)
            {
                for (int j = 0; j < n && j<=index; j++)
                {
                    std += (decimal)Math.Pow((double)avg - (double)list[index - j].Dic[name], 2);
                }
            }
            else
            {
                std = 0;
            }
            std = (decimal)System.Math.Sqrt((double)(std/(n-1)));
            return std;
        }

        public static bool Cross(int index, string namea, string nameb, IList<Model.StockModel> list)
        {
            if (index > 0)
            {
                return list[index].Dic[namea] > list[index].Dic[nameb] &&
                       list[index - 1].Dic[namea] < list[index - 1].Dic[nameb];
            }
            else
            {
                return false;
            }
        }

        public static decimal Reverse()
        {
            return 0;
        }


        public static decimal Ma(int index, string name, int n, IList<Model.WeekStockModel> list)
        {
            decimal ret = 0;
            if (index > n)
            {
                for (int i = 0; i < n; i++)
                {
                    ret += list[index - i].Dic[name];
                }
            }
            return ret / n;
        }

        public static decimal Sma(int index, decimal x, string name, decimal n, decimal m, IList<Model.WeekStockModel> list)
        {
            if (index > 0)
            {
                decimal o = list[index - 1].Dic[name];
                return (m * x + (n - m) * o) / n;
            }
            else
            {
                return x;
            }

        }

        public static decimal Ema(int index, decimal x, string xname, int m, IList<Model.WeekStockModel> list)
        {
            if (index > 0)
            {
                decimal o = list[index - 1].Dic[xname];
                return (2 * x + (m - 1) * o) / (m + 1);
            }
            else
            {
                return x;
            }
        }

        public static decimal RefMa(int index, string xname, int n, int m, IList<Model.WeekStockModel> list)
        {
            decimal crm1 = 0;
            if (index >= n + m)
            {
                for (int j = 0; j < n; j++)
                {
                    crm1 += list[index - j - m].Dic[xname];
                }
                return crm1 / n;
            }
            else
            {
                return 0;
            }
        }

        public static decimal Ref(int index, string name, int n, IList<Model.WeekStockModel> list)
        {
            if (index >= n)
            {
                return list[index - n].Dic[name];
            }
            else
            {
                return 0;
            }
        }

        public static decimal LLV(int index, string name, int n, IList<Model.WeekStockModel> list)
        {
            if (index >= n)
            {
                decimal low = list[index].Dic[name];
                for (int j = 0; j < n; j++)
                {
                    decimal temp = list[index - j].Dic[name];
                    if (low > temp)
                    {
                        low = temp;
                    }
                }
                return low;
            }
            else
            {
                return list[0].Dic[name];
            }
        }

        public static decimal HHV(int index, string name, int n, IList<Model.WeekStockModel> list)
        {
            if (index >= n)
            {
                decimal high = list[index].Dic[name];
                for (int j = 0; j < n; j++)
                {
                    decimal temp = list[index - j].Dic[name];
                    if (high < temp)
                    {
                        high = temp;
                    }
                }
                return high;
            }
            else
            {
                return list[0].Dic[name];
            }
        }

        public static decimal STD(int index, string name, int n, IList<Model.WeekStockModel> list)
        {
            decimal std = 0, avg = 0;
            if (index > 0)
            {
                for (int j = 0; j < n && j < index; j++)
                {
                    avg += list[index - j].Dic[name];
                }
                avg = avg / n;
            }
            else
            {
                avg = 0;
            }
            if (index > 0)
            {
                for (int j = 0; j < n && j <= index; j++)
                {
                    std += (decimal)Math.Pow((double)avg - (double)list[index - j].Dic[name], 2);
                }
            }
            else
            {
                std = 0;
            }
            std = (decimal)System.Math.Sqrt((double)(std / (n - 1)));
            return std;
        }

        public static bool Cross(int index, string namea, string nameb, IList<Model.WeekStockModel> list)
        {
            if (index > 0)
            {
                return list[index].Dic[namea] > list[index].Dic[nameb] &&
                       list[index - 1].Dic[namea] < list[index - 1].Dic[nameb];
            }
            else
            {
                return false;
            }
        }
    }
}
