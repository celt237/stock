using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// stockanalyse_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class stockanalyse_info
	{
		public stockanalyse_info()
		{}
		#region Model
		private string _stockcode;
		private int _status;
		private int _opttype;
		private decimal _optprice;
		private int? _a=0;
		private int _b=0;
		private int _adays=0;
		private int _bdays=0;
		private int _currentdate;
		/// <summary>
		/// 
		/// </summary>
		public string stockcode
		{
			set{ _stockcode=value;}
			get{return _stockcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int opttype
		{
			set{ _opttype=value;}
			get{return _opttype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal optprice
		{
			set{ _optprice=value;}
			get{return _optprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? a
		{
			set{ _a=value;}
			get{return _a;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int b
		{
			set{ _b=value;}
			get{return _b;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int adays
		{
			set{ _adays=value;}
			get{return _adays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int bdays
		{
			set{ _bdays=value;}
			get{return _bdays;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int currentdate
		{
			set{ _currentdate=value;}
			get{return _currentdate;}
		}
		#endregion Model

	}
}

