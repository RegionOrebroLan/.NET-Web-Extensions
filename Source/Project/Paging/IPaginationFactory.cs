using System;

namespace RegionOrebroLan.Web.Paging
{
	public interface IPaginationFactory
	{
		#region Methods

		IPagination Create(int maximumNumberOfDisplayedPages, int numberOfItems, string pageIndexKey, int pageSize, Uri url);
		IPagination Create(int maximumNumberOfDisplayedPages, int numberOfItems, string pageIndexKey, int pageSize, Uri url, bool zeroBasedIndex);

		#endregion
	}
}