using Application.Models.Forms;

namespace Application.Models.ViewModels
{
	public class RecaptchaViewModel : ViewModel
	{
		#region Properties

		public virtual RecaptchaForm Form { get; set; } = new RecaptchaForm();
		public virtual bool Saved { get; set; }

		#endregion
	}
}