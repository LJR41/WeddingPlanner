#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class LogUser
{        

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string LogEmail { get; set; }        
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string LogPassword { get; set; }          
    
}