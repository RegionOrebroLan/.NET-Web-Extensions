using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RegionOrebroLan.ServiceLocation;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace RegionOrebroLan.Web.Security.Captcha
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Singleton, ServiceType = typeof(IRecaptchaValidator))]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
	public class RecaptchaValidator : IRecaptchaValidator
	{
		#region Constructors

		public RecaptchaValidator(IDateTimeContext dateTimeContext, IRecaptchaSettings settings, IRecaptchaValidationClient validationClient)
		{
			this.DateTimeContext = dateTimeContext ?? throw new ArgumentNullException(nameof(dateTimeContext));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this.ValidationClient = validationClient ?? throw new ArgumentNullException(nameof(validationClient));
		}

		#endregion

		#region Properties

		protected internal virtual IDateTimeContext DateTimeContext { get; }
		protected internal virtual bool Enabled => this.Settings.EnabledOnServer();
		protected internal virtual IRecaptchaSettings Settings { get; }
		protected internal virtual IRecaptchaValidationClient ValidationClient { get; }

		#endregion

		#region Methods

		protected internal virtual IEnumerable<string> GetValidationErrors(IRecaptchaRequest request, IRecaptchaValidationResult validationResult)
		{
			if(request == null)
				throw new ArgumentNullException(nameof(request));

			if(validationResult == null)
				throw new ArgumentNullException(nameof(validationResult));

			var errors = new List<string>();

			if(!string.Equals(request.Action, validationResult.Action, StringComparison.Ordinal))
				errors.Add("The action is invalid.");

			if(!string.Equals(request.Host, validationResult.Host, StringComparison.Ordinal))
				errors.Add("The host is invalid.");

			if(validationResult.Score < this.Settings.MinimumScore)
				errors.Add("The score is below minimum.");

			if(validationResult.Timestamp == null)
				errors.Add("The timestamp is null.");

			var timeElapsed = this.DateTimeContext.UtcNow - validationResult.Timestamp;

			if(timeElapsed > this.Settings.MaximumTimestampElapse)
				errors.Add("The timestamp has expired. Too much time elapsed.");

			if(timeElapsed < this.Settings.MinimumTimestampElapse)
				errors.Add("The timestamp is not valid. Not enough time elapsed.");

			return errors;
		}

		public virtual async Task ValidateAsync(IRecaptchaRequest request)
		{
			if(request == null)
				throw new ArgumentNullException(nameof(request));

			if(!this.Enabled)
				return;

			var ip = this.Settings.ValidateIp ? request.Ip?.ToString() ?? string.Empty : null;

			var argument = string.Format(CultureInfo.InvariantCulture, "token \"{0}\"", request.Token);

			if(this.Settings.ValidateIp)
				argument = string.Format(CultureInfo.InvariantCulture, "ip \"{0}\" and {1}", ip, argument);

			IRecaptchaValidationResult validationResult;

			try
			{
				validationResult = await this.ValidationClient.GetValidationResultAsync(ip, request.Token).ConfigureAwait(false);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not validate recaptcha for {0}.", argument), exception);
			}

			if(validationResult == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not validate recaptcha for {0}. The validation-result is null.", argument));

			var errors = (!validationResult.Success ? validationResult.Errors.Any() ? validationResult.Errors : new[] {"unsuccessful"} : this.GetValidationErrors(request, validationResult)).ToArray();

			if(errors.Any())
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Recaptcha-validation for {0} failed. Errors: {1}", argument, string.Join(", ", errors)));
		}

		#endregion
	}
}