using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha;

namespace RegionOrebroLan.Web.IntegrationTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidationClientTest
	{
		#region Methods

		protected internal virtual IRecaptchaSettings CreateDefaultSettings()
		{
			return new RecaptchaSettings
			{
				ClientScriptUrlFormat = Global.RecaptchaSettings.ClientScriptUrlFormat,
				Mode = Global.RecaptchaSettings.Mode,
				SecretKey = Global.RecaptchaSettings.SecretKey,
				SiteKey = Global.RecaptchaSettings.SiteKey,
				TimestampExpiration = Global.RecaptchaSettings.TimestampExpiration,
				ValidationUrl = Global.RecaptchaSettings.ValidationUrl
			};
		}

		protected internal virtual RecaptchaValidationClient CreateValidationClient()
		{
			return this.CreateValidationClient(this.CreateDefaultSettings());
		}

		protected internal virtual RecaptchaValidationClient CreateValidationClient(IRecaptchaSettings settings)
		{
			return new RecaptchaValidationClient(settings);
		}

		[TestMethod]
		public void GetValidationResultAsync_Test()
		{
			var validationClient = this.CreateValidationClient();

			var asynchronousResult = validationClient.GetValidationResultAsync(null, "Test");

			var result = asynchronousResult.Result;

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Success);
			Assert.AreEqual(1, result.Errors.Count());
			Assert.AreEqual("invalid-input-response", result.Errors.First());
		}

		#endregion
	}
}