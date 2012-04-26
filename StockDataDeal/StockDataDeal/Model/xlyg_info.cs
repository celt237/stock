using System;
namespace StockDataDeal.Model
{
	/// <summary>
	/// xlyg_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class xlyg_info
	{
		public xlyg_info()
		{}
		#region Model
		private long _id;
		private string _stockcode;
		private int _sampledate;
		private decimal _xlyg2;
		private decimal _xlyg3;
		private decimal _xlyg5;
		private decimal _xlyg8;
		private decimal _xlyg13;
		private decimal _xlyg21;
		private decimal _xlygup;
		private decimal _xlygdown;
		private decimal _xlygupgui;
		private decimal _xlygdowngui;
		private decimal _xlygstock;
		private decimal _xlygmoney;
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
		public decimal xlyg2
		{
			set{ _xlyg2=value;}
			get{return _xlyg2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlyg3
		{
			set{ _xlyg3=value;}
			get{return _xlyg3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlyg5
		{
			set{ _xlyg5=value;}
			get{return _xlyg5;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlyg8
		{
			set{ _xlyg8=value;}
			get{return _xlyg8;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlyg13
		{
			set{ _xlyg13=value;}
			get{return _xlyg13;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlyg21
		{
			set{ _xlyg21=value;}
			get{return _xlyg21;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygup
		{
			set{ _xlygup=value;}
			get{return _xlygup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygdown
		{
			set{ _xlygdown=value;}
			get{return _xlygdown;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygupgui
		{
			set{ _xlygupgui=value;}
			get{return _xlygupgui;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygdowngui
		{
			set{ _xlygdowngui=value;}
			get{return _xlygdowngui;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygstock
		{
			set{ _xlygstock=value;}
			get{return _xlygstock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal xlygmoney
		{
			set{ _xlygmoney=value;}
			get{return _xlygmoney;}
		}
		#endregion Model

	}
}

