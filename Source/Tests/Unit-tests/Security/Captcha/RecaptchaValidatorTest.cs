using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Configuration;

namespace UnitTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidatorTest
	{
		#region Methods

		protected internal virtual async Task<IOptionsMonitor<RecaptchaOptions>> CreateOptionsMonitorAsync(RecaptchaModes mode)
		{
			var options = new RecaptchaOptions
			{
				ClientScriptUrlFormat = default,
				MaximumTimestampElapse = default,
				MinimumScore = default,
				MinimumTimestampElapse = default,
				Mode = mode,
				SiteKey = default,
				SecretKey = default,
				TokenParameterName = default,
				ValidateIp = default,
				ValidationUrl = default
			};
			var optionsMonitorMock = new Mock<IOptionsMonitor<RecaptchaOptions>>();
			optionsMonitorMock.Setup(optionsMonitor => optionsMonitor.CurrentValue).Returns(options);
			return await Task.FromResult(optionsMonitorMock.Object);
		}

		protected internal virtual async Task<ISystemClock> CreateSystemClockAsync()
		{
			return await Task.FromResult(new SystemClock());
		}

		protected internal virtual async Task<ISystemClock> CreateSystemClockAsync(DateTimeOffset utcNow)
		{
			var systemClockMock = new Mock<ISystemClock>();
			systemClockMock.Setup(systemClock => systemClock.UtcNow).Returns(utcNow);
			return await Task.FromResult(systemClockMock.Object);
		}

		[TestMethod]
		public async Task ValidateAsync_IfSettingsModeIsEnabledOnClient_ShouldNotCallTheClient()
		{
			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(await this.CreateOptionsMonitorAsync(RecaptchaModes.EnabledOnClient), Mock.Of<ISystemClock>(), recaptchaValidationClientMock.Object);

			await validator.ValidateAsync(Mock.Of<IRecaptchaRequest>());

			Assert.IsFalse(called);
		}

		[TestMethod]
		public async Task ValidateAsync_IfSettingsModeIsEnabledOnServer_ShouldCallTheClient()
		{
			var utcNow = DateTimeOffset.UtcNow;

			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
			{
				var recaptchaValidationResultMock = new Mock<IRecaptchaValidationResult>();
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(true);
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Timestamp).Returns(utcNow.UtcDateTime);

				return Task.FromResult(recaptchaValidationResultMock.Object);
			}).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(await this.CreateOptionsMonitorAsync(RecaptchaModes.EnabledOnServer), await this.CreateSystemClockAsync(utcNow), recaptchaValidationClientMock.Object);

			await validator.ValidateAsync(Mock.Of<IRecaptchaRequest>());

			Assert.IsTrue(called);
		}

		[TestMethod]
		public async Task ValidateAsync_IfSettingsModeIsNone_ShouldNotCallTheClient()
		{
			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(await this.CreateOptionsMonitorAsync(RecaptchaModes.None), Mock.Of<ISystemClock>(), recaptchaValidationClientMock.Object);

			await validator.ValidateAsync(Mock.Of<IRecaptchaRequest>());

			Assert.IsFalse(called);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public async Task ValidateAsync_IfTheValidationResultFromTheClientIsNotSuccessful_ShouldThrowAnInvalidOperationException()
		{
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
			{
				var recaptchaValidationResultMock = new Mock<IRecaptchaValidationResult>();
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(false);

				return Task.FromResult(recaptchaValidationResultMock.Object);
			});

			var validator = new RecaptchaValidator(await this.CreateOptionsMonitorAsync(RecaptchaModes.EnabledOnServer), Mock.Of<ISystemClock>(), recaptchaValidationClientMock.Object);

			try
			{
				await validator.ValidateAsync(Mock.Of<IRecaptchaRequest>());
			}
			catch(AggregateException aggregateException)
			{
				if(aggregateException.InnerExceptions.FirstOrDefault() is InvalidOperationException invalidOperationException && invalidOperationException.Message.Equals("Recaptcha-validation for token \"\" failed. Errors: unsuccessful", StringComparison.Ordinal))
					throw invalidOperationException;
			}
		}

		#endregion
	}
}