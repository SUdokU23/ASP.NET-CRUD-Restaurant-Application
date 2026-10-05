using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Resto.Models;
using Resto.Service;
using Resto.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Resto.ViewModels.Auth;

namespace Resto.Controllers;

public class AuthController(AuthService authService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel reg)
    {
        if (reg.Password != reg.ConfPass)
        {
            TempData["Error"] = "Passwords do not match!";
            return Index();
        }
        var user = new User
        {
            HashPwd = BCrypt.Net.BCrypt.HashPassword(reg.Password),
            IsAdministrator = reg.IsAdmin
        };

        if ( await authService.AddUser(user))
        {
            return RedirectToAction("Login", "Auth");
        }
        
        TempData["Error"] = "Internal error pls try again later";
        return Index();
    }
    
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (!await authService.IsUserValid(int.Parse(login.Id)))
        {
            TempData["Error"] = "User not found!";
            return Index();
        }

        if (await authService.IsPasswordCorrect(int.Parse(login.Id), login.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Id),
                new Claim(ClaimTypes.Role, await authService.GetRole(int.Parse(login.Id)))
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimIdentity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            return RedirectToAction("Index", "Home");
        }
        TempData["Error"] = "Password is incorrect!";
        return Login();
    }
    
    public async Task<IActionResult> Logout()
    {
        // Sign the user out
        await HttpContext.SignOutAsync();

        // Remove all cookies
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        // Redirect to home or login page
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ViewUsers()
    {
        IEnumerable<User> users = await authService.GetAllUsers();
        return View(users);
    }
}