using System;
using RegionOrebroLan.ServiceLocation;

namespace RegionOrebroLan.Web.Paging
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Singleton, ServiceType = typeof(IPaginationFactory))]
	public class PaginationFactory : IPaginationFactory
	{
		#region Constructors

		public PaginationFactory(IPaginationValidator validator)
		{
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		#endregion

		#region Properties

		protected internal virtual IPaginationValidator Validator { get; }
		public virtual bool ZeroBasedIndex { get; set; }

		#endregion

		#region Methods

		public virtual IPagination Create(int maximumNumberOfDisplayedPages, int numberOfItems, string pageIndexKey, int pageSize, Uri url)
		{
			return this.Create(maximumNumberOfDisplayedPages, numberOfItems, pageIndexKey, pageSize, url, this.ZeroBasedIndex);
		}

		public virtual IPagination Create(int maximumNumberOfDisplayedPages, int numberOfItems, string pageIndexKey, int pageSize, Uri url, bool zeroBasedIndex)
		{
			return new Pagination(maximumNumberOfDisplayedPages, numberOfItems, pageIndexKey, pageSize, url, this.Validator, zeroBasedIndex);
		}

		#endregion
	}
}