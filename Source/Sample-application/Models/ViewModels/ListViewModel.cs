using System.Collections.Generic;
using RegionOrebroLan.Web.Paging;
using SampleApplication.Models.Forms;

namespace SampleApplication.Models.ViewModels
{
	public class ListViewModel : ViewModel
	{
		#region Properties

		public virtual PaginationSettingsForm Form { get; set; } = new PaginationSettingsForm();
		public virtual IList<string> Items { get; } = new List<string>();
		public virtual IPagination Pagination { get; set; }

		#endregion
	}
}