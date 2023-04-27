using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ZORGATH;

public class BountyContext : IdentityDbContext<ElementUser>
{
    public BountyContext(DbContextOptions<BountyContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
}
