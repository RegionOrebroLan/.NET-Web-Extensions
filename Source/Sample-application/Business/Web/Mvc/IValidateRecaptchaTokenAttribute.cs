using System.Diagnostics.CodeAnalysis;

namespace SampleApplication.Business.Web.Mvc
{
	[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
	public interface IValidateRecaptchaTokenAttribute
	{
		#region Properties

		string Action { get; }

		#endregion
	}
}