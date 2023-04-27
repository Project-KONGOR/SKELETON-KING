using System.ComponentModel.DataAnnotations;

namespace ZORGATH;

public class Friend
{
    [Key]
    // Unique key in the DB. Isn't used for anything else.
    public int FriendId { get; set; }

    // Id of our Account.
    public int AccountId { get; set; }

    // Account we are friends with.
    [Required]
    public Account FriendAccount { get; set; } = null!;

    // Timestamp when the invite expires (null if friends).
    public DateTime? ExpirationDateTime { get; set; }

    // Friend group.
    [Required]
    [MaxLength(15)]
    public string Group { get; set; } = null!;
}
