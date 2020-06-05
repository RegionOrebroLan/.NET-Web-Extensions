using System;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;

namespace SampleApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(Lifetime = ServiceLifetime.Scoped, ServiceType = typeof(ILayout))]
	public class Layout : ILayout
	{
		#region Constructors

		public Layout(ISettings settings)
		{
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		#endregion

		#region Properties

		public virtual ISettings Settings { get; }

		#endregion
	}
}