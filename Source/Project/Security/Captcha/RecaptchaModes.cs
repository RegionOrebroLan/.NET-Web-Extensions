using System;

namespace RegionOrebroLan.Web.Security.Captcha
{
	[Flags]
	public enum RecaptchaModes
	{
		None = 0,
		EnabledOnClient = 1,
		EnabledOnServer = 2
	}
}