using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public const string AdminUsername = "admin";
        public const string AdminPassword = "Admin123@";
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BlogDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, BlogDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Views()
        {
            return View(await _context.Blogs.Include(b => b.Category).ToListAsync());
        }
        /*public IActionResult Admin()
        {
            return View();
        }*/
        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản admin và sign in
                if (username == AdminUsername && password == AdminPassword)
                {
                    var admin = await _userManager.FindByNameAsync(AdminUsername);
                    if (admin == null)
                    {
                        admin = new AppUser { UserName = AdminUsername };
                        var res = await _userManager.CreateAsync(admin, AdminPassword);
                        if (!res.Succeeded)
                        {
                            ModelState.AddModelError("", "Error while creating user");
                            return RedirectToAction("Index");
                        }
                        if (!await _roleManager.RoleExistsAsync("Admin"))
                        {
                            if (!_roleManager.CreateAsync(new IdentityRole("Admin")).Result.Succeeded)
                            {
                                ModelState.AddModelError("", "Error while creating role");
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    //sign in and add role admin
                    await _signInManager.SignInAsync(admin, false);
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    return RedirectToAction("Index", "Blog", new { area = "Admin" });
                }
                // Kiểm tra tài khoản người dùng
                /*var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid username or password.");
                }*/
            }
            return RedirectToAction("Index");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account.Username == AdminUsername && account.Password == AdminPassword)
                {
                    var admin = await _userManager.FindByNameAsync(AdminUsername);
                    if (admin == null)
                    {
                        admin = new AppUser { UserName = AdminUsername };
                        var res = await _userManager.CreateAsync(admin, AdminPassword);
                        if (!res.Succeeded)
                        {
                            ModelState.AddModelError("", "Error while creating user");
                            return RedirectToAction("Index");
                        }
                        if (!await _roleManager.RoleExistsAsync("Admin"))
                        {
                            if (!_roleManager.CreateAsync(new IdentityRole("Admin")).Result.Succeeded)
                            {
                                ModelState.AddModelError("", "Error while creating role");
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    //sign in and add role admin
                    await _signInManager.SignInAsync(admin, false);
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    return RedirectToAction("Index", "Blog", new { area = "Admin" });
                }
                AppUser user = new AppUser
                {
                    UserName = account.Username,
                    Email = account.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, account.Password);
                if (result.Succeeded)
                {
                    // đăng nhập và chuyển hướng
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(account);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
