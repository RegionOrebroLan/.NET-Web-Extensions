using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.Web.Security.Captcha;

namespace RegionOrebroLan.Web.UnitTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaValidatorTest
	{
		#region Methods

		protected internal virtual IDateTimeContext CreateDateTimeContext()
		{
			return this.CreateDateTimeContext(DateTime.UtcNow);
		}

		protected internal virtual IDateTimeContext CreateDateTimeContext(DateTime utcNow)
		{
			var dateTimeContextMock = new Mock<IDateTimeContext>();
			dateTimeContextMock.Setup(dateTimeContext => dateTimeContext.UtcNow).Returns(utcNow);
			return dateTimeContextMock.Object;
		}

		protected internal virtual IRecaptchaSettings CreateSettings(RecaptchaModes mode)
		{
			var settingsMock = new Mock<IRecaptchaSettings>();
			settingsMock.Setup(settings => settings.Mode).Returns(mode);
			return settingsMock.Object;
		}

		[TestMethod]
		public void ValidateAsync_IfSettingsModeIsEnabledOnClient_ShouldNotCallTheClient()
		{
			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(Mock.Of<IDateTimeContext>(), this.CreateSettings(RecaptchaModes.EnabledOnClient), recaptchaValidationClientMock.Object);

			validator.ValidateAsync(Mock.Of<IRecaptchaRequest>()).Wait();

			Assert.IsFalse(called);
		}

		[TestMethod]
		public void ValidateAsync_IfSettingsModeIsEnabledOnServer_ShouldCallTheClient()
		{
			var utcNow = DateTime.UtcNow;

			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
			{
				var recaptchaValidationResultMock = new Mock<IRecaptchaValidationResult>();
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(true);
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Timestamp).Returns(utcNow);

				return Task.FromResult(recaptchaValidationResultMock.Object);
			}).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(this.CreateDateTimeContext(utcNow), this.CreateSettings(RecaptchaModes.EnabledOnServer), recaptchaValidationClientMock.Object);

			validator.ValidateAsync(Mock.Of<IRecaptchaRequest>()).Wait();

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ValidateAsync_IfSettingsModeIsNone_ShouldNotCallTheClient()
		{
			var called = false;
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Callback(() => { called = true; });

			var validator = new RecaptchaValidator(Mock.Of<IDateTimeContext>(), this.CreateSettings(RecaptchaModes.None), recaptchaValidationClientMock.Object);

			validator.ValidateAsync(Mock.Of<IRecaptchaRequest>()).Wait();

			Assert.IsFalse(called);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateAsync_IfTheValidationResultFromTheClientIsNotSuccessful_ShouldThrowAnInvalidOperationException()
		{
			var recaptchaValidationClientMock = new Mock<IRecaptchaValidationClient>();
			recaptchaValidationClientMock.Setup(recaptchaValidationClient => recaptchaValidationClient.GetValidationResultAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() =>
			{
				var recaptchaValidationResultMock = new Mock<IRecaptchaValidationResult>();
				recaptchaValidationResultMock.Setup(recaptchaValidationResult => recaptchaValidationResult.Success).Returns(false);

				return Task.FromResult(recaptchaValidationResultMock.Object);
			});

			var validator = new RecaptchaValidator(Mock.Of<IDateTimeContext>(), this.CreateSettings(RecaptchaModes.EnabledOnServer), recaptchaValidationClientMock.Object);

			try
			{
				validator.ValidateAsync(Mock.Of<IRecaptchaRequest>()).Wait();
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