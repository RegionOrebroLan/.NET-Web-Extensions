using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using RegionOrebroLan.Web.Security.Captcha.Configuration;
using RegionOrebroLan.Web.Security.Captcha.Configuration.Extensions;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public class RecaptchaValidator : IRecaptchaValidator
	{
		#region Constructors

		public RecaptchaValidator(IOptionsMonitor<RecaptchaOptions> optionsMonitor, ISystemClock systemClock, IRecaptchaValidationClient validationClient)
		{
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
			this.SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
			this.ValidationClient = validationClient ?? throw new ArgumentNullException(nameof(validationClient));
		}

		#endregion

		#region Properties

		protected internal virtual bool Enabled => this.OptionsMonitor.CurrentValue.EnabledOnServer();
		protected internal virtual IOptionsMonitor<RecaptchaOptions> OptionsMonitor { get; }
		protected internal virtual ISystemClock SystemClock { get; }
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
			var options = this.OptionsMonitor.CurrentValue;

			if(!string.Equals(request.Action, validationResult.Action, StringComparison.Ordinal))
				errors.Add("The action is invalid.");

			if(!string.Equals(request.Host, validationResult.Host, StringComparison.Ordinal))
				errors.Add("The host is invalid.");

			if(validationResult.Score == null)
				errors.Add("The score is null.");

			if(validationResult.Score < options.MinimumScore)
				errors.Add("The score is below minimum.");

			if(validationResult.Timestamp == null)
				errors.Add("The timestamp is null.");

			var timeElapsed = this.SystemClock.UtcNow - validationResult.Timestamp;

			if(timeElapsed > options.MaximumTimestampElapse)
				errors.Add("The timestamp has expired. Too much time elapsed.");

			if(timeElapsed < options.MinimumTimestampElapse)
				errors.Add("The timestamp is not valid. Not enough time elapsed.");

			return errors;
		}

		public virtual async Task ValidateAsync(IRecaptchaRequest request)
		{
			if(request == null)
				throw new ArgumentNullException(nameof(request));

			if(!this.Enabled)
				return;

			var validateIp = this.OptionsMonitor.CurrentValue.ValidateIp;

			var ip = validateIp ? request.Ip?.ToString() ?? string.Empty : null;

			var argument = string.Format(CultureInfo.InvariantCulture, "token \"{0}\"", request.Token);

			if(validateIp)
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

			var errors = (!validationResult.Success ? validationResult.Errors.Any() ? validationResult.Errors : new[] { "unsuccessful" } : this.GetValidationErrors(request, validationResult)).ToArray();

			if(errors.Any())
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Recaptcha-validation for {0} failed. Errors: {1}", argument, string.Join(", ", errors)));
		}

		#endregion
	}
}