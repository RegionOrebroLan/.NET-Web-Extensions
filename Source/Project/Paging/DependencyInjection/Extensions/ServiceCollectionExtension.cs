using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RegionOrebroLan.Web.Paging.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddPaging(this IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<IPaginationFactory, PaginationFactory>();
			services.TryAddSingleton<IPaginationValidator, PaginationValidator>();

			return services;
		}

		#endregion
	}
}