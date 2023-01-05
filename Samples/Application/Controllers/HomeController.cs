using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HomeController : SiteController
	{
		#region Methods

		public virtual IActionResult Index()
		{
			return this.View();
		}

		#endregion
	}
}