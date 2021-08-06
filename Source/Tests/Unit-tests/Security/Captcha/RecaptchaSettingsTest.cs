using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha;

namespace UnitTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaSettingsTest
	{
		#region Methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Usage", "CA1806:Do not ignore method results")]
		public void ClientScriptUrlFormat_Set_IfTheValueIsARelativeUrl_ShouldThrowAnArgumentException()
		{
			var url = new Uri("/Path/", UriKind.Relative);
			Assert.AreEqual("/Path/", url.OriginalString);

			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new RecaptchaSettings { ClientScriptUrlFormat = url };
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("value", StringComparison.Ordinal) && argumentException.Message.StartsWith("The url must be absolute.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public void ClientScriptUrlFormat_Set_IfTheValueIsNull_ShouldNotThrowAnException()
		{
			var recaptchaSettings = new RecaptchaSettings { ClientScriptUrlFormat = null };
			Assert.IsNull(recaptchaSettings.ClientScriptUrlFormat);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Usage", "CA1806:Do not ignore method results")]
		public void ValidationUrl_Set_IfTheValueIsARelativeUrl_ShouldThrowAnArgumentException()
		{
			var url = new Uri("/Path/", UriKind.Relative);
			Assert.AreEqual("/Path/", url.OriginalString);

			try
			{
				// ReSharper disable ObjectCreationAsStatement
				new RecaptchaSettings { ValidationUrl = url };
				// ReSharper restore ObjectCreationAsStatement
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.ParamName.Equals("value", StringComparison.Ordinal) && argumentException.Message.StartsWith("The url must be absolute.", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public void ValidationUrl_Set_IfTheValueIsNull_ShouldNotThrowAnException()
		{
			var recaptchaSettings = new RecaptchaSettings { ValidationUrl = null };
			Assert.IsNull(recaptchaSettings.ValidationUrl);
		}

		#endregion
	}
}