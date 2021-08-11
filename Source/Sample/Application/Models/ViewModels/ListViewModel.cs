using System.Collections.Generic;
using Application.Models.Forms;
using RegionOrebroLan.Web.Paging;

namespace Application.Models.ViewModels
{
	public class ListViewModel
	{
		#region Properties

		public virtual PaginationSettingsForm Form { get; set; } = new PaginationSettingsForm();
		public virtual IList<string> Items { get; } = new List<string>();
		public virtual IPagination Pagination { get; set; }

		#endregion
	}
}