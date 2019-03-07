namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaClientActionResolver
	{
		#region Methods

		string Resolve(string action);

		#endregion
	}
}