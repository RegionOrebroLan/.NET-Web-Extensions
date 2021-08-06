using Application.Models.ViewModels.Shared;

namespace Application.Models.ViewModels
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