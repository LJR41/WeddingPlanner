#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;

public class Wedding
{
    [Key]
    public int WeddingId {get;set;}

    [Required]
    public string WedderOne {get;set;}
    [Required]
    public string WedderTwo {get;set;}
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date {get;set;}
    [Required]
    public string Address {get;set;}

    //fk 
    public int UserId {get;set;}

    //nav prop
    public User? WeddingPlanner {get;set;}

    //nav prop

    public List<Guest> GuestList {get;set;} = new();

    
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}