using System;
using System.Collections.Generic;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaValidationResult
	{
		#region Properties

		string Action { get; }
		IEnumerable<string> Errors { get; }
		string Host { get; }
		IDictionary<string, object> Properties { get; }
		decimal? Score { get; }
		bool Success { get; }
		DateTime? Timestamp { get; }

		#endregion
	}
}