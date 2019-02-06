using SampleApplication.Models.ViewModels.Shared;

namespace SampleApplication.Models.ViewModels
{
	public interface IViewModel
	{
		#region Properties

		string Heading { get; set; }
		ILayout Layout { get; set; }
		string Title { get; set; }

		#endregion
	}
}