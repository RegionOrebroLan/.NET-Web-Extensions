using System;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RegionOrebroLan.Web.Paging;
using SampleApplication.Models.Forms;
using SampleApplication.Models.ViewModels;

namespace SampleApplication.Controllers
{
	public class ListController : SiteController
	{
		#region Constructors

		public ListController(IPaginationFactory paginationFactory)
		{
			this.PaginationFactory = paginationFactory ?? throw new ArgumentNullException(nameof(paginationFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IPaginationFactory PaginationFactory { get; }

		#endregion

		#region Methods

		public virtual IActionResult Index(PaginationSettingsForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			var url = new Uri(this.Request.GetDisplayUrl());

			var model = new ListViewModel
			{
				Form = form,
				Heading = "List",
				Pagination = this.PaginationFactory.Create(form.MaximumNumberOfDisplayedPages, form.NumberOfItems, "PageIndex", form.PageSize, url, form.ZeroBasedIndex),
				Title = "List"
			};

			for(var i = 0; i < form.NumberOfItems; i++)
			{
				var index = i + (form.ZeroBasedIndex ? 0 : 1);

				model.Items.Add("Item-" + index);
			}

			return this.View(model);
		}

		#endregion
	}
}