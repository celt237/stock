using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// cr_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class cr_info
	{
		public cr_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _sampledate;
		private decimal _cr;
		private decimal _ma1;
		private decimal _ma2;
		private decimal _ma3;
		private decimal _ma4;
		private decimal _mid;
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
		public decimal cr
		{
			set{ _cr=value;}
			get{return _cr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ma1
		{
			set{ _ma1=value;}
			get{return _ma1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ma2
		{
			set{ _ma2=value;}
			get{return _ma2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ma3
		{
			set{ _ma3=value;}
			get{return _ma3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ma4
		{
			set{ _ma4=value;}
			get{return _ma4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal mid
		{
			set{ _mid=value;}
			get{return _mid;}
		}
		#endregion Model

	}
}

