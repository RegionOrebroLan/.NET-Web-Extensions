using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;

namespace Application.Models.ViewModels.Shared
{
	[ServiceConfiguration(Lifetime = ServiceLifetime.Scoped, ServiceType = typeof(ILayout))]
	[SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
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