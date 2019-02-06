using System.Net;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaRequest
	{
		#region Properties

		string Action { get; }
		string Host { get; }

		/// <summary>
		/// The user's IP address.
		/// </summary>
		IPAddress Ip { get; }

		string Token { get; }

		#endregion
	}
}