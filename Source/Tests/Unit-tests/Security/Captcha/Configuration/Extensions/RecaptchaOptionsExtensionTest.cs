using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha.Configuration;
using RegionOrebroLan.Web.Security.Captcha.Configuration.Extensions;

namespace UnitTests.Security.Captcha.Configuration.Extensions
{
	[TestClass]
	public class RecaptchaOptionsExtensionTest
	{
		#region Methods

		[TestMethod]
		public async Task ClientScriptUrl_IfTheClientScriptUrlFormatPropertyOfTheRecaptchaOptionsParameterContainsAFormatPlaceholder_ShouldReturnAFormattedResult()
		{
			await Task.CompletedTask;

			var recaptchaOptions = new RecaptchaOptions
			{
				ClientScriptUrlFormat = new Uri("http://localhost/?Key={0}"),
				SiteKey = "Site-key"
			};

			Assert.AreEqual(new Uri("http://localhost/?Key=Site-key"), recaptchaOptions.ClientScriptUrl());
		}

		[TestMethod]
		public async Task ClientScriptUrl_IfTheClientScriptUrlFormatPropertyOfTheRecaptchaOptionsParameterIsNull_ShouldReturnNull()
		{
			await Task.CompletedTask;

			var recaptchaOptions = new RecaptchaOptions
			{
				ClientScriptUrlFormat = null,
				SiteKey = "Site-key"
			};

			Assert.IsNull(recaptchaOptions.ClientScriptUrl());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ClientScriptUrl_IfTheRecaptchaOptionsParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			try
			{
				((RecaptchaOptions)null).ClientScriptUrl();
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(string.Equals(argumentNullException.ParamName, "options", StringComparison.Ordinal))
					throw;
			}
		}

		#endregion
	}
}