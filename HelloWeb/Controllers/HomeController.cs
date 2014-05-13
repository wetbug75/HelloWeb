using System.Web.Mvc;
using HelloWeb.Enumerations;
using HelloWeb.Models;

namespace HelloWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			// lookup what the current alert type is:
			var currentAlertType = AlertType.TestOff;

			// define the model for the home page:
			var model = new HomeModel
				{
					AlertModes = new []
						{
							new AlertMode
								{
									Type = AlertType.TestOff,
									Name = "Test - OFF",
									IsSelected = (currentAlertType == AlertType.TestOff)
								},
							new AlertMode
								{
									Type = AlertType.TestOn,
									Name = "Test - ON",
									IsSelected = (currentAlertType == AlertType.TestOn)
								},
							new AlertMode
								{
									Type = AlertType.TestBlink,
									Name = "Test - BLINK",
									IsSelected = (currentAlertType == AlertType.TestBlink)
								}
						}
				};

			return View(model);
		}

		public ActionResult Status()
		{
			return Content("1");
		}
	}
}
