using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// power_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class power_info
	{
		public power_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _sampledate;
		private decimal _song;
		private decimal _pai;
		private decimal _zhuan;
		private decimal _pei;
		private decimal _peiprice;
		private DateTime _updatetime= DateTime.Now;
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
		public decimal song
		{
			set{ _song=value;}
			get{return _song;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal pai
		{
			set{ _pai=value;}
			get{return _pai;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal zhuan
		{
			set{ _zhuan=value;}
			get{return _zhuan;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal pei
		{
			set{ _pei=value;}
			get{return _pei;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal peiprice
		{
			set{ _peiprice=value;}
			get{return _peiprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime updatetime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		#endregion Model

	}
}

