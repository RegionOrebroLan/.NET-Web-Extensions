using System.Threading.Tasks;

namespace RegionOrebroLan.Web.Security.Captcha
{
	public interface IRecaptchaValidator
	{
		#region Methods

		Task ValidateAsync(IRecaptchaRequest request);

		#endregion
	}
}