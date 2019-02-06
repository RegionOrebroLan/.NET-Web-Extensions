using System;
using RegionOrebroLan.ServiceLocation;

namespace SampleApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Request, ServiceType = typeof(ILayout))]
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