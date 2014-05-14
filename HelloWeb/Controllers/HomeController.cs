using System.Web.Mvc;
using HelloWeb.Enumerations;
using HelloWeb.Models;

namespace HelloWeb.Controllers
{
	public class HomeController : Controller
	{
		// declare static variables that act as 'global' variables:
		private static AlertType _currentAlertType = AlertType.TestOff;
		private static bool      _lastAlertStatus;

		public ActionResult Index()
		{
			// define the model for the home page:
			var model = new HomeModel
				{
					AlertModes = new []
						{
							new AlertMode
								{
									Type = AlertType.TestOff,
									Name = "Test - OFF",
									IsSelected = (_currentAlertType == AlertType.TestOff),
									ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.TestOff })
								},
							new AlertMode
								{
									Type = AlertType.TestOn,
									Name = "Test - ON",
									IsSelected = (_currentAlertType == AlertType.TestOn),
									ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.TestOn })
								},
							new AlertMode
								{
									Type = AlertType.TestBlink,
									Name = "Test - BLINK",
									IsSelected = (_currentAlertType == AlertType.TestBlink),
									ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.TestBlink })
								}
						}
				};

			// display the home page view:
			return View(model);
		}

		public ActionResult ChangeAlertType(AlertType newAlertType)
		{
			// change the alert type to the new value passed in:
			_currentAlertType = newAlertType;

			// redirect back to the home page to view the result:
			return RedirectToAction("Index");
		}

		public ActionResult Status()
		{
			// deterine the current status value based on the alert type:
			bool statusValue;
			switch (_currentAlertType)
			{
				case AlertType.TestOff:
					statusValue = false;
					break;
				case AlertType.TestOn:
					statusValue = true;
					break;
				case AlertType.TestBlink:
					statusValue = !_lastAlertStatus;
					break;
				default:
					statusValue = false;
					break;
			}

			// save the last alert status:
			_lastAlertStatus = statusValue;

			// return "1" if true; otherwise "0":
			return Content(statusValue ? "1" : "0");
		}
	}
}
