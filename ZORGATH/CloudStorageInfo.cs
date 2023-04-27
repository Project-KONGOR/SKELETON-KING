using PhpSerializerNET;

namespace ZORGATH;

public class CloudStorageInfo
{
    public CloudStorageInfo(int accountId, string? useCloud, string? cloudAutoUpload, string? fileModifyTime)
    {
        AccountId = accountId;
        UseCloud = useCloud;
        CloudAutoUpload = cloudAutoUpload;
        FileModifyTime = fileModifyTime;
    }

    /// <summary>
    ///     The ID of the logged-in account, be it a master account or a sub-account.
    /// </summary>
    [PhpProperty("account_id")]
    public readonly int AccountId;

    /// <summary>
    ///     Whether the user has selected the option to automatically download
    ///     cloud settings on log-in.
    /// </summary>
    [PhpProperty("use_cloud")]
    public readonly string? UseCloud;

    /// <summary>
    ///     Whether the user has selected the option to automatically upload
    ///     any settings changes to the cloud.
    /// </summary>
    [PhpProperty("cloud_autoupload")]
    public readonly string? CloudAutoUpload;

    /// <summary>
    ///     The timestamp of when the "cloud.zip" was last modified. Extracted
    ///     from the file upload.
    /// </summary>
    [PhpProperty("file_modify_time")]
    public readonly string? FileModifyTime;
}
