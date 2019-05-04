using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RegionOrebroLan.Web.Security.Captcha.Extensions
{
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
	public static class RecaptchaValidatorExtension
	{
		#region Methods

		public static bool IsValid(this IRecaptchaValidator recaptchaValidator, IRecaptchaRequest request, out Exception exception)
		{
			if(recaptchaValidator == null)
				throw new ArgumentNullException(nameof(recaptchaValidator));

			// ReSharper disable UseDeconstruction
			var result = recaptchaValidator.IsValidAsync(request).Result;
			// ReSharper restore UseDeconstruction

			exception = result.Item2;

			return result.Item1;
		}

		public static async Task<Tuple<bool, Exception>> IsValidAsync(this IRecaptchaValidator recaptchaValidator, IRecaptchaRequest request)
		{
			if(recaptchaValidator == null)
				throw new ArgumentNullException(nameof(recaptchaValidator));

			try
			{
				await recaptchaValidator.ValidateAsync(request).ConfigureAwait(false);

				return new Tuple<bool, Exception>(true, null);
			}
			catch(Exception exception)
			{
				return new Tuple<bool, Exception>(false, exception);
			}
		}

		public static void Validate(this IRecaptchaValidator recaptchaValidator, IRecaptchaRequest request)
		{
			if(recaptchaValidator == null)
				throw new ArgumentNullException(nameof(recaptchaValidator));

			recaptchaValidator.ValidateAsync(request).Wait();
		}

		#endregion
	}
}