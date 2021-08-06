using Application.Models.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public abstract class SiteController : Controller, ILayoutModifier
	{
		#region Methods

		public virtual void ModifyLayout(ILayout layout) { }

		#endregion
	}
}