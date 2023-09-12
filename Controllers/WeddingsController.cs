using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;

public class WeddingsController : Controller
{
    private readonly ILogger<WeddingsController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public WeddingsController(ILogger<WeddingsController> logger, MyContext context)
    {
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;
    }
    [SessionCheck]
    [HttpGet("weddings")]
    public ViewResult AllWeddings()
    {
        List<Wedding> Weddings = _context.Weddings.Include(w => w.WeddingPlanner).Include(w => w.GuestList).OrderByDescending(w => w.CreatedAt).ToList();
        return View(Weddings);
    }

    [SessionCheck]
    [HttpGet("weddings/new")]
    public ViewResult NewWedding()
    {
        return View();
    }

    [SessionCheck]
    [HttpGet("weddings/{weddingId}")]
    public IActionResult OneWedding(int weddingId)
    {
        Wedding? OneWedding = _context.Weddings.Include(w => w.WeddingPlanner).Include(w => w.GuestList).ThenInclude(w => w.WeddingGuest).FirstOrDefault(w => w.WeddingId == weddingId);
        if (OneWedding == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View(OneWedding);

    }
    [HttpPost("weddings/{weddingId}/rsvp")]
    public IActionResult ToggleRSVP(int weddingId)
    {
        int LogId = (int)HttpContext.Session.GetInt32("LogId");
        Guest existingGuest = _context.Guests.FirstOrDefault(g => g.WeddingId == weddingId && g.UserId == LogId);
        if (existingGuest == null)
        {
            Guest newGuest = new()
            {
                UserId = LogId,
                WeddingId = weddingId
            };
            _context.Add(newGuest);

        }
        else
        {
            _context.Remove(existingGuest);
        }
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }

    [SessionCheck]
    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            return View("NewWedding");
        }
        newWedding.UserId = (int)HttpContext.Session.GetInt32("LogId");
        _context.Add(newWedding);
        _context.SaveChanges();
        return RedirectToAction("AllWeddings");
    }

    [SessionCheck]
    [HttpPost("weddings/{weddingId}/delete")]
    public IActionResult DeleteWedding(int weddingId)
    {
        Wedding ToBeRemoved = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
        if(ToBeRemoved !=null)
        {
            _context.Remove(ToBeRemoved);
            _context.SaveChanges();
        }
        return RedirectToAction("AllWeddings");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
