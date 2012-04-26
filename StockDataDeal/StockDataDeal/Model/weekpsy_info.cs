using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// weekpsy_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class weekpsy_info
	{
		public weekpsy_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _begindate;
		private int _enddate;
		private decimal _psy;
		private decimal _psyma;
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
		public decimal psy
		{
			set{ _psy=value;}
			get{return _psy;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal psyma
		{
			set{ _psyma=value;}
			get{return _psyma;}
		}
		#endregion Model

	}
}

