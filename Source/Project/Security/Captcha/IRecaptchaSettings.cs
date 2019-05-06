using System;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaSettings
	{
		#region Properties

		Uri ClientScriptUrlFormat { get; }

		/// <summary>
		/// The maximum time elapsed since the timestamp in order to validate.
		/// </summary>
		TimeSpan MaximumTimestampElapse { get; }

		decimal MinimumScore { get; }

		/// <summary>
		/// The minimum time elapsed since the timestamp in order to validate.
		/// </summary>
		TimeSpan MinimumTimestampElapse { get; }

		/// <summary>
		/// To bind to this property by using a configuration-file, eg. AppSettings.json, declare the value as a comma-separated string: "EnabledOnClient, EnabledOnServer".
		/// </summary>
		RecaptchaModes Mode { get; }

		string SecretKey { get; }
		string SiteKey { get; }

		[Obsolete("This property will be removed. It is a misunderstanding. Use MaximumTimestampElapse and MinimumTimestampElapse instead.")]
		TimeSpan TimestampExpiration { get; }

		string TokenParameterName { get; }
		bool ValidateIp { get; }
		Uri ValidationUrl { get; }

		#endregion
	}
}