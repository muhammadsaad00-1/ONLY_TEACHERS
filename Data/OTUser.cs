using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;


namespace OT.Areas.Identity.Data;

// Add profile data for application users by adding properties to the OTUser class
public class OTUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }
      [Display(Name = "User Status")]
    public int Paid { get; set; }
    public String? UserType {get; set;}


}



