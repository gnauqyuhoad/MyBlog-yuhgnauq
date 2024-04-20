using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Resume()
        {
            return View();
        }
    }
}
