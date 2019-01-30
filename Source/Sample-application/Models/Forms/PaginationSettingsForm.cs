using System.ComponentModel.DataAnnotations;

namespace SampleApplication.Models.Forms
{
	public class PaginationSettingsForm
	{
		#region Properties

		[Display(Name = "Maximum number of displayed pages", Order = 3)]
		[Range(1, 50)]
		[Required]
		public virtual int MaximumNumberOfDisplayedPages { get; set; } = 10;

		[Display(Description = "The number of items in the list to use pagination on.", Name = "Number of items", Order = 1)]
		[Range(0, int.MaxValue)]
		[Required]
		public virtual int NumberOfItems { get; set; } = 1000;

		[Display(Name = "Page-size", Order = 2)]
		[Range(1, 1000)]
		[Required]
		public virtual int PageSize { get; set; } = 10;

		[Display(Name = "Zero-based index", Order = 4)]
		public virtual bool ZeroBasedIndex { get; set; }

		#endregion
	}
}