﻿namespace ZORGATH;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class BountyContext : IdentityDbContext<ElementUser>
{
    public BountyContext(DbContextOptions<BountyContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Clan> Clans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        EntityTypeBuilder<Friend> friendEntity = modelBuilder.Entity<Friend>();
        friendEntity.HasOne(friend => friend.FriendAccount).WithMany().HasForeignKey("FriendAccountId");

        // Implicitly map between a HashSet<String> and a single-string storage in the DB model. To avoid needing a custom Entity and DB Table.
        ValueConverter<ICollection<string>, string> splitStringConverter = new(collection => string.Join('|', collection), collection => collection.Split('|', StringSplitOptions.RemoveEmptyEntries).ToHashSet());

        ValueComparer<ICollection<string>> listValueComparer = new(
            (collection1, collection2) => collection1!.SequenceEqual(collection2!),
            collection => collection.Aggregate(0, (aggregate, source) => HashCode.Combine(aggregate, source.GetHashCode())),
            collection => collection.ToHashSet());

        modelBuilder.Entity<Account>()
                    .Property(nameof(Account.AutoConnectChatChannels))
                    .HasConversion(splitStringConverter, listValueComparer);
        modelBuilder.Entity<Account>()
                    .Property(nameof(Account.IgnoredList))
                    .HasConversion(splitStringConverter, listValueComparer);
        modelBuilder.Entity<Account>()
                    .Property(nameof(Account.SelectedUpgradeCodes))
                    .HasConversion(splitStringConverter, listValueComparer);
        modelBuilder.Entity<ElementUser>()
                    .Property(nameof(ElementUser.UnlockedUpgradeCodes))
                    .HasConversion(splitStringConverter, listValueComparer);
        modelBuilder.Entity<ElementUser>()
                   .Property(nameof(ElementUser.Id))
                   .HasMaxLength(36);
        modelBuilder.Entity<ElementUser>()
                   .Property(nameof(ElementUser.UserName))
                   .HasMaxLength(15);
        modelBuilder.Entity<ElementUser>()
                   .Property(nameof(ElementUser.NormalizedUserName))
                   .HasMaxLength(15);
        modelBuilder.Entity<ElementUser>()
                   .Property(nameof(ElementUser.SecurityStamp))
                   .HasMaxLength(32);
        modelBuilder.Entity<ElementUser>()
                   .Property(nameof(ElementUser.ConcurrencyStamp))
                   .HasMaxLength(36);
    }
}
