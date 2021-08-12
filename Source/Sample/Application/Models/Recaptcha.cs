using System;
using Microsoft.Extensions.Options;
using RegionOrebroLan.Web.Security.Captcha.Configuration;
using RegionOrebroLan.Web.Security.Captcha.Configuration.Extensions;

namespace Application.Models
{
	/// <inheritdoc />
	public class Recaptcha : IRecaptcha
	{
		#region Constructors

		public Recaptcha(IOptionsMonitor<RecaptchaOptions> optionsMonitor)
		{
			var recaptchaOptions = (optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor))).CurrentValue;

			this.Enabled = recaptchaOptions.EnabledOnClient();
			this.ScriptUrl = recaptchaOptions.ClientScriptUrl();
			this.SiteKey = recaptchaOptions.SiteKey;
			this.TokenParameterName = recaptchaOptions.TokenParameterName;
		}

		#endregion

		#region Properties

		public virtual bool Enabled { get; }
		public virtual Uri ScriptUrl { get; }
		public virtual string SiteKey { get; }
		public virtual string TokenParameterName { get; }

		#endregion
	}
}