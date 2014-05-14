using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace HelloWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC Andrew!";

            return View();
        }

        public ActionResult Stock()
        {
            //http://download.finance.yahoo.com/d/quotes.csv?s=aapl&f=snl1c1p2&e=.xml

            WebClient client = new WebClient();
            byte[] aapl = client.DownloadData("http://download.finance.yahoo.com/d/quotes.csv?s=aapl&f=snl1c1p2&e=.csv");
            String aaplData = System.Text.Encoding.UTF8.GetString(aapl);

            byte[] goog = client.DownloadData("http://download.finance.yahoo.com/d/quotes.csv?s=goog&f=snl1c1p2&e=.csv");
            String googData = System.Text.Encoding.UTF8.GetString(goog);

            byte[] micro = client.DownloadData("http://download.finance.yahoo.com/d/quotes.csv?s=msft&f=snl1c1p2&e=.csv");
            String microData = System.Text.Encoding.UTF8.GetString(micro);

            ViewBag.apple = StockFormat(aaplData);
            ViewBag.google = StockFormat(googData);
            ViewBag.microsoft = StockFormat(microData);

            //Response.Write(data);
            //Response.End();

            return View();
        }

        public ActionResult AirFare()
        {
            Response.Write("Airplanes!");
            return View();
        }

        //go here for stock info: http://vikku.info/codetrash/Yahoo_Finance_Stock_Quote_API
        public ActionResult About()
        {
            return View();
        }

        public string StockFormat(string word)
        {
            string result = word.Substring(word.IndexOf(',') + 2);
            result = result.Substring(0, result.IndexOf(',') - 1) + " | Last Trade Price: " + result.Substring(result.IndexOf(',') + 1);
            result = result.Substring(0, result.IndexOf(',')) + " | Change: " + result.Substring(result.IndexOf(',') + 1);
            result = result.Substring(0, result.IndexOf(',')) + " | Change in Percent: " + result.Substring(result.IndexOf(',') + 2);
            result = result.Substring(0, result.Length - 3); //why -3?
            return result;
        }

    }
}
