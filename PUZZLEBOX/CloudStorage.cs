namespace PUZZLEBOX;

/// <summary>
///     Stores a user's cloud storage settings and preferences.
/// </summary>
public class CloudStorage
{
    /// <summary>
    ///     Primary key identifier for this cloud storage entity.
    /// </summary>
    [Key]
    [Required]
    public int CloudStorageId { get; set; }

    /// <summary>
    ///     The ID of the account, be it a master account or a sub-account.
    /// </summary>
    [Required]
    public int AccountId { get; set; }

    /// <summary>
    ///     Whether the user has selected the option to automatically download
    ///     cloud settings on log-in.
    /// </summary>
    [Required]
    public bool UseCloud { get; set; } = false;

    /// <summary>
    ///     Whether the user has selected the option to automatically upload
    ///     any settings changes to the cloud.
    /// </summary>
    [Required]
    public bool CloudAutoupload { get; set; } = false;

    /// <summary>
    ///     The timestamp of when the "cloud.zip" was last modified. Extracted
    ///     from the file upload.
    /// </summary>
    public string? FileModifyTime { get; set; }

    /// <summary>
    ///     The zipfile containing the cloud.cfg that was backed up from the client.
    /// </summary>
    public byte[]? CloudCfgZip { get; set; }
}