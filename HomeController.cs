using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Demo_Web.Models;

namespace Demo_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static Dictionary<string, string> users = new Dictionary<string, string>();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Login()
    {
        return View();
    }

    // Xử lý đăng nhập
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (users.ContainsKey(username) && users[username] == password)
        {
            HttpContext.Session.SetString("User", username); // Lưu vào Session
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Error = "Mật khẩu nhập lại không khớp!";
            return View();
        }

        if (users.ContainsKey(username))
        {
            ViewBag.Error = "Tên đăng nhập đã tồn tại!";
            return View();
        }

        users[username] = password; // Lưu tài khoản vào danh sách giả lập
        return RedirectToAction("Login");
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
        {
            return RedirectToAction("Login"); // Chưa đăng nhập -> quay lại trang Login
        }
        ViewBag.Username = HttpContext.Session.GetString("User"); // Lấy tên user
        return View();
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Remove("User");
        return RedirectToAction("Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
