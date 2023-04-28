namespace ZORGATH;

public class BountyContext : IdentityDbContext<ElementUser>
{
    public BountyContext(DbContextOptions<BountyContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
}
