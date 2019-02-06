using System;
using RegionOrebroLan.ServiceLocation;

namespace SampleApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Request, ServiceType = typeof(ISettings))]
	public class Settings : ISettings
	{
		#region Constructors

		public Settings(IRecaptcha recaptcha)
		{
			this.Recaptcha = recaptcha ?? throw new ArgumentNullException(nameof(recaptcha));
		}

		#endregion

		#region Properties

		public virtual IRecaptcha Recaptcha { get; }

		#endregion
	}
}