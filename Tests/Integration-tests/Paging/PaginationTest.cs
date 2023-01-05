using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Paging;
using RegionOrebroLan.Web.Paging.Extensions;

namespace IntegrationTests.Paging
{
	[TestClass]
	public class PaginationTest
	{
		#region Fields

		private static readonly Uri _defaultUrl = new Uri("http://localhost/");
		private const string _pageIndexKey = "PageIndex";
		private static readonly IPaginationValidator _validator = new PaginationValidator();

		#endregion

		#region Properties

		public virtual Uri DefaultUrl => _defaultUrl;
		public virtual string PageIndexKey => _pageIndexKey;
		public virtual IPaginationValidator Validator => _validator;

		#endregion

		#region Methods

		/*
			NumberOfItems = 1000 & PageSize = 10 => TotalNumberOfPages = 100,
			NumberOfItems = 436 & PageSize = 19 => TotalNumberOfPages = 23,
			NumberOfItems = 194 & PageSize = 3 => TotalNumberOfPages = 65,
			NumberOfItems = 8 & PageSize = 3 => TotalNumberOfPages = 3,
			NumberOfItems = 2 & PageSize = 5 => TotalNumberOfPages = 0,
		*/

		protected internal virtual async Task<Uri> CreateUrlAsync(int pageIndex)
		{
			return await this.CreateUrlAsync(pageIndex.ToString(CultureInfo.InvariantCulture));
		}

		protected internal virtual async Task<Uri> CreateUrlAsync(string pageIndexValue)
		{
			return await Task.FromResult(new Uri("http://localhost/?" + this.PageIndexKey + "=" + pageIndexValue));
		}

		[TestMethod]
		public async Task FirstPageUrl_Test()
		{
			var pagination = new Pagination(10, 436, this.PageIndexKey, 19, await this.CreateUrlAsync(-5), this.Validator, false);
			Assert.IsNull(pagination.FirstPageUrl);

			pagination = new Pagination(10, 436, this.PageIndexKey, 19, await this.CreateUrlAsync(0), this.Validator, false);
			Assert.IsNull(pagination.FirstPageUrl);

			pagination = new Pagination(10, 436, this.PageIndexKey, 19, await this.CreateUrlAsync(1), this.Validator, false);
			Assert.IsNull(pagination.FirstPageUrl);

			pagination = new Pagination(10, 436, this.PageIndexKey, 19, await this.CreateUrlAsync(2), this.Validator, false);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=1"), pagination.FirstPageUrl);
		}

		[TestMethod]
		public async Task LastPageUrl_Test()
		{
			await Task.CompletedTask;

			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=100"), pagination.LastPageUrl);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, true);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=99"), pagination.LastPageUrl);
		}

		[TestMethod]
		public async Task NextPageGroupUrl_Test()
		{
			await Task.CompletedTask;

			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=11"), pagination.NextPageGroupUrl);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, true);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=10"), pagination.NextPageGroupUrl);
		}

		[TestMethod]
		public async Task Pages_Test()
		{
			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			var pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(0).Selected);
			Assert.AreEqual(1, pages.ElementAt(0).Index);
			Assert.AreEqual(10, pages.ElementAt(9).Index);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, true);
			pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(0).Selected);
			Assert.AreEqual(0, pages.ElementAt(0).Index);
			Assert.AreEqual(9, pages.ElementAt(9).Index);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(25), this.Validator, false);
			pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(4).Selected);
			Assert.AreEqual(21, pages.ElementAt(0).Index);
			Assert.AreEqual(30, pages.ElementAt(9).Index);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(25), this.Validator, true);
			pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(5).Selected);
			Assert.AreEqual(20, pages.ElementAt(0).Index);
			Assert.AreEqual(29, pages.ElementAt(9).Index);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(100), this.Validator, false);
			pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(9).Selected);
			Assert.AreEqual(91, pages.ElementAt(0).Index);
			Assert.AreEqual(100, pages.ElementAt(9).Index);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(100), this.Validator, true);
			pages = pagination.Pages.ToArray();
			Assert.AreEqual(10, pages.Length);
			Assert.IsTrue(pages.ElementAt(9).Selected);
			Assert.AreEqual(90, pages.ElementAt(0).Index);
			Assert.AreEqual(99, pages.ElementAt(9).Index);
		}

		[TestMethod]
		public async Task PreviousPageGroupUrl_Test()
		{
			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			Assert.IsNull(pagination.PreviousPageGroupUrl);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, true);
			Assert.IsNull(pagination.PreviousPageGroupUrl);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(35), this.Validator, false);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=30"), pagination.PreviousPageGroupUrl);

			pagination = new Pagination(10, 1000, this.PageIndexKey, 10, await this.CreateUrlAsync(35), this.Validator, true);
			Assert.AreEqual(new Uri("http://localhost/?PageIndex=29"), pagination.PreviousPageGroupUrl);
		}

		[TestMethod]
		public async Task SelectedPage_Test()
		{
			await Task.CompletedTask;

			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			Assert.AreEqual(1, pagination.SelectedPage().Index);
		}

		[TestMethod]
		public async Task Skip_Test()
		{
			await Task.CompletedTask;

			var pagination = new Pagination(10, 1000, this.PageIndexKey, 10, this.DefaultUrl, this.Validator, false);
			Assert.AreEqual(0, pagination.Skip);
		}

		#endregion
	}
}