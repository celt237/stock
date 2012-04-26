using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// stockdeallog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class stockdeallog
	{
		public stockdeallog()
		{}
		#region Model
		private string _stockcode;
		private int _dealdate;
		private int _num;
		private int _opttype;
		private decimal _optprice;
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
		public int dealdate
		{
			set{ _dealdate=value;}
			get{return _dealdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int num
		{
			set{ _num=value;}
			get{return _num;}
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
		#endregion Model

	}
}

