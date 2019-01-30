using System;

namespace RegionOrebroLan.Web.Paging
{
	public interface IPaginationValidator
	{
		#region Methods

		void ValidateMaximumNumberOfDisplayedPages(int maximumNumberOfDisplayedPages);
		void ValidateNumberOfItems(int numberOfItems);
		void ValidatePageIndexKey(string pageIndexKey);
		void ValidatePageSize(int pageSize);
		void ValidateUrl(Uri url);

		#endregion
	}
}