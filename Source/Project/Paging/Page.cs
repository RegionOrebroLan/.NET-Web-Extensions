using System;

namespace RegionOrebroLan.Web.Paging
{
	public class Page : IPage
	{
		#region Properties

		public virtual bool First { get; set; }
		public virtual int Index { get; set; }
		public virtual bool Last { get; set; }
		public virtual bool Selected { get; set; }
		public virtual Uri Url { get; set; }

		#endregion
	}
}