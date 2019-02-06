namespace SampleApplication.Models.ViewModels.Shared
{
	public interface ISettings
	{
		#region Properties

		IRecaptcha Recaptcha { get; }

		#endregion
	}
}