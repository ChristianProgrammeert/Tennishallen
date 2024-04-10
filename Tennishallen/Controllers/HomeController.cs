using Microsoft.AspNetCore.Mvc;

namespace Tennishallen.Controllers;

public class HomeController : Controller
{
    /// <summary>
    ///     Show the user the front page
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }
}