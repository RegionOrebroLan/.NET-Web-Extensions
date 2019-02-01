using System;
using System.Linq;

namespace RegionOrebroLan.Web.Paging.Extensions
{
	public static class PaginationExtension
	{
		#region Methods

		public static IPage SelectedPage(this IPagination pagination)
		{
			if(pagination == null)
				throw new ArgumentNullException(nameof(pagination));

			return (pagination.Pages ?? Enumerable.Empty<IPage>()).FirstOrDefault(page => page.Selected);
		}

		#endregion
	}
}