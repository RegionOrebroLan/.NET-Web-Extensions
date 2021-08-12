using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Web.Security.Captcha.DependencyInjection.Extensions.Internal
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddRecaptchaDependencies(this IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<IRecaptchaClientActionResolver, RecaptchaClientActionResolver>();
			services.TryAddSingleton<IRecaptchaValidationClient, RecaptchaValidationClient>();
			services.TryAddSingleton<IRecaptchaValidator, RecaptchaValidator>();
			services.TryAddSingleton<ISystemClock, SystemClock>();

			return services;
		}

		#endregion
	}
}