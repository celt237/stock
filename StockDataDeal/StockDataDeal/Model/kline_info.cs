using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// kline_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class kline_info
	{
		public kline_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _sampledate;
		private decimal _c;
		private decimal _h;
		private decimal _l;
		private decimal _o;
		private decimal _mid;
		private decimal _cjl;
		private decimal _jye;
		private decimal _hsl;
		private decimal _je;
		private decimal _fd;
		private decimal _powerc;
		private decimal _powerh;
		private decimal _powerl;
		private decimal _powero;
		private decimal _powermid;
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
		public int sampledate
		{
			set{ _sampledate=value;}
			get{return _sampledate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal c
		{
			set{ _c=value;}
			get{return _c;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal h
		{
			set{ _h=value;}
			get{return _h;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal l
		{
			set{ _l=value;}
			get{return _l;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal o
		{
			set{ _o=value;}
			get{return _o;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal mid
		{
			set{ _mid=value;}
			get{return _mid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal cjl
		{
			set{ _cjl=value;}
			get{return _cjl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal jye
		{
			set{ _jye=value;}
			get{return _jye;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal hsl
		{
			set{ _hsl=value;}
			get{return _hsl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal je
		{
			set{ _je=value;}
			get{return _je;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal fd
		{
			set{ _fd=value;}
			get{return _fd;}
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
		public decimal powermid
		{
			set{ _powermid=value;}
			get{return _powermid;}
		}
		#endregion Model

	}
}

