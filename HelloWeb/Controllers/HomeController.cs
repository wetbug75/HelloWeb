using System.Web.Mvc;

namespace HelloWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC Andrew!";

			return View();
		}

		public ActionResult Status()
		{
			return Content("1");
		}
	}
}
