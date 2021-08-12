using System;
using System.Globalization;

namespace RegionOrebroLan.Web.Security.Captcha.Configuration.Extensions
{
	public static class RecaptchaOptionsExtension
	{
		#region Methods

		public static Uri ClientScriptUrl(this RecaptchaOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			// ReSharper disable InvertIf
			if(options.ClientScriptUrlFormat != null)
			{
				var value = options.ClientScriptUrlFormat.OriginalString;

				if(value.Contains("{0}"))
					return new Uri(string.Format(CultureInfo.InvariantCulture, value, options.SiteKey));
			}
			// ReSharper restore InvertIf

			return options.ClientScriptUrlFormat;
		}

		public static bool EnabledOnClient(this RecaptchaOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return (options.Mode & RecaptchaModes.EnabledOnClient) == RecaptchaModes.EnabledOnClient;
		}

		public static bool EnabledOnServer(this RecaptchaOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return (options.Mode & RecaptchaModes.EnabledOnServer) == RecaptchaModes.EnabledOnServer;
		}

		#endregion
	}
}