using SampleApplication.Models.ViewModels.Shared;

namespace SampleApplication.Models.ViewModels
{
	public class ViewModel : IViewModel
	{
		#region Properties

		public virtual string Heading { get; set; }
		public virtual ILayout Layout { get; set; }
		public virtual string Title { get; set; }

		#endregion
	}
}