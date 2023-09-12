using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models;

public class Guest

{
    [Key]
    public int GuestId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    
    // fk
    public int UserId {get;set;}
    // nav prop
    public User? WeddingGuest {get;set;}

    //fk
    public int WeddingId {get;set;}
    //nav prop
    public Wedding? NextWedding {get;set;}
}