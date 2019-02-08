using System;
using System.Threading.Tasks;

namespace RegionOrebroLan.Web.Security.Captcha.Extensions
{
	public static class RecaptchaValidationClientExtension
	{
		#region Methods

		public static IRecaptchaValidationResult GetValidationResult(this IRecaptchaValidationClient recaptchaValidationClient, string token)
		{
			if(recaptchaValidationClient == null)
				throw new ArgumentNullException(nameof(recaptchaValidationClient));

			return recaptchaValidationClient.GetValidationResultAsync(token).Result;
		}

		public static IRecaptchaValidationResult GetValidationResult(this IRecaptchaValidationClient recaptchaValidationClient, string ip, string token)
		{
			if(recaptchaValidationClient == null)
				throw new ArgumentNullException(nameof(recaptchaValidationClient));

			return recaptchaValidationClient.GetValidationResultAsync(ip, token).Result;
		}

		public static async Task<IRecaptchaValidationResult> GetValidationResultAsync(this IRecaptchaValidationClient recaptchaValidationClient, string token)
		{
			if(recaptchaValidationClient == null)
				throw new ArgumentNullException(nameof(recaptchaValidationClient));

			return await recaptchaValidationClient.GetValidationResultAsync(null, token).ConfigureAwait(false);
		}

		#endregion
	}
}