using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha;

namespace UnitTests.Security.Captcha
{
	[TestClass]
	public class RecaptchaClientActionResolverTest
	{
		#region Fields

		private static readonly RecaptchaClientActionResolver _recaptchaClientActionResolver = new RecaptchaClientActionResolver();

		#endregion

		#region Properties

		protected internal virtual RecaptchaClientActionResolver RecaptchaClientActionResolver => _recaptchaClientActionResolver;

		#endregion

		#region Methods

		[TestMethod]
		public void Resolve_IfTheActionParameterIsAnEmptyString_ShouldReturnAnEmptyString()
		{
			Assert.AreEqual(string.Empty, this.RecaptchaClientActionResolver.Resolve(string.Empty));
		}

		[TestMethod]
		public void Resolve_IfTheActionParameterIsLongerThanHundredCharacters_ShouldReturnTheFirstHundredCharacters()
		{
			const string action = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_123456789!\"#¤%&/()=?@£$€{[]}\\^~*'abcdefghijklmn";
			const string expected = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ__________________________________abcdefghijklmn";
			Assert.AreEqual(100, action.Length);
			Assert.AreEqual(100, expected.Length);
			Assert.AreEqual(expected, this.RecaptchaClientActionResolver.Resolve(action));
		}

		[TestMethod]
		public void Resolve_IfTheActionParameterIsNull_ShouldReturnNull()
		{
			Assert.IsNull(this.RecaptchaClientActionResolver.Resolve(null));
		}

		[TestMethod]
		public void Resolve_IfTheActionParameterOnlyContainsInvalidCharacters_ShouldReturnAValueWhereTheInvalidCharactersAreReplacedWithAnUnderscore()
		{
			const string action = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_123456789!\"#¤%&/()=?@£$€{[]}\\^~*'";
			const string expected = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ__________________________________";
			Assert.AreEqual(expected, this.RecaptchaClientActionResolver.Resolve(action));
		}

		[TestMethod]
		public void Resolve_IfTheActionParameterOnlyContainsValidCharacters_ShouldReturnAnUnchangedValue()
		{
			const string action = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
			Assert.AreEqual(action, this.RecaptchaClientActionResolver.Resolve(action));
		}

		#endregion
	}
}