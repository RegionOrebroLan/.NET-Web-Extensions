﻿using System;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace SampleApplication.Models.ViewModels.Shared
{
	/// <inheritdoc />
	[ServiceConfiguration(Lifetime = ServiceLifetime.Scoped, ServiceType = typeof(IRecaptcha))]
	public class Recaptcha : IRecaptcha
	{
		#region Fields

		private bool _enabled;

		#endregion

		#region Constructors

		public Recaptcha(IRecaptchaSettings settings)
		{
			if(settings == null)
				throw new ArgumentNullException(nameof(settings));

			this.EnabledInternal = settings.EnabledOnClient();
			this.ScriptUrl = settings.ClientScriptUrl();
			this.SiteKey = settings.SiteKey;
			this.TokenParameterName = settings.TokenParameterName;
		}

		#endregion

		#region Properties

		public virtual bool Enabled
		{
			get => this._enabled && this.EnabledInternal;
			set => this._enabled = value;
		}

		protected internal virtual bool EnabledInternal { get; }
		public virtual Uri ScriptUrl { get; }
		public virtual string SiteKey { get; }
		public virtual string TokenParameterName { get; }

		#endregion
	}
}