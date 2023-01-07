using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public class RecaptchaValidationResult : IRecaptchaValidationResult
	{
		#region Fields

		private IList<string> _errors;
		private IDictionary<string, object> _properties;

		#endregion

		#region Properties

		[JsonPropertyName("action")]
		public virtual string Action { get; set; }

		[JsonPropertyName("error-codes")]
		public virtual IList<string> Errors
		{
			get => this._errors ??= new List<string>();
			set => this._errors = value;
		}

		[JsonPropertyName("hostname")]
		public virtual string Host { get; set; }

		[JsonExtensionData]
		public virtual IDictionary<string, object> Properties
		{
			get => this._properties ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			set => this._properties = value;
		}

		[JsonPropertyName("score")]
		public virtual decimal? Score { get; set; }

		[JsonPropertyName("success")]
		public virtual bool Success { get; set; }

		[JsonPropertyName("challenge_ts")]
		public virtual DateTimeOffset? Timestamp { get; set; }

		#endregion
	}
}