using Microsoft.AspNet.Mvc;

namespace Nakkitehdas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Step1()
        {
            return View();
        }

        public IActionResult Step2()
        {
            return View();
        }

        public IActionResult Done()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
