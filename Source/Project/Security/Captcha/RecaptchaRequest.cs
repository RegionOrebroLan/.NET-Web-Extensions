using System.Net;

namespace RegionOrebroLan.Web.Security.Captcha
{
	/// <inheritdoc />
	public class RecaptchaRequest : IRecaptchaRequest
	{
		#region Properties

		public virtual string Action { get; set; }
		public virtual string Host { get; set; }
		public virtual IPAddress Ip { get; set; }
		public virtual string Token { get; set; }

		#endregion
	}
}