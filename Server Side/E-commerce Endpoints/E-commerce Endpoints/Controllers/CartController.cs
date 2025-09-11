using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Endpoints.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
