using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Paging;

namespace IntegrationTests.Paging
{
	[TestClass]
	public class PaginationFactoryTest
	{
		#region Fields

		private static readonly IPaginationValidator _validator = new PaginationValidator();
		private static readonly IPaginationFactory _paginationFactory = new PaginationFactory(_validator);

		#endregion

		#region Properties

		protected internal virtual IPaginationFactory PaginationFactory => _paginationFactory;

		#endregion

		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfTheMaximumNumberOfDisplayedPagesParameterIsLessThanOne_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(0, 10, "Test", 10, new Uri("http://localhost"));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("maximumNumberOfDisplayedPages", StringComparison.Ordinal) && argumentException.Message.StartsWith("The maximum number of displayed pages can not be less than 1.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfTheNumberOfItemsParameterIsLessThanZero_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, -1, "Test", 10, new Uri("http://localhost"));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("numberOfItems", StringComparison.Ordinal) && argumentException.Message.StartsWith("The number of items can not be less than 0.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfThePageIndexKeyParameterHasADifferentUrlEncodedValue_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			const string pageIndexKey = " Test ";

			try
			{
				this.PaginationFactory.Create(10, 10, pageIndexKey, 10, new Uri("http://localhost"));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("pageIndexKey", StringComparison.Ordinal) && argumentException.Message.StartsWith("The page-index-key \"" + pageIndexKey + "\" is invalid.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfThePageIndexKeyParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, 10, string.Empty, 10, new Uri("http://localhost"));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("pageIndexKey", StringComparison.Ordinal) && argumentException.Message.StartsWith("The page-index-key can not be empty or whitespace.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task Create_IfThePageIndexKeyParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, 10, null, 10, new Uri("http://localhost"));
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName.Equals("pageIndexKey", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfThePageIndexKeyParameterIsWhitespace_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, 10, "    ", 10, new Uri("http://localhost"));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("pageIndexKey", StringComparison.Ordinal) && argumentException.Message.StartsWith("The page-index-key can not be empty or whitespace.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Create_IfTheUrlParameterIsNotAbsolute_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, 10, "Test", 10, new Uri("/Test/", UriKind.Relative));
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("url", StringComparison.Ordinal) && argumentException.Message.StartsWith("The url must be absolute.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task Create_IfTheUrlParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			try
			{
				this.PaginationFactory.Create(10, 10, "Test", 10, null);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName.Equals("url", StringComparison.Ordinal))
					throw;
			}
		}

		#endregion
	}
}