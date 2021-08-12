using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Web.Security.Captcha.Configuration;
using RegionOrebroLan.Web.Security.Captcha.DependencyInjection.Extensions.Internal;

namespace RegionOrebroLan.Web.Security.Captcha.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddRecaptcha(this IServiceCollection services, Action<RecaptchaOptions> configureOptions)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configureOptions == null)
				throw new ArgumentNullException(nameof(configureOptions));

			services.Configure(configureOptions);

			services.AddRecaptchaDependencies();

			return services;
		}

		public static IServiceCollection AddRecaptcha(this IServiceCollection services, IConfiguration configuration, string recaptchaSectionPath = ConfigurationKeys.RecaptchaPath)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(recaptchaSectionPath == null)
				throw new ArgumentNullException(nameof(recaptchaSectionPath));

			services.Configure<RecaptchaOptions>(configuration.GetSection(recaptchaSectionPath));

			services.AddRecaptchaDependencies();

			return services;
		}

		#endregion
	}
}