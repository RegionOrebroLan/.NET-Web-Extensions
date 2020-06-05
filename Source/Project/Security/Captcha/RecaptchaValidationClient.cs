using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RegionOrebroLan.DependencyInjection;

namespace RegionOrebroLan.Web.Security.Captcha
{
	[ServiceConfiguration(ServiceType = typeof(IRecaptchaValidationClient))]
	public class RecaptchaValidationClient : IRecaptchaValidationClient
	{
		#region Constructors

		public RecaptchaValidationClient(IRecaptchaSettings settings)
		{
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		#endregion

		#region Properties

		protected internal virtual IRecaptchaSettings Settings { get; }

		#endregion

		#region Methods

		protected internal virtual HttpContent CreateValidationResultContent(string ip, string token)
		{
			return new FormUrlEncodedContent(this.CreateValidationResultParameters(ip, token));
		}

		protected internal virtual IDictionary<string, string> CreateValidationResultParameters(string ip, string token)
		{
			var parameters = new Dictionary<string, string>
			{
				{"response", token},
				{"secret", this.Settings.SecretKey}
			};

			if(ip != null)
				parameters.Add("remoteip", ip);

			return parameters;
		}

		public virtual async Task<IRecaptchaValidationResult> GetValidationResultAsync(string ip, string token)
		{
			using(var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsync(this.Settings.ValidationUrl, this.CreateValidationResultContent(ip, token)).ConfigureAwait(false);

				// ReSharper disable InvertIf
				if(response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					return this.ParseValidationResult(content);
				}
				// ReSharper restore InvertIf

				throw new InvalidOperationException(this.GetValidationResultExceptionMessage(ip, response.StatusCode, token));
			}
		}

		protected internal virtual string GetValidationResultExceptionMessage(string ip, HttpStatusCode statusCode, string token)
		{
			var format = string.Format(CultureInfo.InvariantCulture, "Could not get a validation-result for {0}. Response-status-code: {1}.", "{0}", statusCode);

			var argument = string.Format(CultureInfo.InvariantCulture, "token \"{0}\"", token);

			if(ip != null)
				argument = string.Format(CultureInfo.InvariantCulture, "ip \"{0}\" and {1}", ip, argument);

			return string.Format(CultureInfo.InvariantCulture, format, argument);
		}

		protected internal virtual IRecaptchaValidationResult ParseValidationResult(string content)
		{
			return JsonConvert.DeserializeObject<RecaptchaValidationResult>(content);
		}

		#endregion
	}
}