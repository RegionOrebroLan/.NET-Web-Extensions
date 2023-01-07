using System;
using System.Collections.Generic;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaValidationResult
	{
		#region Properties

		string Action { get; }
		IList<string> Errors { get; }
		string Host { get; }
		IDictionary<string, object> Properties { get; }
		decimal? Score { get; }
		bool Success { get; }
		DateTimeOffset? Timestamp { get; }

		#endregion
	}
}