using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// stockinfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class stockinfo
	{
		public stockinfo()
		{}
		#region Model
		private string _stockcode;
		private string _stockname;
		private string _industry;
		private decimal _tradableshare;
		private decimal _fullshare;
		private string _pyforshort;
		private int _risktype;
		private int _stocktype;
		private int _begindate;
		private int _isnewstock;
		private int _status=1;
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
		public string stockname
		{
			set{ _stockname=value;}
			get{return _stockname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string industry
		{
			set{ _industry=value;}
			get{return _industry;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal tradableshare
		{
			set{ _tradableshare=value;}
			get{return _tradableshare;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal fullshare
		{
			set{ _fullshare=value;}
			get{return _fullshare;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pyforshort
		{
			set{ _pyforshort=value;}
			get{return _pyforshort;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int risktype
		{
			set{ _risktype=value;}
			get{return _risktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int stocktype
		{
			set{ _stocktype=value;}
			get{return _stocktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int begindate
		{
			set{ _begindate=value;}
			get{return _begindate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int isnewstock
		{
			set{ _isnewstock=value;}
			get{return _isnewstock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

