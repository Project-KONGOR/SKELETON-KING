using System.ComponentModel.DataAnnotations;

namespace ZORGATH;

public class CloudStorage
{
    /// <summary>
    ///     Primary key identifier for this cloud storage entity.
    /// </summary>
    public int CloudStorageId { get; set; }

    /// <summary>
    ///     The ID of the account, be it a master account or a sub-account.
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    ///     Whether the user has selected the option to automatically download
    ///     cloud settings on log-in.
    /// </summary>
    [Required]
    public string UseCloud { get; set; } = "0";

    /// <summary>
    ///     Whether the user has selected the option to automatically upload
    ///     any settings changes to the cloud.
    /// </summary>
    [Required]
    public string CloudAutoUpload { get; set; } = "0";

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
