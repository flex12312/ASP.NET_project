using Microsoft.AspNetCore.Mvc;

namespace stepik_asp.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}