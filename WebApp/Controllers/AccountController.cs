using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Filters;
using WebApp.Model;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View(new CredentialViewModel());
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public async Task<IActionResult> Login(CredentialViewModel credential)
        {
            if(credential.EmailAddress == "admin@company.com" && credential.Password == "password" ||
                credential.EmailAddress == "user@company.com" && credential.Password == "password")
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, credential.EmailAddress));

                if (credential.EmailAddress == "admin@company.com")
                {
                    claims.Add(new Claim("Role", "Admin"));
                }
                else
                {
                    claims.Add(new Claim("Role", "User"));
                }

                var identity = new ClaimsIdentity(claims, "CookieScheme");
                var userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieScheme", userPrincipal);


                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("Error", "Not able to login.");
            }

            return View(credential);
        }
         
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("CookieScheme");

            return RedirectToAction("Index", "Home");
        }
    }
}
