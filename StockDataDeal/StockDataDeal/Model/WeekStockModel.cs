using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockDataDeal.Model
{
    public class WeekStockModel:StockModelBase
    {
        public int BeginDate { get; set; }
        public int EndDate { get; set; }
    }
}
