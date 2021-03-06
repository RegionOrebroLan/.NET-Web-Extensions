﻿using System;
using System.Collections.Generic;

namespace RegionOrebroLan.Web.Paging
{
	public interface IPagination
	{
		#region Properties

		Uri FirstPageUrl { get; }
		Uri LastPageUrl { get; }
		Uri NextPageGroupUrl { get; }
		Uri NextPageUrl { get; }
		IEnumerable<IPage> Pages { get; }
		Uri PreviousPageGroupUrl { get; }
		Uri PreviousPageUrl { get; }

		[Obsolete("This property will be removed. Use RegionOrebroLan.Web.Paging.Extensions.PaginationExtension.SelectedPage(this IPagination pagination) instead.", true)]
		IPage SelectedPage { get; }

		int Skip { get; }
		int Take { get; }
		int TotalNumberOfPages { get; }
		bool ZeroBasedIndex { get; }

		#endregion
	}
}