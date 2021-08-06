using Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HomeController : SiteController
	{
		#region Methods

		public virtual IActionResult Index()
		{
			return this.View(new ViewModel { Heading = "Home", Title = "Home" });
		}

		#endregion
	}
}