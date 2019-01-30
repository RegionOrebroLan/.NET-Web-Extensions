using Microsoft.AspNetCore.Mvc;
using SampleApplication.Models.ViewModels;

namespace SampleApplication.Controllers
{
	public class HomeController : SiteController
	{
		#region Methods

		public virtual IActionResult Index()
		{
			return this.View(new ViewModel {Heading = "Home", Title = "Home"});
		}

		#endregion
	}
}