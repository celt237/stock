using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockDataDeal.Model
{
    public class StockModelBase
    {
        /// <summary>
        /// 状态 0 默认 1 要写入数据库 
        /// </summary>
        public int Status { get; set; }

        public Dictionary<string, decimal> Dic = new Dictionary<string, decimal>();

        public StockModelBase()
        {
            Status = 0;
        }
    }
}
