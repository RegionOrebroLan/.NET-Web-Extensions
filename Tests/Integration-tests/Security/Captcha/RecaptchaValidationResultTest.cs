using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

			const string value = "{\"action\":\"Action\",\"error-codes\":[\"Error-code-1\",\"Error-code-2\"],\"hostname\":\"host.com\",\"success\":true,\"First-property\":\"First-property-value\",\"Second-property\":\"Second-property-value\"}";

			var recaptchaValidationResult = JsonSerializer.Deserialize<RecaptchaValidationResult>(value, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

			Assert.IsNotNull(recaptchaValidationResult);
			Assert.AreEqual("Action", recaptchaValidationResult.Action);
			Assert.AreEqual(2, recaptchaValidationResult.Errors.Count);
			Assert.AreEqual("Error-code-1", recaptchaValidationResult.Errors.ElementAt(0));
			Assert.AreEqual("Error-code-2", recaptchaValidationResult.Errors.ElementAt(1));
			Assert.AreEqual("host.com", recaptchaValidationResult.Host);
			Assert.IsTrue(recaptchaValidationResult.Success);
			Assert.AreEqual(2, recaptchaValidationResult.Properties.Count);
			Assert.AreEqual("First-property", recaptchaValidationResult.Properties.ElementAt(0).Key);
			Assert.AreEqual("First-property-value", ((JsonElement)recaptchaValidationResult.Properties.ElementAt(0).Value).GetString());
			Assert.AreEqual(JsonValueKind.String, ((JsonElement)recaptchaValidationResult.Properties.ElementAt(0).Value).ValueKind);
			Assert.AreEqual("Second-property", recaptchaValidationResult.Properties.ElementAt(1).Key);
			Assert.AreEqual("Second-property-value", ((JsonElement)recaptchaValidationResult.Properties.ElementAt(1).Value).GetString());
			Assert.AreEqual(JsonValueKind.String, ((JsonElement)recaptchaValidationResult.Properties.ElementAt(1).Value).ValueKind);
		}

		[TestMethod]
		public async Task JsonSerialize_Test()
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

			var value = JsonSerializer.Serialize(recaptchaValidationResult, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

			Assert.AreEqual("{\"action\":\"Action\",\"error-codes\":[\"Error-code-1\",\"Error-code-2\"],\"hostname\":\"host.com\",\"success\":false,\"First-property\":\"First-property-value\",\"Second-property\":\"Second-property-value\"}", value);
		}

		#endregion
	}
}