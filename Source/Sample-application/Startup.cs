using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.ServiceLocation;
using RegionOrebroLan.ServiceLocation.Extensions;
using RegionOrebroLan.Web.Paging;

namespace SampleApplication
{
	public class Startup
	{
		#region Constructors

		public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IConfiguration Configuration { get; }
		protected internal virtual IHostingEnvironment HostingEnvironment { get; }
		protected internal virtual ILoggerFactory LoggerFactory { get; }

		#endregion

		#region Methods

		public virtual void Configure(IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder.UseDeveloperExceptionPage();
			applicationBuilder.UseStaticFiles();
			applicationBuilder.UseMvcWithDefaultRoute();
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			var assembliesToScan = new[] {typeof(IPagination).Assembly};

			foreach(var mapping in new ServiceConfigurationScanner().Scan(assembliesToScan))
			{
				services.Add(new ServiceDescriptor(mapping.Configuration.ServiceType, mapping.Type, this.GetServiceLifetime(mapping)));
			}

			services.AddMvc();
		}

		[SuppressMessage("Microsoft.Style", "IDE0010:Add missing cases")]
		protected internal virtual ServiceLifetime GetServiceLifetime(IServiceConfigurationMapping serviceConfigurationMapping)
		{
			if(serviceConfigurationMapping == null)
				throw new ArgumentNullException(nameof(serviceConfigurationMapping));

			// ReSharper disable SwitchStatementMissingSomeCases
			switch(serviceConfigurationMapping.Configuration.InstanceMode)
			{
				case InstanceMode.Request:
				case InstanceMode.Thread:
					return ServiceLifetime.Scoped;
				case InstanceMode.Singleton:
					return ServiceLifetime.Singleton;
				default:
					return ServiceLifetime.Transient;
			}
			// ReSharper restore SwitchStatementMissingSomeCases
		}

		#endregion
	}
}