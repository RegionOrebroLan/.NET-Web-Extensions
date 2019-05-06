using System;
using System.Diagnostics.CodeAnalysis;

namespace RegionOrebroLan.Web.Security.Captcha
{
	/// <inheritdoc />
	[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
	public class RecaptchaSettings : IRecaptchaSettings
	{
		#region Fields

		private Uri _clientScriptUrlFormat = new Uri("https://www.google.com/recaptcha/api.js?render={0}");
		private Uri _validationUrl = new Uri("https://www.google.com/recaptcha/api/siteverify");

		#endregion

		#region Properties

		public virtual Uri ClientScriptUrlFormat
		{
			get => this._clientScriptUrlFormat;
			set
			{
				this.ValidateUrl(value);
				this._clientScriptUrlFormat = value;
			}
		}

		public virtual TimeSpan MaximumTimestampElapse { get; set; } = TimeSpan.FromHours(4);
		public virtual decimal MinimumScore { get; set; } = 0.1m;
		public virtual TimeSpan MinimumTimestampElapse { get; set; } = TimeSpan.FromSeconds(1);
		public virtual RecaptchaModes Mode { get; set; } = RecaptchaModes.EnabledOnClient | RecaptchaModes.EnabledOnServer;
		public virtual string SecretKey { get; set; }
		public virtual string SiteKey { get; set; }

		[Obsolete("This property will be removed. It is a misunderstanding. Use MaximumTimestampElapse and MinimumTimestampElapse instead.")]
		public virtual TimeSpan TimestampExpiration { get; set; } = TimeSpan.FromMinutes(1);

		public virtual string TokenParameterName { get; set; } = "RecaptchaToken";
		public virtual bool ValidateIp { get; set; } = true;

		public virtual Uri ValidationUrl
		{
			get => this._validationUrl;
			set
			{
				this.ValidateUrl(value);
				this._validationUrl = value;
			}
		}

		#endregion

		#region Methods

		protected internal virtual void ValidateUrl(Uri value)
		{
			if(value != null && !value.IsAbsoluteUri)
				throw new ArgumentException("The url must be absolute.", nameof(value));
		}

		#endregion
	}
}