using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// weekkline_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class weekkline_info
	{
		public weekkline_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _begindate;
		private int _enddate;
		private decimal _powero;
		private decimal _powerc;
		private decimal _powerh;
		private decimal _powerl;
		/// <summary>
		/// 
		/// </summary>
		public long id
		{
			set{ _id=value;}
			get{return _id;}
		}
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
		public int begindate
		{
			set{ _begindate=value;}
			get{return _begindate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int enddate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal powero
		{
			set{ _powero=value;}
			get{return _powero;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal powerc
		{
			set{ _powerc=value;}
			get{return _powerc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal powerh
		{
			set{ _powerh=value;}
			get{return _powerh;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal powerl
		{
			set{ _powerl=value;}
			get{return _powerl;}
		}
		#endregion Model

	}
}

