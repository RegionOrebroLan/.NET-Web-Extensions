using System;

namespace RegionOrebroLan.Web.Paging
{
	public interface IPage
	{
		#region Properties

		bool First { get; }
		int Index { get; }
		bool Last { get; }
		bool Selected { get; }
		Uri Url { get; }

		#endregion
	}
}