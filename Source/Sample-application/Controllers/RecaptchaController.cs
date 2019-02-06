using System;
using Microsoft.AspNetCore.Mvc;
using SampleApplication.Business.Web.Mvc;
using SampleApplication.Models.Forms;
using SampleApplication.Models.ViewModels;
using SampleApplication.Models.ViewModels.Shared;

namespace SampleApplication.Controllers
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
			model.Heading = model.Title = "Recaptcha";

			if(form != null)
				model.Form = form;

			return model;
		}

		public virtual IActionResult Index()
		{
			return this.View(this.CreateModel());
		}

		public override void ModifyLayout(ILayout layout)
		{
			if(layout == null)
				throw new ArgumentNullException(nameof(layout));

			base.ModifyLayout(layout);

			layout.Settings.Recaptcha.Enabled = true;
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