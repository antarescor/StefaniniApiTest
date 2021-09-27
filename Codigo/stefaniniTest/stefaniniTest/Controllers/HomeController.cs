using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace stefaniniTest.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            HttpClient cliente = new HttpClient();

            var json = await cliente.GetStringAsync("https://localhost:44331/api/GetAllBanks/");

            return View();
        }
    }
}
