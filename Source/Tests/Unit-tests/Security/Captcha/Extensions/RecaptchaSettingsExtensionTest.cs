using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Extensions;

namespace UnitTests.Security.Captcha.Extensions
{
	[TestClass]
	public class RecaptchaSettingsExtensionTest
	{
		#region Methods

		[TestMethod]
		public void ClientScriptUrl_IfTheClientScriptUrlFormatPropertyOfTheRecaptchaSettingsParameterContainsAFormatPlaceholder_ShouldReturnAFormattedResult()
		{
			var recaptchaSettingsMock = new Mock<IRecaptchaSettings>();
			recaptchaSettingsMock.Setup(recaptchaSettings => recaptchaSettings.ClientScriptUrlFormat).Returns(new Uri("http://localhost/?Key={0}"));
			recaptchaSettingsMock.Setup(recaptchaSettings => recaptchaSettings.SiteKey).Returns("Site-key");
			Assert.AreEqual(new Uri("http://localhost/?Key=Site-key"), recaptchaSettingsMock.Object.ClientScriptUrl());
		}

		[TestMethod]
		public void ClientScriptUrl_IfTheClientScriptUrlFormatPropertyOfTheRecaptchaSettingsParameterIsNull_ShouldReturnNull()
		{
			var recaptchaSettingsMock = new Mock<IRecaptchaSettings>();
			recaptchaSettingsMock.Setup(recaptchaSettings => recaptchaSettings.ClientScriptUrlFormat).Returns((Uri)null);
			recaptchaSettingsMock.Setup(recaptchaSettings => recaptchaSettings.SiteKey).Returns("Site-key");
			Assert.IsNull(recaptchaSettingsMock.Object.ClientScriptUrl());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ClientScriptUrl_IfTheRecaptchaSettingsParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				((IRecaptchaSettings)null).ClientScriptUrl();
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName.Equals("recaptchaSettings", StringComparison.Ordinal))
					throw;
			}
		}

		#endregion
	}
}