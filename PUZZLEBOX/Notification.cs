namespace PUZZLEBOX;

[Index(nameof(AccountId), IsUnique = false)]
public class Notification
{
    // Unique id of a Notification. Not used for anything else.
    public int NotificationId { get; set; }

    // Serialized content of the notification.
    [Required]
    public string Content { get; set; } = null!;

    // Id of the Account that the notification belongs to.
    public int AccountId { get; set; }

    // Timestamp when the notification got created.
    public DateTime TimestampCreated { get; set; }
}
