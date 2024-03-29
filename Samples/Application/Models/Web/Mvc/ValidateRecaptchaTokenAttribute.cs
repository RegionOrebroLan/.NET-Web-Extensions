using System;
using Application.Models.Web.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Models.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ValidateRecaptchaTokenAttribute : Attribute, IFilterFactory, IOrderedFilter, IValidateRecaptchaTokenAttribute
	{
		#region Properties

		public string Action { get; set; }

		/// <inheritdoc />
		public bool IsReusable => true;

		/// <inheritdoc />
		public int Order { get; set; }

		#endregion

		#region Methods

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			if(serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			return serviceProvider.GetRequiredService<ValidateRecaptchaTokenFilter>();
		}

		#endregion
	}
}