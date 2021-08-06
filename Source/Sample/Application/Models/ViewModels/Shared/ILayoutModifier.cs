namespace Application.Models.ViewModels.Shared
{
	public interface ILayoutModifier
	{
		#region Methods

		void ModifyLayout(ILayout layout);

		#endregion
	}
}