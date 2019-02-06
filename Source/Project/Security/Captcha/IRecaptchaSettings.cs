using System;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaSettings
	{
		#region Properties

		Uri ClientScriptUrlFormat { get; }
		decimal MinimumScore { get; }

		/// <summary>
		/// To bind to this property by using a configuration-file, eg. AppSettings.json, declare the value as a comma-separated string: "EnabledOnClient, EnabledOnServer".
		/// </summary>
		RecaptchaModes Mode { get; }

		string SecretKey { get; }
		string SiteKey { get; }
		TimeSpan TimestampExpiration { get; }
		string TokenParameterName { get; }
		bool ValidateIp { get; }
		Uri ValidationUrl { get; }

		#endregion
	}
}