using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public class RecaptchaValidationResult : IRecaptchaValidationResult
	{
		#region Properties

		[JsonProperty("action", Order = 0)]
		public virtual string Action { get; set; }

		[JsonProperty("error-codes", Order = 1)]
		public virtual IEnumerable<string> Errors { get; set; }

		[JsonProperty("hostname", Order = 2)]
		public virtual string Host { get; set; }

		[JsonExtensionData]
		public virtual IDictionary<string, object> Properties { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		[JsonProperty("score", Order = 3)]
		public virtual decimal? Score { get; set; }

		[JsonProperty("success", Order = 4)]
		public virtual bool Success { get; set; }

		[JsonProperty("challenge_ts", Order = 5)]
		public virtual DateTimeOffset? Timestamp { get; set; }

		#endregion
	}
}