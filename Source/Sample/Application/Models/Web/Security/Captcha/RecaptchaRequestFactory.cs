using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Security.Captcha;

namespace Application.Models.Web.Security.Captcha
{
	/// <inheritdoc />
	[ServiceConfiguration(ServiceType = typeof(IRecaptchaRequestFactory))]
	public class RecaptchaRequestFactory : IRecaptchaRequestFactory
	{
		#region Constructors

		public RecaptchaRequestFactory(IHttpContextAccessor httpContextAccessor, IOptionsMonitor<RecaptchaSettings> optionsMonitor)
		{
			this.HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual IHttpContextAccessor HttpContextAccessor { get; }
		protected internal virtual IOptionsMonitor<RecaptchaSettings> OptionsMonitor { get; }

		#endregion

		#region Methods

		public virtual IRecaptchaRequest Create(string action)
		{
			var httpContext = this.HttpContextAccessor.HttpContext;

			return new RecaptchaRequest
			{
				Action = action,
				Host = this.GetHost(httpContext),
				Ip = this.GetIp(httpContext),
				Token = this.GetToken(httpContext)
			};
		}

		protected internal virtual string GetHost(HttpContext httpContext)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			return httpContext.Request.Host.Host;
		}

		protected internal virtual IPAddress GetIp(HttpContext httpContext)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			return httpContext.Connection.RemoteIpAddress;
		}

		protected internal virtual string GetToken(HttpContext httpContext)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			var httpRequest = httpContext.Request;
			var tokenParameterName = this.OptionsMonitor.CurrentValue.TokenParameterName;

			// ReSharper disable ConvertIfStatementToReturnStatement
			if(string.Equals(httpRequest.Method, "post", StringComparison.OrdinalIgnoreCase))
				return httpRequest.Form[tokenParameterName];
			// ReSharper restore ConvertIfStatementToReturnStatement

			return httpRequest.Query[tokenParameterName];
		}

		#endregion
	}
}