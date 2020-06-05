using System;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;

namespace SampleApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(Lifetime = ServiceLifetime.Scoped, ServiceType = typeof(ISettings))]
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