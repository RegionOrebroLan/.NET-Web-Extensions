using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SampleApplication.Models.ViewModels;
using SampleApplication.Models.ViewModels.Shared;

namespace SampleApplication.Business.Web.Mvc.Filters
{
	public class ViewModelFilter : IActionFilter
	{
		#region Constructors

		public ViewModelFilter(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		#endregion

		#region Properties

		protected internal virtual IServiceProvider ServiceProvider { get; }

		#endregion

		#region Methods

		public virtual void OnActionExecuted(ActionExecutedContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(!(context.Controller is Controller controller))
				return;

			if(!(controller.ViewData?.Model is IViewModel viewModel))
				return;

			viewModel.Layout = this.ServiceProvider.GetRequiredService<ILayout>();

			if(controller is ILayoutModifier layoutModifier)
				layoutModifier.ModifyLayout(viewModel.Layout);
		}

		public virtual void OnActionExecuting(ActionExecutingContext context) { }

		#endregion
	}
}