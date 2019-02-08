﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RegionOrebroLan.ServiceLocation;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace SampleApplication.Business.Web.Mvc.Filters
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Singleton, ServiceType = typeof(ValidateRecaptchaTokenFilter))]
	public class ValidateRecaptchaTokenFilter : IAsyncActionFilter
	{
		#region Fields

		private const string _modelErrorKey = "Recaptcha";

		#endregion

		#region Constructors

		public ValidateRecaptchaTokenFilter(IModelMetadataProvider modelMetadataProvider, IRecaptchaRequestFactory requestFactory, IRecaptchaSettings settings, IRecaptchaValidator validator)
		{
			this.ModelMetadataProvider = modelMetadataProvider ?? throw new ArgumentNullException(nameof(modelMetadataProvider));
			this.RequestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		#endregion

		#region Properties

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

			return context.HttpContext.Request.Path;
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

			var request = this.RequestFactory.Create(this.GetAction(context));

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