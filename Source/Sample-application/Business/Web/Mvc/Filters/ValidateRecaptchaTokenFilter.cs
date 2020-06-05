using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace SampleApplication.Business.Web.Mvc.Filters
{
	[ServiceConfiguration(ServiceType = typeof(ValidateRecaptchaTokenFilter))]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
	public class ValidateRecaptchaTokenFilter : IAsyncActionFilter
	{
		#region Fields

		private const string _modelErrorKey = "Recaptcha";

		#endregion

		#region Constructors

		public ValidateRecaptchaTokenFilter(IRecaptchaClientActionResolver clientActionResolver, IModelMetadataProvider modelMetadataProvider, IRecaptchaRequestFactory requestFactory, IRecaptchaSettings settings, IRecaptchaValidator validator)
		{
			this.ClientActionResolver = clientActionResolver ?? throw new ArgumentNullException(nameof(clientActionResolver));
			this.ModelMetadataProvider = modelMetadataProvider ?? throw new ArgumentNullException(nameof(modelMetadataProvider));
			this.RequestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		#endregion

		#region Properties

		protected internal virtual IRecaptchaClientActionResolver ClientActionResolver { get; }
		protected internal virtual string ModelErrorKey => _modelErrorKey;
		protected internal virtual IModelMetadataProvider ModelMetadataProvider { get; }
		protected internal virtual IRecaptchaRequestFactory RequestFactory { get; }
		protected internal virtual IRecaptchaSettings Settings { get; }
		protected internal virtual IRecaptchaValidator Validator { get; }

		#endregion

		#region Methods

		protected internal virtual string GetAction(ActionExecutingContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			// ReSharper disable InvertIf
			if(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
			{
				var validateRecaptchaTokenAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).OfType<IValidateRecaptchaTokenAttribute>().FirstOrDefault();

				var action = validateRecaptchaTokenAttribute?.Action;

				if(action != null)
					return action;
			}
			// ReSharper restore InvertIf

			return this.GetDefaultAction(context);
		}

		protected internal virtual string GetDefaultAction(ActionExecutingContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			return context.HttpContext.Features.Get<IHttpRequestFeature>().RawTarget;
		}

		protected internal virtual string GetExceptionMessage(Exception exception)
		{
			var messages = new List<string>();

			while(exception != null)
			{
				if(!string.IsNullOrWhiteSpace(exception.Message))
					messages.Add(exception.Message);

				exception = exception.InnerException;
			}

			return string.Join(" -> ", messages);
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
		[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
		public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			await this.ValidateTokenIfNecessaryAsync(context).ConfigureAwait(false);

			await next().ConfigureAwait(false);
		}

		protected internal virtual async Task ValidateTokenIfNecessaryAsync(ActionExecutingContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(!this.Settings.EnabledOnServer())
				return;

			var action = this.ClientActionResolver.Resolve(this.GetAction(context));

			var request = this.RequestFactory.Create(action);

			try
			{
				await this.Validator.ValidateAsync(request).ConfigureAwait(false);
			}
			catch(Exception exception)
			{
				context.ModelState.AddModelError(this.ModelErrorKey, this.GetExceptionMessage(exception));
			}
		}

		#endregion
	}
}