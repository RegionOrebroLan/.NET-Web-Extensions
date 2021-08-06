using Application.Models.ViewModels.Shared;

namespace Application.Models.ViewModels
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