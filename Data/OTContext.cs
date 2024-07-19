using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OT.Areas.Identity.Data;

namespace OT.Data;

public class OTContext : IdentityDbContext<OTUser>
{
    public OTContext(DbContextOptions<OTContext> options)
        : base(options)
    {
    }
    public DbSet<OT.Models.Post> Post { get; set; }=default!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
