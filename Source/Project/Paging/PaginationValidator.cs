using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using RegionOrebroLan.DependencyInjection;

namespace RegionOrebroLan.Web.Paging
{
	[ServiceConfiguration(ServiceType = typeof(IPaginationValidator))]
	public class PaginationValidator : IPaginationValidator
	{
		#region Methods

		public virtual void ValidateMaximumNumberOfDisplayedPages(int maximumNumberOfDisplayedPages)
		{
			if(maximumNumberOfDisplayedPages < 1)
				throw new ArgumentException("The maximum number of displayed pages can not be less than 1.", nameof(maximumNumberOfDisplayedPages));
		}

		public virtual void ValidateNumberOfItems(int numberOfItems)
		{
			if(numberOfItems < 0)
				throw new ArgumentException("The number of items can not be less than 0.", nameof(numberOfItems));
		}

		[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code")]
		public virtual void ValidatePageIndexKey(string pageIndexKey)
		{
			if(pageIndexKey == null)
				throw new ArgumentNullException(nameof(pageIndexKey));

			if(string.IsNullOrWhiteSpace(pageIndexKey))
				throw new ArgumentException("The page-index-key can not be empty or whitespace.", nameof(pageIndexKey));

			var uriBuilder = new UriBuilder("http://localhost/")
			{
				Query = pageIndexKey + "="
			};

			if(!uriBuilder.Uri.Query.Equals("?" + pageIndexKey + "=", StringComparison.Ordinal))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The page-index-key \"{0}\" is invalid.", pageIndexKey), nameof(pageIndexKey));
		}

		public virtual void ValidatePageSize(int pageSize)
		{
			if(pageSize < 1)
				throw new ArgumentException("The page-size can not be less than 1.", nameof(pageSize));
		}

		public virtual void ValidateUrl(Uri url)
		{
			if(url == null)
				throw new ArgumentNullException(nameof(url));

			if(!url.IsAbsoluteUri)
				throw new ArgumentException("The url must be absolute.", nameof(url));
		}

		#endregion
	}
}