using System.Threading.Tasks;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaValidationClient
	{
		#region Methods

		/// <summary>
		/// Validate the client-side token.
		/// </summary>
		/// <param name="ip">Optional. The user's IP address.</param>
		/// <param name="token">Required. The user response token provided by the reCAPTCHA client-side integration on your site.</param>
		/// <returns></returns>
		Task<IRecaptchaValidationResult> GetValidationResultAsync(string ip, string token);

		#endregion
	}
}