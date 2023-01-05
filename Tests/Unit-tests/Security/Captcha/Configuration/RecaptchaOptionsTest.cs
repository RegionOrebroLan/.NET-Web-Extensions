using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha.Configuration;

namespace UnitTests.Security.Captcha.Configuration
{
	[TestClass]
	public class RecaptchaOptionsTest
	{
		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Usage", "CA1806:Do not ignore method results")]
		public async Task ClientScriptUrlFormat_Set_IfTheValueIsARelativeUrl_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			var url = new Uri("/Path/", UriKind.Relative);
			Assert.AreEqual("/Path/", url.OriginalString);

			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new RecaptchaOptions { ClientScriptUrlFormat = url };
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(string.Equals(argumentException.ParamName, "value", StringComparison.Ordinal) && argumentException.Message.StartsWith("The url must be absolute.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public async Task ClientScriptUrlFormat_Set_IfTheValueIsNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			var recaptchaOptions = new RecaptchaOptions { ClientScriptUrlFormat = null };
			Assert.IsNull(recaptchaOptions.ClientScriptUrlFormat);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Usage", "CA1806:Do not ignore method results")]
		public async Task ValidationUrl_Set_IfTheValueIsARelativeUrl_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;

			var url = new Uri("/Path/", UriKind.Relative);
			Assert.AreEqual("/Path/", url.OriginalString);

			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new RecaptchaOptions { ValidationUrl = url };
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(string.Equals(argumentException.ParamName, "value", StringComparison.Ordinal) && argumentException.Message.StartsWith("The url must be absolute.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public async Task ValidationUrl_Set_IfTheValueIsNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			var recaptchaOptions = new RecaptchaOptions { ValidationUrl = null };
			Assert.IsNull(recaptchaOptions.ValidationUrl);
		}

		#endregion
	}
}