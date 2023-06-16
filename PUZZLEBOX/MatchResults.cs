namespace PUZZLEBOX;

[Index(nameof(match_id), IsUnique = true)]
[Index(nameof(datetime), IsUnique = false)]
public class MatchResults
{
    [Key]
    public int match_id { get; set; }
    public long server_id { get; set; }
    public string? map { get; set; }
    public string? map_version { get; set; }
    public int time_played { get; set; }
    public int file_size { get; set; }
    public string? file_name { get; set; }
    public int c_state { get; set; }
    public string? version { get; set; }
    public int avgpsr { get; set; }
    public int avgpsr_team1 { get; set; }
    public int avgpsr_team2 { get; set; }
    public string? gamemode { get; set; }
    public int teamscoregoal { get; set; }
    public int playerscoregoal { get; set; }
    public int numrounds { get; set; }
    public string? release_stage { get; set; }
    public string? banned_heroes { get; set; }
    public int awd_mann { get; set; }
    public int awd_mqk { get; set; }
    public int awd_lgks { get; set; }
    public int awd_msd { get; set; }
    public int awd_mkill { get; set; }
    public int awd_masst { get; set; }
    public int awd_ledth { get; set; }
    public int awd_mbdmg { get; set; }
    public int awd_mwk { get; set; }
    public int awd_mhdd { get; set; }
    public int awd_hcs { get; set; }
    public int mvp { get; set; }
    public string? submission_debug { get; set; }

    // Additional field that are not set by the game server and populated manually.
    public string? name { get; set; } // server name
    public string? date { get; set; } // DEPRECATED -> TODO: convert this to a [NotMapped] string generated from "datetime"
    public DateTime datetime { get; set; }
    public string? time { get; set; } // time in seconds since the day started.
    public long timestamp { get; set; }  // unix timestamp in seconds since epoch
    public string? inventory { get; set; }
}
