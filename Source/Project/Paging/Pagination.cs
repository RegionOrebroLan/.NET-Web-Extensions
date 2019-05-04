using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web;
using RegionOrebroLan.Web.Paging.Extensions;

namespace RegionOrebroLan.Web.Paging
{
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
	public class Pagination : IPagination
	{
		#region Fields

		private int? _firstPageIndex;
		private Lazy<Uri> _firstPageUrl;
		private int? _lastPageIndex;
		private Lazy<Uri> _lastPageUrl;
		private Lazy<Uri> _nextPageGroupUrl;
		private Lazy<Uri> _nextPageUrl;
		private int? _pageIndex;
		private IEnumerable<IPage> _pages;
		private Lazy<Uri> _previousPageGroupUrl;
		private Lazy<Uri> _previousPageUrl;
		private int? _skip;
		private int? _take;
		private int? _totalNumberOfPages;

		#endregion

		#region Constructors

		public Pagination(int maximumNumberOfDisplayedPages, int numberOfItems, string pageIndexKey, int pageSize, Uri url, IPaginationValidator validator, bool zeroBasedIndex)
		{
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));

			validator.ValidateMaximumNumberOfDisplayedPages(maximumNumberOfDisplayedPages);
			validator.ValidateNumberOfItems(numberOfItems);
			validator.ValidatePageIndexKey(pageIndexKey);
			validator.ValidatePageSize(pageSize);
			validator.ValidateUrl(url);

			this.MaximumNumberOfDisplayedPages = maximumNumberOfDisplayedPages;
			this.NumberOfItems = numberOfItems;
			this.PageIndexKey = pageIndexKey;
			this.PageSize = pageSize;
			this.Url = url;
			this.ZeroBasedIndex = zeroBasedIndex;
		}

		#endregion

		#region Properties

		protected internal virtual int FirstPageIndex
		{
			get
			{
				if(this._firstPageIndex == null)
					this._firstPageIndex = this.TotalNumberOfPages > 0 ? 0 : -1;

				return this._firstPageIndex.Value;
			}
		}

		public virtual Uri FirstPageUrl
		{
			get
			{
				if(this._firstPageUrl == null)
				{
					this._firstPageUrl = new Lazy<Uri>(() =>
					{
						if(this.TotalNumberOfPages <= 0 || this.PageIndex == this.FirstPageIndex)
							return null;

						return this.CreateUrl(this.FirstPageIndex);
					});
				}

				return this._firstPageUrl.Value;
			}
		}

		protected internal virtual int LastPageIndex
		{
			get
			{
				if(this._lastPageIndex == null)
					this._lastPageIndex = this.TotalNumberOfPages > 0 ? this.TotalNumberOfPages - 1 : -2;

				return this._lastPageIndex.Value;
			}
		}

		public virtual Uri LastPageUrl
		{
			get
			{
				if(this._lastPageUrl == null)
				{
					this._lastPageUrl = new Lazy<Uri>(() =>
					{
						if(this.TotalNumberOfPages <= 0 || this.PageIndex == this.LastPageIndex)
							return null;

						return this.CreateUrl(this.LastPageIndex);
					});
				}

				return this._lastPageUrl.Value;
			}
		}

		protected internal virtual int MaximumNumberOfDisplayedPages { get; }

		public virtual Uri NextPageGroupUrl
		{
			get
			{
				if(this._nextPageGroupUrl == null)
				{
					this._nextPageGroupUrl = new Lazy<Uri>(() =>
					{
						// ReSharper disable InvertIf
						if(this.PageIndex < this.LastPageIndex)
						{
							var quotient = this.PageIndex / this.MaximumNumberOfDisplayedPages;

							var firstIndexInNextPageGroup = this.AddNumbersSafely(this.MultiplyNumbersSafely(quotient, this.MaximumNumberOfDisplayedPages), this.MaximumNumberOfDisplayedPages);

							if(firstIndexInNextPageGroup < this.LastPageIndex - 1)
								return this.CreateUrl(firstIndexInNextPageGroup);
						}
						// ReSharper restore InvertIf

						return null;
					});
				}

				return this._nextPageGroupUrl.Value;
			}
		}

		public virtual Uri NextPageUrl
		{
			get
			{
				if(this._nextPageUrl == null)
				{
					this._nextPageUrl = new Lazy<Uri>(() =>
					{
						if(this.TotalNumberOfPages <= 0 || this.PageIndex == this.LastPageIndex)
							return null;

						return this.CreateUrl(this.PageIndex + 1);
					});
				}

				return this._nextPageUrl.Value;
			}
		}

		protected internal virtual int NumberOfItems { get; }
		protected internal virtual int PageGroupIndex => this.PageIndex / this.MaximumNumberOfDisplayedPages;

		protected internal virtual int PageIndex
		{
			get
			{
				if(this._pageIndex == null)
					this._pageIndex = this.PageIndexInternal;

				return this._pageIndex.Value;
			}
		}

		protected internal virtual int PageIndexInternal
		{
			get
			{
				var query = this.ParseQueryString(this.Url);

				var pageIndexValue = query[this.PageIndexKey];

				// ReSharper disable All
				if(!string.IsNullOrWhiteSpace(pageIndexValue) && int.TryParse(pageIndexValue, out var pageIndex))
				{
					if(!this.ZeroBasedIndex)
						pageIndex--;

					if(pageIndex > this.FirstPageIndex)
					{
						if(pageIndex > this.LastPageIndex)
							return this.LastPageIndex;

						return pageIndex;
					}
				}
				// ReSharper restore All

				return this.FirstPageIndex;
			}
		}

		protected internal virtual string PageIndexKey { get; }

		public virtual IEnumerable<IPage> Pages
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._pages == null)
				{
					if(this.TotalNumberOfPages > 0)
					{
						var firstPageIndex = this.FirstPageIndex;
						var lastPageIndex = this.LastPageIndex;

						if(this.MaximumNumberOfDisplayedPages < this.TotalNumberOfPages)
						{
							firstPageIndex += this.PageGroupIndex * this.MaximumNumberOfDisplayedPages;

							var potentialLastPageIndex = firstPageIndex + this.MaximumNumberOfDisplayedPages - 1;

							if(potentialLastPageIndex < lastPageIndex)
								lastPageIndex = potentialLastPageIndex;
						}

						var pages = new List<IPage>();

						for(var i = firstPageIndex; i <= lastPageIndex; i++)
						{
							var page = new Page
							{
								First = i == firstPageIndex,
								Index = i + (this.ZeroBasedIndex ? 0 : 1),
								Last = i == lastPageIndex,
								Selected = i == this.PageIndex,
								Url = this.CreateUrl(i)
							};

							pages.Add(page);
						}

						this._pages = pages.ToArray();
					}
					else
					{
						this._pages = Array.Empty<IPage>();
					}
				}
				// ReSharper restore InvertIf

				return this._pages;
			}
		}

		protected internal virtual int PageSize { get; }

		public virtual Uri PreviousPageGroupUrl
		{
			get
			{
				if(this._previousPageGroupUrl == null)
				{
					this._previousPageGroupUrl = new Lazy<Uri>(() =>
					{
						// ReSharper disable InvertIf
						if(this.PageIndex > 0)
						{
							var quotient = this.PageIndex / this.MaximumNumberOfDisplayedPages;

							if(quotient > 0)
							{
								var lastIndexInPreviousPageGroup = this.MultiplyNumbersSafely(quotient, this.MaximumNumberOfDisplayedPages) - 1;

								return this.CreateUrl(lastIndexInPreviousPageGroup);
							}
						}
						// ReSharper restore InvertIf

						return null;
					});
				}

				return this._previousPageGroupUrl.Value;
			}
		}

		public virtual Uri PreviousPageUrl
		{
			get
			{
				if(this._previousPageUrl == null)
				{
					this._previousPageUrl = new Lazy<Uri>(() =>
					{
						if(this.TotalNumberOfPages <= 0 || this.PageIndex == this.FirstPageIndex)
							return null;

						return this.CreateUrl(this.PageIndex - 1);
					});
				}

				return this._previousPageUrl.Value;
			}
		}

		[Obsolete("This property will be removed. Use RegionOrebroLan.Web.Paging.Extensions.PaginationExtension.SelectedPage(this IPagination pagination) instead.", true)]
		public virtual IPage SelectedPage => this.SelectedPage();

		public virtual int Skip
		{
			get
			{
				if(this._skip == null)
					this._skip = this.TotalNumberOfPages <= 0 ? 0 : this.MultiplyNumbersSafely(this.PageIndex, this.PageSize);

				return this._skip.Value;
			}
		}

		public virtual int Take
		{
			get
			{
				if(this._take == null)
					this._take = this.TotalNumberOfPages <= 0 ? int.MaxValue : this.PageSize;

				return this._take.Value;
			}
		}

		public virtual int TotalNumberOfPages
		{
			get
			{
				if(this._totalNumberOfPages == null)
					this._totalNumberOfPages = this.TotalNumberOfPagesInternal;

				return this._totalNumberOfPages.Value;
			}
		}

		protected internal virtual int TotalNumberOfPagesInternal
		{
			get
			{
				var totalNumberOfPages = 0;

				// ReSharper disable All
				if(this.NumberOfItems > 1 && this.PageSize > 0)
				{
					totalNumberOfPages = 1 + ((this.NumberOfItems - 1) / this.PageSize);

					if(totalNumberOfPages < 2)
						totalNumberOfPages = 0;
				}
				// ReSharper restore All

				return totalNumberOfPages;
			}
		}

		protected internal virtual Uri Url { get; }
		protected internal virtual IPaginationValidator Validator { get; }
		public virtual bool ZeroBasedIndex { get; }

		#endregion

		#region Methods

		protected internal virtual int AddNumbersSafely(int firstNumber, int secondNumber)
		{
			try
			{
				return checked(firstNumber + secondNumber);
			}
			catch(OverflowException)
			{
				return int.MaxValue;
			}
		}

		protected internal virtual Uri CreateUrl(int pageIndex)
		{
			try
			{
				if(!this.ZeroBasedIndex)
					pageIndex++;

				var uriBuilder = new UriBuilder(this.Url);
				var queryString = this.ParseQueryString(this.Url);

				queryString.Set(this.PageIndexKey, pageIndex.ToString(CultureInfo.InvariantCulture));
				uriBuilder.Query = queryString.ToString();

				return uriBuilder.Uri;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not create a page-indexed url for page-index \"{0}\" based on url \"{1}\".", pageIndex, this.Url), exception);
			}
		}

		protected internal virtual int MultiplyNumbersSafely(int firstNumber, int secondNumber)
		{
			try
			{
				return checked(firstNumber * secondNumber);
			}
			catch(OverflowException)
			{
				return int.MaxValue;
			}
		}

		protected internal virtual NameValueCollection ParseQueryString(Uri url)
		{
			if(url == null)
				throw new ArgumentNullException(nameof(url));

			try
			{
				return HttpUtility.ParseQueryString(url.Query);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not parse the query-string of url \"{0}\".", url), exception);
			}
		}

		#endregion
	}
}