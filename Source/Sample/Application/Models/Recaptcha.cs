using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace Application.Models
{
	/// <inheritdoc />
	[ServiceConfiguration(Lifetime = ServiceLifetime.Scoped, ServiceType = typeof(IRecaptcha))]
	public class Recaptcha : IRecaptcha
	{
		#region Constructors

		public Recaptcha(IOptionsMonitor<RecaptchaSettings> recaptchaOptionsMonitor)
		{
			var recaptchaOptions = (recaptchaOptionsMonitor ?? throw new ArgumentNullException(nameof(recaptchaOptionsMonitor))).CurrentValue;

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