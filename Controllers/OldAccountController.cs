//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using TaskManager.Models;
//using System.Threading.Tasks;

//namespace TaskManager.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly SignInManager<User> _signInManager;
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            _signInManager = signInManager;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        // GET: Login
//        [HttpGet]
//        public IActionResult Login()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                // Sprawdź rolę użytkownika i przekieruj do odpowiedniego dashboardu
//                if (User.IsInRole("Administrator"))
//                {
//                    return RedirectToAction("Index", "AdminDashboard");
//                }
//                return RedirectToAction("Index", "UserDashboard");
//            }

//            // Przekierowanie do widoku logowania w Identity
//            return RedirectToPage("/Account/Login", new { area = "Identity" });
//        }

//        // POST: Login
//        //[HttpPost]
//        //public async Task<IActionResult> Login(string username, string password, string role)
//        //{
//        //    var user = await _userManager.FindByNameAsync(username);

//        //    if (user == null || !await _userManager.CheckPasswordAsync(user, password))
//        //    {
//        //        ModelState.AddModelError(string.Empty, "Invalid username or password.");
//        //        return RedirectToPage("/Account/Login", new { area = "Identity" });
//        //    }

//        //    // Check if the user has the selected role
//        //    if (!await _userManager.IsInRoleAsync(user, role))
//        //    {
//        //        ModelState.AddModelError(string.Empty, "Invalid role for this user.");
//        //        return RedirectToPage("/Account/Login", new { area = "Identity" });
//        //    }

//        //    var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

//        //    if (result.Succeeded)
//        //    {
//        //        return role == "Administrator"
//        //            ? RedirectToAction("Index", "AdminDashboard")
//        //            : RedirectToAction("Index", "UserDashboard");
//        //    }

//        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//        //    return RedirectToPage("/Account/Login", new { area = "Identity" });
//        //}

//        // GET: Register
//        //[HttpGet]
//        //public IActionResult Register()
//        //{
//        //    return RedirectToPage("/Account/Register", new { area = "Identity" });
//        //}

//        //// POST: Register
//        //[HttpPost]
//        //public async Task<IActionResult> Register(string username, string email, string password, string role)
//        //{
//        //    if (!await _roleManager.RoleExistsAsync(role))
//        //    {
//        //        ModelState.AddModelError(string.Empty, "Invalid role selected.");
//        //        return RedirectToPage("/Account/Register", new { area = "Identity" });
//        //    }

//        //    User user = role switch
//        //    {
//        //        "Administrator" => new Administrator
//        //        {
//        //            UserName = username,
//        //            Email = email
//        //        },
//        //        _ => new RegularUser
//        //        {
//        //            UserName = username,
//        //            Email = email
//        //        }
//        //    };

//        //    var result = await _userManager.CreateAsync(user, password);
//        //    if (result.Succeeded)
//        //    {
//        //        await _userManager.AddToRoleAsync(user, role);

//        //        // Automatyczne logowanie użytkownika po rejestracji
//        //        await _signInManager.SignInAsync(user, isPersistent: false);

//        //        // Przekierowanie na odpowiedni dashboard
//        //        return role == "Administrator"
//        //            ? RedirectToAction("Index", "AdminDashboard")
//        //            : RedirectToAction("Index", "UserDashboard");
//        //    }

//        //    // Obsługa błędów walidacji
//        //    foreach (var error in result.Errors)
//        //    {
//        //        ModelState.AddModelError(string.Empty, error.Description);
//        //    }

//        //    return RedirectToPage("/Account/Register", new { area = "Identity" });
//        //}


//        // POST: Logout
//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync();
//            return RedirectToPage("/Account/Login", new { area = "Identity" });
//        }
//    }
//}
