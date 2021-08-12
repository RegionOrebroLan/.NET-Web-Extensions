using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RegionOrebroLan.Web.Security.Captcha;

namespace IntegrationTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidationResultTest
	{
		#region Methods

		[TestMethod]
		public async Task JsonDeserialize_Test()
		{
			await Task.CompletedTask;

			var recaptchaValidationResult = new RecaptchaValidationResult
			{
				Action = "Action",
				Errors = new[] { "Error-code-1", "Error-code-2" },
				Host = "host.com"
			};

			recaptchaValidationResult.Properties.Add("First-property", "First-property-value");
			recaptchaValidationResult.Properties.Add("Second-property", "Second-property-value");

			var value = JsonConvert.SerializeObject(recaptchaValidationResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

			Assert.AreEqual("{\"action\":\"Action\",\"error-codes\":[\"Error-code-1\",\"Error-code-2\"],\"hostname\":\"host.com\",\"success\":false,\"First-property\":\"First-property-value\",\"Second-property\":\"Second-property-value\"}", value);
		}

		[TestMethod]
		public async Task JsonSerialize_Test()
		{
			await Task.CompletedTask;

			const string value = "{\"action\":\"Action\",\"error-codes\":[\"Error-code-1\",\"Error-code-2\"],\"hostname\":\"host.com\",\"success\":true,\"First-property\":\"First-property-value\",\"Second-property\":\"Second-property-value\"}";

			var recaptchaValidationResult = JsonConvert.DeserializeObject<RecaptchaValidationResult>(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

			Assert.AreEqual("Action", recaptchaValidationResult.Action);
			Assert.AreEqual(2, recaptchaValidationResult.Errors.Count());
			Assert.AreEqual("Error-code-1", recaptchaValidationResult.Errors.ElementAt(0));
			Assert.AreEqual("Error-code-2", recaptchaValidationResult.Errors.ElementAt(1));
			Assert.AreEqual("host.com", recaptchaValidationResult.Host);
			Assert.IsTrue(recaptchaValidationResult.Success);
			Assert.AreEqual(2, recaptchaValidationResult.Properties.Count);
			Assert.AreEqual("First-property", recaptchaValidationResult.Properties.ElementAt(0).Key);
			Assert.AreEqual("First-property-value", recaptchaValidationResult.Properties.ElementAt(0).Value);
			Assert.AreEqual("Second-property", recaptchaValidationResult.Properties.ElementAt(1).Key);
			Assert.AreEqual("Second-property-value", recaptchaValidationResult.Properties.ElementAt(1).Value);
		}

		#endregion
	}
}