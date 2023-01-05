using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.Web.Security.Captcha;
using RegionOrebroLan.Web.Security.Captcha.Configuration;
using UnitTests.Mocks;

namespace UnitTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidatorTest
	{
		#region Methods

		protected internal virtual async Task<IOptionsMonitor<RecaptchaOptions>> CreateOptionsMonitorAsync(RecaptchaModes mode)
		{
			var options = await this.CreateResetOptionsAsync();
			options.Mode = mode;

			return await this.CreateOptionsMonitorAsync(options);
		}

		protected internal virtual async Task<IOptionsMonitor<RecaptchaOptions>> CreateOptionsMonitorAsync(RecaptchaOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var optionsMonitorMock = new Mock<IOptionsMonitor<RecaptchaOptions>>();
			optionsMonitorMock.Setup(optionsMonitor => optionsMonitor.CurrentValue).Returns(options);

			return await Task.FromResult(optionsMonitorMock.Object);
		}

		protected internal virtual async Task<RecaptchaOptions> CreateResetOptionsAsync()
		{
			return await Task.FromResult(new RecaptchaOptions
			{
				ClientScriptUrlFormat = default,
				MaximumTimestampElapse = default,
				MinimumScore = default,
				MinimumTimestampElapse = default,
				Mode = default,
				SiteKey = default,
				SecretKey = default,
				TokenParameterName = default,
				ValidateIp = default,
				ValidationUrl = default
			});
		}

		protected internal virtual async Task<ISystemClock> CreateSystemClockAsync(DateTimeOffset utcNow)
		{
			var systemClockMock = new Mock<ISystemClock>();
			systemClockMock.Setup(systemClock => systemClock.UtcNow).Returns(utcNow);

			return await Task.FromResult(systemClockMock.Object);
		}

		protected internal virtual async Task<RecaptchaValidator> CreateValidatorAsync(RecaptchaOptions options, ISystemClock systemClock, IRecaptchaValidationClient validationClient = null)
		{
			var optionsMonitor = await this.CreateOptionsMonitorAsync(options);

			return new RecaptchaValidator(optionsMonitor, systemClock, validationClient ?? Mock.Of<IRecaptchaValidationClient>());
		}

		[TestMethod]
		public async Task GetValidationErrors_IfTheResultScoreIsNull_ShouldReturnErrors()
		{
			var options = new RecaptchaOptions();
			var systemClock = new SystemClockMock();
			var validator = await this.CreateValidatorAsync(options, systemClock);

			var request = new RecaptchaRequest();
			var result = new RecaptchaValidationResult();

			var errors = validator.GetValidationErrors(request, result).ToArray();

			Assert.AreEqual(2, errors.Length);
			Assert.AreEqual("The score is null.", errors.First());
		}

		[TestMethod]
		public async Task GetValidationErrors_IfTheResultTimestampIsNull_ShouldReturnErrors()
		{
			var options = new RecaptchaOptions();
			var systemClock = new SystemClockMock();
			var validator = await this.CreateValidatorAsync(options, systemClock);

			var request = new RecaptchaRequest();
			var result = new RecaptchaValidationResult();

			var errors = validator.GetValidationErrors(request, result).ToArray();

			Assert.AreEqual(2, errors.Length);
			Assert.AreEqual("The timestamp is null.", errors.ElementAt(1));
		}

		[TestMethod]
		public async Task GetValidationErrors_IfTooLittleTimeElapsed_ShouldReturnErrors()
		{
			var utcNow = DateTimeOffset.UtcNow;

			var options = new RecaptchaOptions();
			var systemClock = new SystemClockMock
			{
				UtcNow = utcNow
			};
			var validator = await this.CreateValidatorAsync(options, systemClock);

			var request = new RecaptchaRequest();
			var result = new RecaptchaValidationResult
			{
				Timestamp = utcNow
			};

			var errors = validator.GetValidationErrors(request, result).ToArray();

			Assert.AreEqual(2, errors.Length);
			Assert.AreEqual("The timestamp is not valid. Not enough time elapsed.", errors.ElementAt(1));
		}

		[TestMethod]
		public async Task GetValidationErrors_IfTooMuchTimeElapsed_ShouldReturnErrors()
		{
			var utcNow = DateTimeOffset.UtcNow;

			var options = new RecaptchaOptions();
			var systemClock = new SystemClockMock
			{
				UtcNow = utcNow
			};
			var validator = await this.CreateValidatorAsync(options, systemClock);

			var request = new RecaptchaRequest();
			var result = new RecaptchaValidationResult
			{
				Timestamp = utcNow.AddHours(-5)
			};

			var errors = validator.GetValidationErrors(request, result).ToArray();

			Assert.AreEqual(2, errors.Length);
			Assert.AreEqual("The timestamp has expired. Too much time elapsed.", errors.ElementAt(1));
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
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Score).Returns(0);
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(true);
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Timestamp).Returns(utcNow);

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
			var options = await this.CreateResetOptionsAsync();
			options.Mode = RecaptchaModes.EnabledOnServer;

			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
			{
				var recaptchaValidationResultMock = new Mock<IRecaptchaValidationResult>();
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(false);

				return Task.FromResult(recaptchaValidationResultMock.Object);
			});

			var validator = await this.CreateValidatorAsync(options, Mock.Of<ISystemClock>(), recaptchaValidationClientMock.Object);

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