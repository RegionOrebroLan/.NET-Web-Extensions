namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaRequestFactory
	{
		#region Methods

		IRecaptchaRequest Create(string action);

		#endregion
	}
}