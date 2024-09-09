using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Lab_0.Controllers
{
    public class MyController : Controller
    {
        // GET: /My/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /My/Welcome/ 
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }

        /*// GET: /My/Welcome/ 
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }*/

        /*// GET: /My/Welcome/ 
        // Requires using System.Text.Encodings.Web;
        public string Welcome(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }*/
    }
}
