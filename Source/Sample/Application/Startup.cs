using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.DependencyInjection.Extensions;
using RegionOrebroLan.Web.Security.Captcha;

namespace Application
{
	public class Startup
	{
		#region Constructors

		public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment, ILoggerFactory loggerFactory)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.HostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IConfiguration Configuration { get; }
		protected internal virtual IHostEnvironment HostEnvironment { get; }
		protected internal virtual ILoggerFactory LoggerFactory { get; }

		#endregion

		#region Methods

		public virtual void Configure(IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder.UseDeveloperExceptionPage();
			applicationBuilder.UseStaticFiles();
			applicationBuilder.UseRouting();
			applicationBuilder.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.Configure<RecaptchaSettings>(this.Configuration.GetSection("Recaptcha"));

			services.AddTransient<IRecaptchaSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptionsMonitor<RecaptchaSettings>>().CurrentValue);

			var assembliesToScan = new[] { typeof(RegionOrebroLan.IDateTimeContext).Assembly, typeof(RegionOrebroLan.Web.Paging.IPagination).Assembly, this.GetType().Assembly };

			foreach(var mapping in new ServiceConfigurationScanner().Scan(assembliesToScan))
			{
				services.Add(new ServiceDescriptor(mapping.Configuration.ServiceType, mapping.Type, mapping.Configuration.Lifetime));
			}

			services.AddHttpContextAccessor();

			services.AddControllersWithViews();
		}

		#endregion
	}
}