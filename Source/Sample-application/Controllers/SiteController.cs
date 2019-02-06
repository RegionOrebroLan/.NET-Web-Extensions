using Microsoft.AspNetCore.Mvc;
using SampleApplication.Models.ViewModels.Shared;

namespace SampleApplication.Controllers
{
	public abstract class SiteController : Controller, ILayoutModifier
	{
		#region Methods

		public virtual void ModifyLayout(ILayout layout) { }

		#endregion
	}
}