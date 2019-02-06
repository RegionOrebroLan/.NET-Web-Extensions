using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace RegionOrebroLan.Web.Security.Captcha.Extensions
{
	public static class RecaptchaSettingsExtension
	{
		#region Methods

		[SuppressMessage("Design", "CA1055:Uri return values should not be strings")]
		public static Uri ClientScriptUrl(this IRecaptchaSettings recaptchaSettings)
		{
			if(recaptchaSettings == null)
				throw new ArgumentNullException(nameof(recaptchaSettings));

			// ReSharper disable InvertIf
			if(recaptchaSettings.ClientScriptUrlFormat != null)
			{
				var value = recaptchaSettings.ClientScriptUrlFormat.OriginalString;

				if(value.Contains("{0}"))
					return new Uri(string.Format(CultureInfo.InvariantCulture, value, recaptchaSettings.SiteKey));
			}
			// ReSharper restore InvertIf

			return recaptchaSettings.ClientScriptUrlFormat;
		}

		public static bool EnabledOnClient(this IRecaptchaSettings recaptchaSettings)
		{
			if(recaptchaSettings == null)
				throw new ArgumentNullException(nameof(recaptchaSettings));

			return (recaptchaSettings.Mode & RecaptchaModes.EnabledOnClient) == RecaptchaModes.EnabledOnClient;
		}

		public static bool EnabledOnServer(this IRecaptchaSettings recaptchaSettings)
		{
			if(recaptchaSettings == null)
				throw new ArgumentNullException(nameof(recaptchaSettings));

			return (recaptchaSettings.Mode & RecaptchaModes.EnabledOnServer) == RecaptchaModes.EnabledOnServer;
		}

		#endregion
	}
}