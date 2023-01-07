using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.DependencyInjection.Extensions;

namespace IntegrationTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidationClientTest
	{
		#region Methods

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync()
		{
			var configuration = Global.CreateDefaultConfigurationBuilder().Build();

			var services = Global.CreateServices(configuration);
			services.AddRecaptcha(configuration);

			return await Task.FromResult(services.BuildServiceProvider());
		}

		[TestMethod]
		public async Task GetValidationResultAsync_Test()
		{
			using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var validationClient = serviceProvider.GetRequiredService<IRecaptchaValidationClient>();

				var result = await validationClient.GetValidationResultAsync(null, "Test");

				Assert.IsNotNull(result);
				Assert.IsFalse(result.Success);
				Assert.AreEqual(1, result.Errors.Count);
				Assert.AreEqual("invalid-input-response", result.Errors.First());
			}
		}

		#endregion
	}
}