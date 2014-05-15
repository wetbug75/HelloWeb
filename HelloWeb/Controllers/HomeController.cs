using System.Web.Mvc;
using HelloWeb.Enumerations;
using HelloWeb.Models;
using System.Net;

namespace HelloWeb.Controllers
{
	public class HomeController : Controller
	{
		// declare static variables that act as 'global' variables:
		private static AlertType _currentAlertType = AlertType.TestOff;
		private static bool      _lastAlertStatus;

        private static int pushCount = 0;
        private static int cycleCount = 0;

        PagerDutyApi api = new PagerDutyApi();

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
								},
                            new AlertMode
                                {
                                    Type = AlertType.StockCheck,
									Name = "Stock Alert",
									IsSelected = (_currentAlertType == AlertType.StockCheck),
									ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.StockCheck })
                                },
                            new AlertMode
                                {
                                    Type = AlertType.PressButton,
                                    Name = "Press 10 Times",
                                    IsSelected = (_currentAlertType == AlertType.PressButton),
                                    ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.PressButton })
                                },
                            new AlertMode
                                {
                                    Type = AlertType.PagerDuty,
                                    Name = "Pager Duty",
                                    IsSelected = (_currentAlertType == AlertType.PagerDuty),
                                    ChangeTypeUrl = Url.Action("ChangeAlertType", new { newAlertType = AlertType.PagerDuty })
                                },
						}
				};

			// display the home page view:
			return View(model);
		}

		public ActionResult ChangeAlertType(AlertType newAlertType)
		{
            pushCount++;

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
                case AlertType.StockCheck: //if stock is down, alert light turns on
                    statusValue = isStockDown("http://download.finance.yahoo.com/d/quotes.csv?s=aapl&f=snl1c1p2&e=.csv");
                    break;
                case AlertType.PressButton: //press a designated # of times to turn on light for designated # of cycles
                    statusValue = hasEnoughPushes(10, 2);
                    break;
                case AlertType.PagerDuty:
                    statusValue = api.HasAnOutstandingIncident();
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

        public bool hasEnoughPushes(int num, int cycles)
        {
            if (pushCount >= num)
            {
                cycleCount++;
                if (cycleCount >= cycles)
                {
                    cycleCount = 0;
                    pushCount = 0;
                }
                return true;
            } 
            return false;
        }

        public bool isStockDown(string x)
        {
            WebClient client = new WebClient();
            byte[] rawStock = client.DownloadData(x);
            x = System.Text.Encoding.UTF8.GetString(rawStock);
            x = x.Substring(x.LastIndexOf('"', x.Length - 4) + 1, 1); //gets + or - sign from percentage
            if(x.Equals("-"))
                return true;
            return false;
        }
	}
}
