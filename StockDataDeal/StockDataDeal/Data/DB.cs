using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace StockDataDeal.Data
{
    public class DB
    {
        private static Database _dbInstance = null;
        private static object o = new object();
        public static Database DbInstance
        {
            get 
            {
                if (_dbInstance == null)
                {
                    lock (o)
                    {
                        if (_dbInstance == null)
                        {
                            _dbInstance = DatabaseFactory.CreateDatabase("default");
                        }
                    }
                }
                return _dbInstance;
            }
        }
    }
}
