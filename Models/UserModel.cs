#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class User
{        
    [Key]        
    public int UserId { get; set; }
    
    [Required(ErrorMessage ="First name is required")]  
    [MinLength(2,ErrorMessage ="First name must be at least 2 characers.")]      
    public string FirstName { get; set; }
    
    [Required(ErrorMessage ="Last name is required")] 
    [MinLength(2,ErrorMessage ="Last name must be at least 2 characers.")]         
    public string LastName { get; set; }         
    
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="A valid email is required")]
    [UniqueEmail]
    public string Email { get; set; }        
    
    [Required(ErrorMessage ="Password required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }          
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    
    [Required(ErrorMessage ="Please confirm password")]
    // This does not need to be moved to the bottom
    // But it helps make it clear what is being mapped and what is not
    [NotMapped]
    // There is also a built-in attribute for comparing two fields we can use!
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConPassword { get; set; }

    // nav prop
    public List<Wedding> PlannedWeddings {get;set;} = new();

    // nav prop
    public List<Guest> WeddingsAsGuest {get;set;} = new();
}


public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}

