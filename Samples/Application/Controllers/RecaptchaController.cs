using Application.Models.Forms;
using Application.Models.ViewModels;
using Application.Models.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class RecaptchaController : SiteController
	{
		#region Methods

		protected internal virtual RecaptchaViewModel CreateModel()
		{
			return this.CreateModel(null);
		}

		protected internal virtual RecaptchaViewModel CreateModel(RecaptchaForm form)
		{
			var model = new RecaptchaViewModel();

			if(form != null)
				model.Form = form;

			return model;
		}

		public virtual IActionResult Index()
		{
			return this.View(this.CreateModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateRecaptchaToken]
		public virtual IActionResult Post(RecaptchaForm form)
		{
			var model = this.CreateModel(form);

			// ReSharper disable InvertIf
			if(this.ModelState.IsValid)
			{
				this.ModelState.Clear();
				model.Saved = true;
				model.Form = new RecaptchaForm();
			}
			// ReSharper restore InvertIf

			return this.View("Index", model);
		}

		#endregion
	}
}