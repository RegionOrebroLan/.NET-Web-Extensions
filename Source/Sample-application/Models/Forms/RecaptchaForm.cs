using System.ComponentModel.DataAnnotations;

namespace SampleApplication.Models.Forms
{
	public class RecaptchaForm
	{
		#region Properties

		[Display(Name = "Test-value")]
		[Required]
		public virtual string Value { get; set; }

		#endregion
	}
}