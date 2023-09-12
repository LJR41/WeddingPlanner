using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public UserController(ILogger<UserController> logger, MyContext context)    
    {        
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;    
    }
    [HttpGet("")]
    public IActionResult Index()
    {
        return View("Index");
    }
    [HttpPost("register/user")]
    public IActionResult RegisterUser(User newUser)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher = new();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("LogId", newUser.UserId);
        return RedirectToAction("AllWeddings", "Weddings");
    }
    [HttpPost("login/user")]
    public IActionResult LoginUser(LogUser logUser)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        User? dbUser = _context.Users.FirstOrDefault(u => u.Email ==  logUser.LogEmail);
        if(dbUser == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid credentials");
            return View("Index");
        }
        PasswordHasher<LogUser> hasher = new();
        PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(logUser,dbUser.Password,logUser.LogPassword);
        if(pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid credentials");
            return View("Index");
        }
        HttpContext.Session.SetInt32("LogId", dbUser.UserId);
        return RedirectToAction("AllWeddings", "Weddings");
    }

    [HttpPost("logout/user")]
    public IActionResult LogOut()
    {
        // HttpContext.Session.Clear();
        HttpContext.Session.Remove("LogId");
        return RedirectToAction("Index");
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
