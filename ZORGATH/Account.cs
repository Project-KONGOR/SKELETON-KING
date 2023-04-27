using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZORGATH;

/// <summary>
///     An Entity Framework object representing a playable Account.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [Required]
    // This is a transition property, automatically initialized by EF. Never null.
    public ElementUser User { get; set; } = null!;
}
