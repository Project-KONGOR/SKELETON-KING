USE [BOUNTY]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Name] [nvarchar](20) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClanId] [int] NULL,
	[ClanTier] [int] NOT NULL,
	[AscensionLevel] [int] NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
	[LastActivity] [datetime2](7) NOT NULL,
	[AccountType] [int] NOT NULL,
	[PlayerSeasonStatsRankedId] [int] NOT NULL,
	[PlayerSeasonStatsPublicId] [int] NOT NULL,
	[PlayerSeasonStatsMidWarsId] [int] NOT NULL,
	[AutoConnectChatChannels] [nvarchar](max) NOT NULL,
	[SelectedUpgradeCodes] [nvarchar](max) NOT NULL,
	[IgnoredList] [nvarchar](max) NOT NULL,
	[PlayerSeasonStatsRankedCasualId] [int] NOT NULL,
	[HardwareIdCollection] [nvarchar](max) NOT NULL,
	[IpAddressCollection] [nvarchar](max) NOT NULL,
	[MacAddressCollection] [nvarchar](max) NOT NULL,
	[SystemInformationCollection] [nvarchar](max) NOT NULL,
	[Cookie] [nvarchar](32) NULL,
	[LastPlayedMatchId] [int] NOT NULL,
	[PlayerSeasonStatsGrimmsCrossingId] [int] NULL,
	[LastPlayedFromInternetCafe] [bit] NOT NULL,
	[LastPlayedHardwareSnapshotHash] [varbinary](64) NULL,
	[LastPlayedUniqueHardwareHash] [varbinary](64) NULL,
	[AccountId] [int] NOT NULL,
	[StatResetCount] [int] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArchivedStats]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArchivedStats](
	[ResetId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ResetType] [nvarchar](max) NOT NULL,
	[ResetTime] [datetime2](7) NOT NULL,
	[Stats_Rating] [real] NOT NULL,
	[Stats_Wins] [int] NOT NULL,
	[Stats_Losses] [int] NOT NULL,
	[Stats_Concedes] [int] NOT NULL,
	[Stats_ConcedeVotes] [int] NOT NULL,
	[Stats_Buybacks] [int] NOT NULL,
	[Stats_TimesDisconnected] [int] NOT NULL,
	[Stats_TimesKicked] [int] NOT NULL,
	[Stats_HeroKills] [int] NOT NULL,
	[Stats_HeroDamage] [int] NOT NULL,
	[Stats_HeroExp] [int] NOT NULL,
	[Stats_HeroKillsGold] [int] NOT NULL,
	[Stats_HeroAssists] [int] NOT NULL,
	[Stats_Deaths] [int] NOT NULL,
	[Stats_GoldLost2Death] [int] NOT NULL,
	[Stats_SecsDead] [int] NOT NULL,
	[Stats_TeamCreepKills] [int] NOT NULL,
	[Stats_TeamCreepDmg] [int] NOT NULL,
	[Stats_TeamCreepExp] [int] NOT NULL,
	[Stats_TeamCreepGold] [int] NOT NULL,
	[Stats_NeutralCreepKills] [int] NOT NULL,
	[Stats_NeutralCreepDmg] [int] NOT NULL,
	[Stats_NeutralCreepExp] [int] NOT NULL,
	[Stats_NeutralCreepGold] [int] NOT NULL,
	[Stats_BDmg] [int] NOT NULL,
	[Stats_BDmgExp] [int] NOT NULL,
	[Stats_Razed] [int] NOT NULL,
	[Stats_BGold] [int] NOT NULL,
	[Stats_Denies] [int] NOT NULL,
	[Stats_ExpDenies] [int] NOT NULL,
	[Stats_Gold] [int] NOT NULL,
	[Stats_GoldSpent] [int] NOT NULL,
	[Stats_Exp] [int] NOT NULL,
	[Stats_Actions] [int] NOT NULL,
	[Stats_Secs] [int] NOT NULL,
	[Stats_Consumables] [int] NOT NULL,
	[Stats_Wards] [int] NOT NULL,
	[Stats_EmPlayed] [int] NOT NULL,
	[Stats_Level] [int] NOT NULL,
	[Stats_LevelExp] [int] NOT NULL,
	[Stats_MinExp] [int] NOT NULL,
	[Stats_MaxExp] [int] NOT NULL,
	[Stats_TimeEarningExp] [int] NOT NULL,
	[Stats_Bloodlust] [int] NOT NULL,
	[Stats_DoubleKill] [int] NOT NULL,
	[Stats_TrippleKill] [int] NOT NULL,
	[Stats_QuadKill] [int] NOT NULL,
	[Stats_Annihilation] [int] NOT NULL,
	[Stats_Ks3] [int] NOT NULL,
	[Stats_Ks4] [int] NOT NULL,
	[Stats_Ks5] [int] NOT NULL,
	[Stats_Ks6] [int] NOT NULL,
	[Stats_Ks7] [int] NOT NULL,
	[Stats_Ks8] [int] NOT NULL,
	[Stats_Ks9] [int] NOT NULL,
	[Stats_Ks10] [int] NOT NULL,
	[Stats_Ks15] [int] NOT NULL,
	[Stats_Smackdown] [int] NOT NULL,
	[Stats_Humiliation] [int] NOT NULL,
	[Stats_Nemesis] [int] NOT NULL,
	[Stats_Retribution] [int] NOT NULL,
	[Stats_WinStreak] [int] NOT NULL,
	[Stats_SerializedMatchIds] [nvarchar](max) NOT NULL,
	[Stats_SerializedMatchDates] [nvarchar](max) NOT NULL,
	[Stats_PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[Stats_PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MostKills] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[Stats_PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[Stats_PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[Stats_PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[Stats_PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[Stats_PlayerAwardSummary_MVP] [int] NOT NULL,
	[Stats_SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[Stats_PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_ArchivedStats] PRIMARY KEY CLUSTERED 
(
	[ResetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Salt] [nvarchar](max) NOT NULL,
	[PasswordSalt] [nvarchar](max) NOT NULL,
	[HashedPassword] [nvarchar](max) NOT NULL,
	[Points] [int] NOT NULL,
	[MMPoints] [int] NOT NULL,
	[UnlockedUpgradeCodes] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Tickets] [int] NOT NULL,
	[TotalExperience] [int] NOT NULL,
	[TotalLevel] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clans]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clans](
	[ClanId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
	[Tag] [nvarchar](4) NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Clans] PRIMARY KEY CLUSTERED 
(
	[ClanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CloudStorages]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CloudStorages](
	[CloudStorageId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[UseCloud] [nvarchar](max) NOT NULL,
	[CloudAutoupload] [nvarchar](max) NOT NULL,
	[FileModifyTime] [nvarchar](max) NULL,
	[CloudCfgZip] [varbinary](max) NULL,
 CONSTRAINT [PK_CloudStorages] PRIMARY KEY CLUSTERED 
(
	[CloudStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Collectives]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collectives](
	[Name] [nvarchar](450) NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Multiplier] [float] NOT NULL,
	[NumberOfCouponsRetrieved] [int] NOT NULL,
 CONSTRAINT [PK_Collectives] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[Code] [nvarchar](450) NOT NULL,
	[Points] [int] NOT NULL,
	[MMPoints] [int] NOT NULL,
	[UpgradeId] [int] NOT NULL,
	[ClaimedById] [nvarchar](450) NULL,
	[ClaimedDateTime] [datetime2](7) NOT NULL,
	[CollectiveId] [int] NOT NULL,
	[DonorId] [int] NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friends]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friends](
	[FriendId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[FriendAccountId] [int] NULL,
	[ExpirationDateTime] [datetime2](7) NULL,
	[Group] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED 
(
	[FriendId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameServerManagers]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameServerManagers](
	[GameServerManagerId] [int] NOT NULL,
	[Cookie] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GameServerManagers] PRIMARY KEY CLUSTERED 
(
	[GameServerManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameServers]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameServers](
	[GameServerId] [int] NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
	[TimestampLastSession] [datetime2](7) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Port] [smallint] NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[HostName] [nvarchar](max) NOT NULL,
	[HostPasswordHash] [nvarchar](max) NOT NULL,
	[Official] [bit] NOT NULL,
	[Verifier] [nvarchar](max) NOT NULL,
	[Cookie] [nvarchar](max) NOT NULL,
	[GameServerManagerId] [int] NOT NULL,
 CONSTRAINT [PK_GameServers] PRIMARY KEY CLUSTERED 
(
	[GameServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GrimmsCrossingTeamStats]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrimmsCrossingTeamStats](
	[AccountIds] [nvarchar](450) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_GrimmsCrossingTeamStats] PRIMARY KEY CLUSTERED 
(
	[AccountIds] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guides]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guides](
	[GuideId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[HeroName] [nvarchar](max) NOT NULL,
	[HeroIdentifier] [nvarchar](max) NOT NULL,
	[Intro] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[StartingItems] [nvarchar](max) NOT NULL,
	[EarlyGameItems] [nvarchar](max) NOT NULL,
	[CoreItems] [nvarchar](max) NOT NULL,
	[LuxuryItems] [nvarchar](max) NOT NULL,
	[AbilityQueue] [nvarchar](max) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[Rating] [real] NOT NULL,
	[UpVotes] [int] NOT NULL,
	[DownVotes] [int] NOT NULL,
	[Public] [bit] NOT NULL,
	[Featured] [bit] NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
	[TimestampLastUpdated] [datetime2](7) NULL,
 CONSTRAINT [PK_Guides] PRIMARY KEY CLUSTERED 
(
	[GuideId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HardwareSnapshots]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HardwareSnapshots](
	[Hash] [varbinary](64) NOT NULL,
	[SnapshotJsonCompressed] [varbinary](max) NOT NULL,
	[FirstTimeObserved] [datetime2](7) NOT NULL,
	[HardwareId] [nvarchar](64) NULL,
	[IPAddress] [nvarchar](39) NULL,
	[UserId] [nvarchar](36) NULL,
 CONSTRAINT [PK_HardwareSnapshots] PRIMARY KEY CLUSTERED 
(
	[Hash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentReports]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentReports](
	[IncidentReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReporterId] [int] NOT NULL,
	[AccusedId] [int] NOT NULL,
	[MatchId] [int] NOT NULL,
	[IncidentType] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[OffenseOccurrenceTime] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ResolutionDetails] [nvarchar](max) NULL,
	[ReviewerId] [int] NULL,
 CONSTRAINT [PK_IncidentReports] PRIMARY KEY CLUSTERED 
(
	[IncidentReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Masteries]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Masteries](
	[MasteryId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Hero_Accursed_XP] [int] NOT NULL,
	[Hero_Adrenaline_XP] [int] NOT NULL,
	[Hero_Aluna_XP] [int] NOT NULL,
	[Hero_Andromeda_XP] [int] NOT NULL,
	[Hero_Apex_XP] [int] NOT NULL,
	[Hero_Arachna_XP] [int] NOT NULL,
	[Hero_Armadon_XP] [int] NOT NULL,
	[Hero_Artesia_XP] [int] NOT NULL,
	[Hero_Artillery_XP] [int] NOT NULL,
	[Hero_BabaYaga_XP] [int] NOT NULL,
	[Hero_Behemoth_XP] [int] NOT NULL,
	[Hero_Bephelgor_XP] [int] NOT NULL,
	[Hero_Berzerker_XP] [int] NOT NULL,
	[Hero_Blitz_XP] [int] NOT NULL,
	[Hero_Bombardier_XP] [int] NOT NULL,
	[Hero_Bubbles_XP] [int] NOT NULL,
	[Hero_Bushwack_XP] [int] NOT NULL,
	[Hero_Calamity_XP] [int] NOT NULL,
	[Hero_Chipper_XP] [int] NOT NULL,
	[Hero_Chi_XP] [int] NOT NULL,
	[Hero_Chronos_XP] [int] NOT NULL,
	[Hero_Circe_XP] [int] NOT NULL,
	[Hero_CorruptedDisciple_XP] [int] NOT NULL,
	[Hero_Cthulhuphant_XP] [int] NOT NULL,
	[Hero_Dampeer_XP] [int] NOT NULL,
	[Hero_Deadlift_XP] [int] NOT NULL,
	[Hero_Deadwood_XP] [int] NOT NULL,
	[Hero_Defiler_XP] [int] NOT NULL,
	[Hero_Devourer_XP] [int] NOT NULL,
	[Hero_DiseasedRider_XP] [int] NOT NULL,
	[Hero_DoctorRepulsor_XP] [int] NOT NULL,
	[Hero_Dreadknight_XP] [int] NOT NULL,
	[Hero_DrunkenMaster_XP] [int] NOT NULL,
	[Hero_DwarfMagi_XP] [int] NOT NULL,
	[Hero_Ebulus_XP] [int] NOT NULL,
	[Hero_Electrician_XP] [int] NOT NULL,
	[Hero_Ellonia_XP] [int] NOT NULL,
	[Hero_EmeraldWarden_XP] [int] NOT NULL,
	[Hero_Empath_XP] [int] NOT NULL,
	[Hero_Engineer_XP] [int] NOT NULL,
	[Hero_Fade_XP] [int] NOT NULL,
	[Hero_Fairy_XP] [int] NOT NULL,
	[Hero_FlameDragon_XP] [int] NOT NULL,
	[Hero_FlintBeastwood_XP] [int] NOT NULL,
	[Hero_Flux_XP] [int] NOT NULL,
	[Hero_ForsakenArcher_XP] [int] NOT NULL,
	[Hero_Frosty_XP] [int] NOT NULL,
	[Hero_Gauntlet_XP] [int] NOT NULL,
	[Hero_Gemini_XP] [int] NOT NULL,
	[Hero_Geomancer_XP] [int] NOT NULL,
	[Hero_Gladiator_XP] [int] NOT NULL,
	[Hero_Goldenveil_XP] [int] NOT NULL,
	[Hero_Grinex_XP] [int] NOT NULL,
	[Hero_Gunblade_XP] [int] NOT NULL,
	[Hero_Hammerstorm_XP] [int] NOT NULL,
	[Hero_Hantumon_XP] [int] NOT NULL,
	[Hero_Hellbringer_XP] [int] NOT NULL,
	[Hero_HellDemon_XP] [int] NOT NULL,
	[Hero_Hiro_XP] [int] NOT NULL,
	[Hero_Hunter_XP] [int] NOT NULL,
	[Hero_Hydromancer_XP] [int] NOT NULL,
	[Hero_Ichor_XP] [int] NOT NULL,
	[Hero_Javaras_XP] [int] NOT NULL,
	[Hero_Jereziah_XP] [int] NOT NULL,
	[Hero_Kane_XP] [int] NOT NULL,
	[Hero_Kenisis_XP] [int] NOT NULL,
	[Hero_KingKlout_XP] [int] NOT NULL,
	[Hero_Klanx_XP] [int] NOT NULL,
	[Hero_Kraken_XP] [int] NOT NULL,
	[Hero_Krixi_XP] [int] NOT NULL,
	[Hero_Kunas_XP] [int] NOT NULL,
	[Hero_Legionnaire_XP] [int] NOT NULL,
	[Hero_Lodestone_XP] [int] NOT NULL,
	[Hero_Magmar_XP] [int] NOT NULL,
	[Hero_Maliken_XP] [int] NOT NULL,
	[Hero_Martyr_XP] [int] NOT NULL,
	[Hero_MasterOfArms_XP] [int] NOT NULL,
	[Hero_Midas_XP] [int] NOT NULL,
	[Hero_Mimix_XP] [int] NOT NULL,
	[Hero_Moira_XP] [int] NOT NULL,
	[Hero_Monarch_XP] [int] NOT NULL,
	[Hero_MonkeyKing_XP] [int] NOT NULL,
	[Hero_Moraxus_XP] [int] NOT NULL,
	[Hero_Mumra_XP] [int] NOT NULL,
	[Hero_Nitro_XP] [int] NOT NULL,
	[Hero_Nomad_XP] [int] NOT NULL,
	[Hero_Oogie_XP] [int] NOT NULL,
	[Hero_Ophelia_XP] [int] NOT NULL,
	[Hero_Panda_XP] [int] NOT NULL,
	[Hero_Parallax_XP] [int] NOT NULL,
	[Hero_Parasite_XP] [int] NOT NULL,
	[Hero_Pearl_XP] [int] NOT NULL,
	[Hero_Pestilence_XP] [int] NOT NULL,
	[Hero_Plant_XP] [int] NOT NULL,
	[Hero_PollywogPriest_XP] [int] NOT NULL,
	[Hero_Predator_XP] [int] NOT NULL,
	[Hero_Prisoner_XP] [int] NOT NULL,
	[Hero_Prophet_XP] [int] NOT NULL,
	[Hero_PuppetMaster_XP] [int] NOT NULL,
	[Hero_Pyromancer_XP] [int] NOT NULL,
	[Hero_Rally_XP] [int] NOT NULL,
	[Hero_Rampage_XP] [int] NOT NULL,
	[Hero_Ravenor_XP] [int] NOT NULL,
	[Hero_Ra_XP] [int] NOT NULL,
	[Hero_Revenant_XP] [int] NOT NULL,
	[Hero_Rhapsody_XP] [int] NOT NULL,
	[Hero_Riftmage_XP] [int] NOT NULL,
	[Hero_Riptide_XP] [int] NOT NULL,
	[Hero_Rocky_XP] [int] NOT NULL,
	[Hero_Salomon_XP] [int] NOT NULL,
	[Hero_SandWraith_XP] [int] NOT NULL,
	[Hero_Sapphire_XP] [int] NOT NULL,
	[Hero_Scar_XP] [int] NOT NULL,
	[Hero_Scout_XP] [int] NOT NULL,
	[Hero_ShadowBlade_XP] [int] NOT NULL,
	[Hero_Shaman_XP] [int] NOT NULL,
	[Hero_Shellshock_XP] [int] NOT NULL,
	[Hero_Silhouette_XP] [int] NOT NULL,
	[Hero_SirBenzington_XP] [int] NOT NULL,
	[Hero_Skrap_XP] [int] NOT NULL,
	[Hero_Solstice_XP] [int] NOT NULL,
	[Hero_Soulstealer_XP] [int] NOT NULL,
	[Hero_Succubis_XP] [int] NOT NULL,
	[Hero_Taint_XP] [int] NOT NULL,
	[Hero_Tarot_XP] [int] NOT NULL,
	[Hero_Tempest_XP] [int] NOT NULL,
	[Hero_Treant_XP] [int] NOT NULL,
	[Hero_Tremble_XP] [int] NOT NULL,
	[Hero_Tundra_XP] [int] NOT NULL,
	[Hero_Valkyrie_XP] [int] NOT NULL,
	[Hero_Vanya_XP] [int] NOT NULL,
	[Hero_Vindicator_XP] [int] NOT NULL,
	[Hero_Voodoo_XP] [int] NOT NULL,
	[Hero_Warchief_XP] [int] NOT NULL,
	[Hero_WitchSlayer_XP] [int] NOT NULL,
	[Hero_WolfMan_XP] [int] NOT NULL,
	[Hero_Xalynx_XP] [int] NOT NULL,
	[Hero_Yogi_XP] [int] NOT NULL,
	[Hero_Zephyr_XP] [int] NOT NULL,
 CONSTRAINT [PK_Masteries] PRIMARY KEY CLUSTERED 
(
	[MasteryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasteryRewards]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasteryRewards](
	[MasteryRewardsId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Obtained_0001] [bit] NOT NULL,
	[Obtained_0002] [bit] NOT NULL,
	[Obtained_0003] [bit] NOT NULL,
	[Obtained_0005] [bit] NOT NULL,
	[Obtained_0007] [bit] NOT NULL,
	[Obtained_0010] [bit] NOT NULL,
	[Obtained_0015] [bit] NOT NULL,
	[Obtained_0020] [bit] NOT NULL,
	[Obtained_0025] [bit] NOT NULL,
	[Obtained_0030] [bit] NOT NULL,
	[Obtained_0035] [bit] NOT NULL,
	[Obtained_0040] [bit] NOT NULL,
	[Obtained_0045] [bit] NOT NULL,
	[Obtained_0050] [bit] NOT NULL,
	[Obtained_0055] [bit] NOT NULL,
	[Obtained_0060] [bit] NOT NULL,
	[Obtained_0065] [bit] NOT NULL,
	[Obtained_0070] [bit] NOT NULL,
	[Obtained_0075] [bit] NOT NULL,
	[Obtained_0080] [bit] NOT NULL,
	[Obtained_0085] [bit] NOT NULL,
	[Obtained_0090] [bit] NOT NULL,
	[Obtained_0095] [bit] NOT NULL,
	[Obtained_0100] [bit] NOT NULL,
	[Obtained_0105] [bit] NOT NULL,
	[Obtained_0110] [bit] NOT NULL,
	[Obtained_0115] [bit] NOT NULL,
	[Obtained_0120] [bit] NOT NULL,
	[Obtained_0125] [bit] NOT NULL,
	[Obtained_0130] [bit] NOT NULL,
	[Obtained_0135] [bit] NOT NULL,
	[Obtained_0140] [bit] NOT NULL,
	[Obtained_0145] [bit] NOT NULL,
	[Obtained_0150] [bit] NOT NULL,
	[Obtained_0155] [bit] NOT NULL,
	[Obtained_0160] [bit] NOT NULL,
	[Obtained_0165] [bit] NOT NULL,
	[Obtained_0170] [bit] NOT NULL,
	[Obtained_0175] [bit] NOT NULL,
	[Obtained_0180] [bit] NOT NULL,
	[Obtained_0185] [bit] NOT NULL,
	[Obtained_0190] [bit] NOT NULL,
	[Obtained_0195] [bit] NOT NULL,
	[Obtained_0200] [bit] NOT NULL,
	[Obtained_0205] [bit] NOT NULL,
	[Obtained_0210] [bit] NOT NULL,
	[Obtained_0215] [bit] NOT NULL,
	[Obtained_0220] [bit] NOT NULL,
	[Obtained_0225] [bit] NOT NULL,
	[Obtained_0230] [bit] NOT NULL,
	[Obtained_0235] [bit] NOT NULL,
	[Obtained_0240] [bit] NOT NULL,
	[Obtained_0245] [bit] NOT NULL,
	[Obtained_0250] [bit] NOT NULL,
	[Obtained_0255] [bit] NOT NULL,
	[Obtained_0260] [bit] NOT NULL,
	[Obtained_0265] [bit] NOT NULL,
	[Obtained_0270] [bit] NOT NULL,
	[Obtained_0275] [bit] NOT NULL,
	[Obtained_0280] [bit] NOT NULL,
	[Obtained_0285] [bit] NOT NULL,
	[Obtained_0290] [bit] NOT NULL,
	[Obtained_0295] [bit] NOT NULL,
	[Obtained_0300] [bit] NOT NULL,
	[Obtained_0305] [bit] NOT NULL,
	[Obtained_0310] [bit] NOT NULL,
	[Obtained_0315] [bit] NOT NULL,
	[Obtained_0320] [bit] NOT NULL,
	[Obtained_0325] [bit] NOT NULL,
	[Obtained_0330] [bit] NOT NULL,
	[Obtained_0335] [bit] NOT NULL,
	[Obtained_0340] [bit] NOT NULL,
	[Obtained_0345] [bit] NOT NULL,
	[Obtained_0350] [bit] NOT NULL,
	[Obtained_0355] [bit] NOT NULL,
	[Obtained_0360] [bit] NOT NULL,
	[Obtained_0365] [bit] NOT NULL,
	[Obtained_0370] [bit] NOT NULL,
	[Obtained_0375] [bit] NOT NULL,
	[Obtained_0380] [bit] NOT NULL,
	[Obtained_0385] [bit] NOT NULL,
	[Obtained_0390] [bit] NOT NULL,
	[Obtained_0395] [bit] NOT NULL,
	[Obtained_0400] [bit] NOT NULL,
	[Obtained_0405] [bit] NOT NULL,
	[Obtained_0410] [bit] NOT NULL,
	[Obtained_0415] [bit] NOT NULL,
	[Obtained_0420] [bit] NOT NULL,
	[Obtained_0425] [bit] NOT NULL,
	[Obtained_0430] [bit] NOT NULL,
	[Obtained_0435] [bit] NOT NULL,
	[Obtained_0440] [bit] NOT NULL,
	[Obtained_0445] [bit] NOT NULL,
	[Obtained_0450] [bit] NOT NULL,
	[Obtained_0455] [bit] NOT NULL,
	[Obtained_0460] [bit] NOT NULL,
	[Obtained_0465] [bit] NOT NULL,
	[Obtained_0470] [bit] NOT NULL,
	[Obtained_0475] [bit] NOT NULL,
	[Obtained_0480] [bit] NOT NULL,
	[Obtained_0485] [bit] NOT NULL,
	[Obtained_0490] [bit] NOT NULL,
	[Obtained_0495] [bit] NOT NULL,
	[Obtained_0500] [bit] NOT NULL,
	[Obtained_0505] [bit] NOT NULL,
	[Obtained_0510] [bit] NOT NULL,
	[Obtained_0515] [bit] NOT NULL,
	[Obtained_0520] [bit] NOT NULL,
	[Obtained_0525] [bit] NOT NULL,
	[Obtained_0530] [bit] NOT NULL,
	[Obtained_0535] [bit] NOT NULL,
	[Obtained_0540] [bit] NOT NULL,
	[Obtained_0545] [bit] NOT NULL,
	[Obtained_0550] [bit] NOT NULL,
	[Obtained_0555] [bit] NOT NULL,
	[Obtained_0560] [bit] NOT NULL,
	[Obtained_0565] [bit] NOT NULL,
	[Obtained_0570] [bit] NOT NULL,
	[Obtained_0575] [bit] NOT NULL,
	[Obtained_0580] [bit] NOT NULL,
	[Obtained_0585] [bit] NOT NULL,
	[Obtained_0590] [bit] NOT NULL,
	[Obtained_0595] [bit] NOT NULL,
	[Obtained_0600] [bit] NOT NULL,
	[Obtained_0605] [bit] NOT NULL,
	[Obtained_0610] [bit] NOT NULL,
	[Obtained_0615] [bit] NOT NULL,
	[Obtained_0620] [bit] NOT NULL,
	[Obtained_0625] [bit] NOT NULL,
	[Obtained_0630] [bit] NOT NULL,
	[Obtained_0635] [bit] NOT NULL,
	[Obtained_0640] [bit] NOT NULL,
	[Obtained_0645] [bit] NOT NULL,
	[Obtained_0650] [bit] NOT NULL,
	[Obtained_0655] [bit] NOT NULL,
	[Obtained_0660] [bit] NOT NULL,
	[Obtained_0665] [bit] NOT NULL,
	[Obtained_0670] [bit] NOT NULL,
	[Obtained_0675] [bit] NOT NULL,
	[Obtained_0680] [bit] NOT NULL,
	[Obtained_0685] [bit] NOT NULL,
	[Obtained_0690] [bit] NOT NULL,
	[Obtained_0695] [bit] NOT NULL,
	[Obtained_0700] [bit] NOT NULL,
	[Obtained_0705] [bit] NOT NULL,
	[Obtained_0710] [bit] NOT NULL,
	[Obtained_0715] [bit] NOT NULL,
	[Obtained_0720] [bit] NOT NULL,
	[Obtained_0725] [bit] NOT NULL,
	[Obtained_0730] [bit] NOT NULL,
	[Obtained_0735] [bit] NOT NULL,
	[Obtained_0740] [bit] NOT NULL,
	[Obtained_0745] [bit] NOT NULL,
	[Obtained_0750] [bit] NOT NULL,
	[Obtained_0755] [bit] NOT NULL,
	[Obtained_0760] [bit] NOT NULL,
	[Obtained_0765] [bit] NOT NULL,
	[Obtained_0770] [bit] NOT NULL,
	[Obtained_0775] [bit] NOT NULL,
	[Obtained_0780] [bit] NOT NULL,
	[Obtained_0785] [bit] NOT NULL,
	[Obtained_0790] [bit] NOT NULL,
	[Obtained_0795] [bit] NOT NULL,
	[Obtained_0800] [bit] NOT NULL,
	[Obtained_0805] [bit] NOT NULL,
	[Obtained_0810] [bit] NOT NULL,
	[Obtained_0815] [bit] NOT NULL,
	[Obtained_0820] [bit] NOT NULL,
	[Obtained_0825] [bit] NOT NULL,
	[Obtained_0830] [bit] NOT NULL,
	[Obtained_0835] [bit] NOT NULL,
	[Obtained_0840] [bit] NOT NULL,
	[Obtained_0845] [bit] NOT NULL,
	[Obtained_0850] [bit] NOT NULL,
	[Obtained_0855] [bit] NOT NULL,
	[Obtained_0860] [bit] NOT NULL,
	[Obtained_0865] [bit] NOT NULL,
	[Obtained_0870] [bit] NOT NULL,
	[Obtained_0875] [bit] NOT NULL,
	[Obtained_0880] [bit] NOT NULL,
	[Obtained_0885] [bit] NOT NULL,
	[Obtained_0890] [bit] NOT NULL,
	[Obtained_0895] [bit] NOT NULL,
	[Obtained_0900] [bit] NOT NULL,
	[Obtained_0905] [bit] NOT NULL,
	[Obtained_0910] [bit] NOT NULL,
	[Obtained_0915] [bit] NOT NULL,
	[Obtained_0920] [bit] NOT NULL,
	[Obtained_0925] [bit] NOT NULL,
	[Obtained_0930] [bit] NOT NULL,
	[Obtained_0935] [bit] NOT NULL,
	[Obtained_0940] [bit] NOT NULL,
	[Obtained_0945] [bit] NOT NULL,
	[Obtained_0950] [bit] NOT NULL,
	[Obtained_0955] [bit] NOT NULL,
	[Obtained_0960] [bit] NOT NULL,
	[Obtained_0965] [bit] NOT NULL,
	[Obtained_0970] [bit] NOT NULL,
	[Obtained_0975] [bit] NOT NULL,
	[Obtained_0980] [bit] NOT NULL,
	[Obtained_0985] [bit] NOT NULL,
	[Obtained_0990] [bit] NOT NULL,
	[Obtained_0995] [bit] NOT NULL,
	[Obtained_1000] [bit] NOT NULL,
	[Obtained_1005] [bit] NOT NULL,
	[Obtained_1010] [bit] NOT NULL,
	[Obtained_1015] [bit] NOT NULL,
	[Obtained_1020] [bit] NOT NULL,
	[Obtained_1025] [bit] NOT NULL,
	[Obtained_1030] [bit] NOT NULL,
	[Obtained_1035] [bit] NOT NULL,
	[Obtained_1040] [bit] NOT NULL,
	[Obtained_1045] [bit] NOT NULL,
	[Obtained_1050] [bit] NOT NULL,
	[Obtained_1055] [bit] NOT NULL,
	[Obtained_1060] [bit] NOT NULL,
	[Obtained_1065] [bit] NOT NULL,
	[Obtained_1070] [bit] NOT NULL,
	[Obtained_1075] [bit] NOT NULL,
	[Obtained_1080] [bit] NOT NULL,
	[Obtained_1085] [bit] NOT NULL,
	[Obtained_1090] [bit] NOT NULL,
	[Obtained_1095] [bit] NOT NULL,
	[Obtained_1100] [bit] NOT NULL,
	[Obtained_1105] [bit] NOT NULL,
	[Obtained_1110] [bit] NOT NULL,
	[Obtained_1115] [bit] NOT NULL,
	[Obtained_1120] [bit] NOT NULL,
	[Obtained_1125] [bit] NOT NULL,
	[Obtained_1130] [bit] NOT NULL,
	[Obtained_1135] [bit] NOT NULL,
	[Obtained_1140] [bit] NOT NULL,
	[Obtained_1145] [bit] NOT NULL,
	[Obtained_1150] [bit] NOT NULL,
	[Obtained_1155] [bit] NOT NULL,
	[Obtained_1160] [bit] NOT NULL,
	[Obtained_1165] [bit] NOT NULL,
	[Obtained_1170] [bit] NOT NULL,
	[Obtained_1175] [bit] NOT NULL,
	[Obtained_1180] [bit] NOT NULL,
	[Obtained_1185] [bit] NOT NULL,
	[Obtained_1190] [bit] NOT NULL,
	[Obtained_1195] [bit] NOT NULL,
	[Obtained_1200] [bit] NOT NULL,
	[Obtained_1205] [bit] NOT NULL,
	[Obtained_1210] [bit] NOT NULL,
	[Obtained_1215] [bit] NOT NULL,
	[Obtained_1220] [bit] NOT NULL,
	[Obtained_1225] [bit] NOT NULL,
	[Obtained_1230] [bit] NOT NULL,
	[Obtained_1235] [bit] NOT NULL,
	[Obtained_1240] [bit] NOT NULL,
	[Obtained_1245] [bit] NOT NULL,
	[Obtained_1250] [bit] NOT NULL,
	[Obtained_1255] [bit] NOT NULL,
	[Obtained_1260] [bit] NOT NULL,
	[Obtained_1265] [bit] NOT NULL,
	[Obtained_1270] [bit] NOT NULL,
	[Obtained_1275] [bit] NOT NULL,
	[Obtained_1280] [bit] NOT NULL,
	[Obtained_1285] [bit] NOT NULL,
	[Obtained_1290] [bit] NOT NULL,
	[Obtained_1295] [bit] NOT NULL,
	[Obtained_1300] [bit] NOT NULL,
	[Obtained_1305] [bit] NOT NULL,
	[Obtained_1310] [bit] NOT NULL,
	[Obtained_1315] [bit] NOT NULL,
	[Obtained_1320] [bit] NOT NULL,
	[Obtained_1325] [bit] NOT NULL,
	[Obtained_1330] [bit] NOT NULL,
	[Obtained_1335] [bit] NOT NULL,
	[Obtained_1340] [bit] NOT NULL,
	[Obtained_1345] [bit] NOT NULL,
	[Obtained_1350] [bit] NOT NULL,
	[Obtained_1355] [bit] NOT NULL,
	[Obtained_1360] [bit] NOT NULL,
	[Obtained_1365] [bit] NOT NULL,
	[Obtained_1370] [bit] NOT NULL,
	[Obtained_1375] [bit] NOT NULL,
	[Obtained_1380] [bit] NOT NULL,
	[Obtained_1385] [bit] NOT NULL,
	[Obtained_1390] [bit] NOT NULL,
	[Obtained_1395] [bit] NOT NULL,
	[Obtained_1400] [bit] NOT NULL,
	[Obtained_1405] [bit] NOT NULL,
	[Obtained_1410] [bit] NOT NULL,
	[Obtained_1415] [bit] NOT NULL,
	[Obtained_1420] [bit] NOT NULL,
	[Obtained_1425] [bit] NOT NULL,
	[Obtained_1430] [bit] NOT NULL,
	[Obtained_1435] [bit] NOT NULL,
	[Obtained_1440] [bit] NOT NULL,
	[Obtained_1445] [bit] NOT NULL,
	[Obtained_1450] [bit] NOT NULL,
	[Obtained_1455] [bit] NOT NULL,
	[Obtained_1460] [bit] NOT NULL,
	[Obtained_1465] [bit] NOT NULL,
	[Obtained_1470] [bit] NOT NULL,
	[Obtained_1475] [bit] NOT NULL,
	[Obtained_1480] [bit] NOT NULL,
	[Obtained_1485] [bit] NOT NULL,
	[Obtained_1490] [bit] NOT NULL,
	[Obtained_1495] [bit] NOT NULL,
	[Obtained_1500] [bit] NOT NULL,
	[Obtained_1505] [bit] NOT NULL,
	[Obtained_1510] [bit] NOT NULL,
	[Obtained_1515] [bit] NOT NULL,
	[Obtained_1520] [bit] NOT NULL,
	[Obtained_1525] [bit] NOT NULL,
	[Obtained_1530] [bit] NOT NULL,
	[Obtained_1535] [bit] NOT NULL,
	[Obtained_1540] [bit] NOT NULL,
	[Obtained_1545] [bit] NOT NULL,
	[Obtained_1550] [bit] NOT NULL,
	[Obtained_1555] [bit] NOT NULL,
	[Obtained_1560] [bit] NOT NULL,
	[Obtained_1565] [bit] NOT NULL,
	[Obtained_1570] [bit] NOT NULL,
	[Obtained_1575] [bit] NOT NULL,
	[Obtained_1580] [bit] NOT NULL,
	[Obtained_1585] [bit] NOT NULL,
	[Obtained_1590] [bit] NOT NULL,
	[Obtained_1595] [bit] NOT NULL,
	[Obtained_1600] [bit] NOT NULL,
	[Obtained_1605] [bit] NOT NULL,
	[Obtained_1610] [bit] NOT NULL,
	[Obtained_1615] [bit] NOT NULL,
	[Obtained_1620] [bit] NOT NULL,
	[Obtained_1625] [bit] NOT NULL,
	[Obtained_1630] [bit] NOT NULL,
	[Obtained_1635] [bit] NOT NULL,
	[Obtained_1640] [bit] NOT NULL,
	[Obtained_1645] [bit] NOT NULL,
	[Obtained_1650] [bit] NOT NULL,
	[Obtained_1655] [bit] NOT NULL,
	[Obtained_1660] [bit] NOT NULL,
	[Obtained_1665] [bit] NOT NULL,
	[Obtained_1670] [bit] NOT NULL,
	[Obtained_1675] [bit] NOT NULL,
	[Obtained_1680] [bit] NOT NULL,
	[Obtained_1685] [bit] NOT NULL,
	[Obtained_1690] [bit] NOT NULL,
	[Obtained_1695] [bit] NOT NULL,
	[Obtained_1700] [bit] NOT NULL,
	[Obtained_1705] [bit] NOT NULL,
	[Obtained_1710] [bit] NOT NULL,
	[Obtained_1715] [bit] NOT NULL,
	[Obtained_1720] [bit] NOT NULL,
	[Obtained_1725] [bit] NOT NULL,
	[Obtained_1730] [bit] NOT NULL,
	[Obtained_1735] [bit] NOT NULL,
	[Obtained_1740] [bit] NOT NULL,
	[Obtained_1745] [bit] NOT NULL,
	[Obtained_1750] [bit] NOT NULL,
	[Obtained_1755] [bit] NOT NULL,
	[Obtained_1760] [bit] NOT NULL,
	[Obtained_1765] [bit] NOT NULL,
	[Obtained_1770] [bit] NOT NULL,
	[Obtained_1775] [bit] NOT NULL,
	[Obtained_1780] [bit] NOT NULL,
	[Obtained_1785] [bit] NOT NULL,
	[Obtained_1790] [bit] NOT NULL,
	[Obtained_1795] [bit] NOT NULL,
	[Obtained_1800] [bit] NOT NULL,
	[Obtained_1805] [bit] NOT NULL,
	[Obtained_1810] [bit] NOT NULL,
	[Obtained_1815] [bit] NOT NULL,
	[Obtained_1820] [bit] NOT NULL,
	[Obtained_1825] [bit] NOT NULL,
	[Obtained_1830] [bit] NOT NULL,
	[Obtained_1835] [bit] NOT NULL,
	[Obtained_1840] [bit] NOT NULL,
	[Obtained_1845] [bit] NOT NULL,
	[Obtained_1850] [bit] NOT NULL,
	[Obtained_1855] [bit] NOT NULL,
	[Obtained_1860] [bit] NOT NULL,
	[Obtained_1865] [bit] NOT NULL,
	[Obtained_1870] [bit] NOT NULL,
	[Obtained_1875] [bit] NOT NULL,
	[Obtained_1880] [bit] NOT NULL,
	[Obtained_1885] [bit] NOT NULL,
	[Obtained_1890] [bit] NOT NULL,
	[Obtained_1895] [bit] NOT NULL,
	[Obtained_1900] [bit] NOT NULL,
	[Obtained_1905] [bit] NOT NULL,
	[Obtained_1910] [bit] NOT NULL,
	[Obtained_1915] [bit] NOT NULL,
	[Obtained_1920] [bit] NOT NULL,
	[Obtained_1925] [bit] NOT NULL,
	[Obtained_1930] [bit] NOT NULL,
	[Obtained_1935] [bit] NOT NULL,
	[Obtained_1940] [bit] NOT NULL,
	[Obtained_1945] [bit] NOT NULL,
	[Obtained_1950] [bit] NOT NULL,
	[Obtained_1955] [bit] NOT NULL,
	[Obtained_1960] [bit] NOT NULL,
	[Obtained_1965] [bit] NOT NULL,
	[Obtained_1970] [bit] NOT NULL,
	[Obtained_1975] [bit] NOT NULL,
	[Obtained_1980] [bit] NOT NULL,
	[Obtained_1985] [bit] NOT NULL,
	[Obtained_1990] [bit] NOT NULL,
	[Obtained_1995] [bit] NOT NULL,
	[Obtained_2000] [bit] NOT NULL,
	[Obtained_2005] [bit] NOT NULL,
	[Obtained_2010] [bit] NOT NULL,
	[Obtained_2015] [bit] NOT NULL,
	[Obtained_2020] [bit] NOT NULL,
	[Obtained_2025] [bit] NOT NULL,
	[Obtained_2030] [bit] NOT NULL,
	[Obtained_2035] [bit] NOT NULL,
	[Obtained_2040] [bit] NOT NULL,
	[Obtained_2045] [bit] NOT NULL,
	[Obtained_2050] [bit] NOT NULL,
	[Obtained_2055] [bit] NOT NULL,
	[Obtained_2060] [bit] NOT NULL,
	[Obtained_2065] [bit] NOT NULL,
	[Obtained_2070] [bit] NOT NULL,
	[Obtained_2075] [bit] NOT NULL,
	[Obtained_2080] [bit] NOT NULL,
	[Obtained_2085] [bit] NOT NULL,
 CONSTRAINT [PK_MasteryRewards] PRIMARY KEY CLUSTERED 
(
	[MasteryRewardsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MatchResults]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchResults](
	[match_id] [int] IDENTITY(1,1) NOT NULL,
	[server_id] [bigint] NOT NULL,
	[map] [nvarchar](max) NULL,
	[map_version] [nvarchar](max) NULL,
	[time_played] [int] NOT NULL,
	[file_size] [int] NOT NULL,
	[file_name] [nvarchar](max) NULL,
	[c_state] [int] NOT NULL,
	[version] [nvarchar](max) NULL,
	[avgpsr] [int] NOT NULL,
	[avgpsr_team1] [int] NOT NULL,
	[avgpsr_team2] [int] NOT NULL,
	[gamemode] [nvarchar](max) NULL,
	[teamscoregoal] [int] NOT NULL,
	[playerscoregoal] [int] NOT NULL,
	[numrounds] [int] NOT NULL,
	[release_stage] [nvarchar](max) NULL,
	[banned_heroes] [nvarchar](max) NULL,
	[awd_mann] [int] NOT NULL,
	[awd_mqk] [int] NOT NULL,
	[awd_lgks] [int] NOT NULL,
	[awd_msd] [int] NOT NULL,
	[awd_mkill] [int] NOT NULL,
	[awd_masst] [int] NOT NULL,
	[awd_ledth] [int] NOT NULL,
	[awd_mbdmg] [int] NOT NULL,
	[awd_mwk] [int] NOT NULL,
	[awd_mhdd] [int] NOT NULL,
	[awd_hcs] [int] NOT NULL,
	[mvp] [int] NOT NULL,
	[submission_debug] [nvarchar](max) NULL,
	[date] [nvarchar](max) NULL,
	[time] [nvarchar](max) NULL,
	[inventory] [nvarchar](max) NULL,
	[timestamp] [bigint] NOT NULL,
 CONSTRAINT [PK_MatchResults] PRIMARY KEY CLUSTERED 
(
	[match_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NicknameChanges]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NicknameChanges](
	[NicknameChangeId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[CurrentUserName] [nvarchar](max) NOT NULL,
	[PastUserNames] [nvarchar](max) NOT NULL,
	[NicknameChangeDates] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_NicknameChanges] PRIMARY KEY CLUSTERED 
(
	[NicknameChangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[AccountId] [int] NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PerformanceEntries]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Subcategory] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Duration] [bigint] NOT NULL,
	[BeforeProcessDuration] [bigint] NOT NULL,
	[ProcessDuration] [bigint] NOT NULL,
	[AfterProcessDuration] [bigint] NOT NULL,
	[TimesCalled] [int] NOT NULL,
 CONSTRAINT [PK_PerformanceEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerMatchResults]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerMatchResults](
	[nickname] [nvarchar](450) NOT NULL,
	[match_id] [int] NOT NULL,
	[clan_tag] [nvarchar](max) NULL,
	[clan_id] [int] NOT NULL,
	[team] [int] NOT NULL,
	[position] [int] NOT NULL,
	[group_num] [int] NOT NULL,
	[benefit] [int] NOT NULL,
	[hero_id] [bigint] NOT NULL,
	[wins] [int] NOT NULL,
	[losses] [int] NOT NULL,
	[discos] [int] NOT NULL,
	[concedes] [int] NOT NULL,
	[kicked] [int] NOT NULL,
	[social_bonus] [int] NOT NULL,
	[used_token] [int] NOT NULL,
	[pub_skill] [float] NOT NULL,
	[pub_count] [int] NOT NULL,
	[amm_team_rating] [float] NOT NULL,
	[amm_team_count] [int] NOT NULL,
	[concedevotes] [int] NOT NULL,
	[herokills] [int] NOT NULL,
	[herodmg] [int] NOT NULL,
	[herokillsgold] [int] NOT NULL,
	[heroassists] [int] NOT NULL,
	[heroexp] [int] NOT NULL,
	[deaths] [int] NOT NULL,
	[buybacks] [int] NOT NULL,
	[goldlost2death] [int] NOT NULL,
	[secs_dead] [int] NOT NULL,
	[teamcreepkills] [int] NOT NULL,
	[teamcreepdmg] [int] NOT NULL,
	[teamcreepgold] [int] NOT NULL,
	[teamcreepexp] [int] NOT NULL,
	[neutralcreepkills] [int] NOT NULL,
	[neutralcreepdmg] [int] NOT NULL,
	[neutralcreepgold] [int] NOT NULL,
	[neutralcreepexp] [int] NOT NULL,
	[bdmg] [int] NOT NULL,
	[razed] [int] NOT NULL,
	[bdmgexp] [int] NOT NULL,
	[bgold] [int] NOT NULL,
	[denies] [int] NOT NULL,
	[exp_denied] [int] NOT NULL,
	[gold] [int] NOT NULL,
	[gold_spent] [int] NOT NULL,
	[exp] [int] NOT NULL,
	[actions] [int] NOT NULL,
	[secs] [int] NOT NULL,
	[level] [int] NOT NULL,
	[consumables] [int] NOT NULL,
	[wards] [int] NOT NULL,
	[bloodlust] [int] NOT NULL,
	[doublekill] [int] NOT NULL,
	[triplekill] [int] NOT NULL,
	[quadkill] [int] NOT NULL,
	[annihilation] [int] NOT NULL,
	[ks3] [int] NOT NULL,
	[ks4] [int] NOT NULL,
	[ks5] [int] NOT NULL,
	[ks6] [int] NOT NULL,
	[ks7] [int] NOT NULL,
	[ks8] [int] NOT NULL,
	[ks9] [int] NOT NULL,
	[ks10] [int] NOT NULL,
	[ks15] [int] NOT NULL,
	[smackdown] [int] NOT NULL,
	[humiliation] [int] NOT NULL,
	[nemesis] [int] NOT NULL,
	[retribution] [int] NOT NULL,
	[score] [int] NOT NULL,
	[gameplaystat0] [float] NOT NULL,
	[gameplaystat1] [float] NOT NULL,
	[gameplaystat2] [float] NOT NULL,
	[gameplaystat3] [float] NOT NULL,
	[gameplaystat4] [float] NOT NULL,
	[gameplaystat5] [float] NOT NULL,
	[gameplaystat6] [float] NOT NULL,
	[gameplaystat7] [float] NOT NULL,
	[gameplaystat8] [float] NOT NULL,
	[gameplaystat9] [float] NOT NULL,
	[time_earning_exp] [int] NOT NULL,
	[account_id] [int] NULL,
	[map] [nvarchar](max) NULL,
	[cli_name] [nvarchar](max) NULL,
	[mdt] [nvarchar](max) NULL,
 CONSTRAINT [PK_PlayerMatchResults] PRIMARY KEY CLUSTERED 
(
	[match_id] ASC,
	[nickname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerSeasonStatsGrimmsCrossing]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSeasonStatsGrimmsCrossing](
	[PlayerSeasonStatsGrimmsCrossingId] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_PlayerSeasonStatsGrimmsCrossing] PRIMARY KEY CLUSTERED 
(
	[PlayerSeasonStatsGrimmsCrossingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerSeasonStatsMidWars]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSeasonStatsMidWars](
	[PlayerSeasonStatsMidWarsId] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_PlayerSeasonStatsMidWars] PRIMARY KEY CLUSTERED 
(
	[PlayerSeasonStatsMidWarsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerSeasonStatsPublic]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSeasonStatsPublic](
	[PlayerSeasonStatsPublicId] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_PlayerSeasonStatsPublic] PRIMARY KEY CLUSTERED 
(
	[PlayerSeasonStatsPublicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerSeasonStatsRanked]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSeasonStatsRanked](
	[PlayerSeasonStatsRankedId] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_PlayerSeasonStatsRanked] PRIMARY KEY CLUSTERED 
(
	[PlayerSeasonStatsRankedId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerSeasonStatsRankedCasual]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSeasonStatsRankedCasual](
	[PlayerSeasonStatsRankedCasualId] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [real] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Concedes] [int] NOT NULL,
	[ConcedeVotes] [int] NOT NULL,
	[Buybacks] [int] NOT NULL,
	[TimesDisconnected] [int] NOT NULL,
	[TimesKicked] [int] NOT NULL,
	[HeroKills] [int] NOT NULL,
	[HeroDamage] [int] NOT NULL,
	[HeroExp] [int] NOT NULL,
	[HeroKillsGold] [int] NOT NULL,
	[HeroAssists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[GoldLost2Death] [int] NOT NULL,
	[SecsDead] [int] NOT NULL,
	[TeamCreepKills] [int] NOT NULL,
	[TeamCreepDmg] [int] NOT NULL,
	[TeamCreepExp] [int] NOT NULL,
	[TeamCreepGold] [int] NOT NULL,
	[NeutralCreepKills] [int] NOT NULL,
	[NeutralCreepDmg] [int] NOT NULL,
	[NeutralCreepExp] [int] NOT NULL,
	[NeutralCreepGold] [int] NOT NULL,
	[BDmg] [int] NOT NULL,
	[BDmgExp] [int] NOT NULL,
	[Razed] [int] NOT NULL,
	[BGold] [int] NOT NULL,
	[Denies] [int] NOT NULL,
	[ExpDenies] [int] NOT NULL,
	[Gold] [int] NOT NULL,
	[GoldSpent] [int] NOT NULL,
	[Exp] [int] NOT NULL,
	[Actions] [int] NOT NULL,
	[Secs] [int] NOT NULL,
	[Consumables] [int] NOT NULL,
	[Wards] [int] NOT NULL,
	[EmPlayed] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelExp] [int] NOT NULL,
	[MinExp] [int] NOT NULL,
	[MaxExp] [int] NOT NULL,
	[TimeEarningExp] [int] NOT NULL,
	[Bloodlust] [int] NOT NULL,
	[DoubleKill] [int] NOT NULL,
	[TrippleKill] [int] NOT NULL,
	[QuadKill] [int] NOT NULL,
	[Annihilation] [int] NOT NULL,
	[Ks3] [int] NOT NULL,
	[Ks4] [int] NOT NULL,
	[Ks5] [int] NOT NULL,
	[Ks6] [int] NOT NULL,
	[Ks7] [int] NOT NULL,
	[Ks8] [int] NOT NULL,
	[Ks9] [int] NOT NULL,
	[Ks10] [int] NOT NULL,
	[Ks15] [int] NOT NULL,
	[Smackdown] [int] NOT NULL,
	[Humiliation] [int] NOT NULL,
	[Nemesis] [int] NOT NULL,
	[Retribution] [int] NOT NULL,
	[WinStreak] [int] NOT NULL,
	[SerializedMatchIds] [nvarchar](max) NOT NULL,
	[SerializedMatchDates] [nvarchar](max) NOT NULL,
	[PlayerAwardSummary_TopAnnihilations] [int] NOT NULL,
	[PlayerAwardSummary_MostQuadKills] [int] NOT NULL,
	[PlayerAwardSummary_BestKillStreak] [int] NOT NULL,
	[PlayerAwardSummary_MostSmackdowns] [int] NOT NULL,
	[PlayerAwardSummary_MostKills] [int] NOT NULL,
	[PlayerAwardSummary_MostAssists] [int] NOT NULL,
	[PlayerAwardSummary_LeastDeaths] [int] NOT NULL,
	[PlayerAwardSummary_TopSiegeDamage] [int] NOT NULL,
	[PlayerAwardSummary_MostWardsKilled] [int] NOT NULL,
	[PlayerAwardSummary_TopHeroDamage] [int] NOT NULL,
	[PlayerAwardSummary_TopCreepScore] [int] NOT NULL,
	[PlayerAwardSummary_MVP] [int] NOT NULL,
	[SerializedHeroUsage] [nvarchar](max) NOT NULL,
	[PlacementMatchesDetails] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_PlayerSeasonStatsRankedCasual] PRIMARY KEY CLUSTERED 
(
	[PlayerSeasonStatsRankedCasualId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffMemberActions]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffMemberActions](
	[StaffMemberActionId] [int] IDENTITY(1,1) NOT NULL,
	[SourceId] [uniqueidentifier] NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
	[Details] [nvarchar](max) NOT NULL,
	[TargetId] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_StaffMemberActions] PRIMARY KEY CLUSTERED 
(
	[StaffMemberActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suspensions]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suspensions](
	[SuspensionId] [int] IDENTITY(1,1) NOT NULL,
	[OffenderAccountId] [int] NOT NULL,
	[SuspendByMask] [int] NOT NULL,
	[StartTimestamp] [datetime2](7) NOT NULL,
	[EndTimestamp] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[IssuerAccountId] [int] NOT NULL,
	[InternalDetails] [nvarchar](max) NOT NULL,
	[Override] [bit] NOT NULL,
	[BaseEndTimestamp] [datetime2](7) NOT NULL,
	[Multiplier] [real] NOT NULL,
 CONSTRAINT [PK_Suspensions] PRIMARY KEY CLUSTERED 
(
	[SuspensionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tokens]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tokens](
	[Value] [uniqueidentifier] NOT NULL,
	[Purpose] [int] NOT NULL,
	[EmailAddress] [nvarchar](max) NOT NULL,
	[SanitisedEmailAddress] [nvarchar](max) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UniqueHardwareSet]    Script Date: 7/25/2023 5:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UniqueHardwareSet](
	[FirstSnapshotHash] [varbinary](64) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[SerializedData] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UniqueHardwareSet] PRIMARY KEY CLUSTERED 
(
	[FirstSnapshotHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (N'') FOR [IgnoredList]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [PlayerSeasonStatsRankedCasualId]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (N'') FOR [HardwareIdCollection]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (N'') FOR [IpAddressCollection]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (N'') FOR [MacAddressCollection]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (N'') FOR [SystemInformationCollection]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [LastPlayedMatchId]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [LastPlayedFromInternetCafe]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [AccountId]
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_StatResetCount]  DEFAULT ((0)) FOR [StatResetCount]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [Tickets]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [TotalExperience]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [TotalLevel]
GO
ALTER TABLE [dbo].[Coupons] ADD  DEFAULT ((0)) FOR [CollectiveId]
GO
ALTER TABLE [dbo].[Coupons] ADD  DEFAULT ((0)) FOR [DonorId]
GO
ALTER TABLE [dbo].[GameServers] ADD  DEFAULT ((0)) FOR [GameServerManagerId]
GO
ALTER TABLE [dbo].[HardwareSnapshots] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [FirstTimeObserved]
GO
ALTER TABLE [dbo].[MatchResults] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [timestamp]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (N'') FOR [Content]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [AccountId]
GO
ALTER TABLE [dbo].[PlayerSeasonStatsMidWars] ADD  DEFAULT (N'') FOR [PlacementMatchesDetails]
GO
ALTER TABLE [dbo].[PlayerSeasonStatsPublic] ADD  DEFAULT (N'') FOR [PlacementMatchesDetails]
GO
ALTER TABLE [dbo].[PlayerSeasonStatsRanked] ADD  DEFAULT (N'') FOR [PlacementMatchesDetails]
GO
ALTER TABLE [dbo].[PlayerSeasonStatsRankedCasual] ADD  DEFAULT (N'') FOR [PlacementMatchesDetails]
GO
ALTER TABLE [dbo].[StaffMemberActions] ADD  DEFAULT (N'') FOR [Details]
GO
ALTER TABLE [dbo].[StaffMemberActions] ADD  DEFAULT (N'') FOR [TargetId]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT ((0)) FOR [OffenderAccountId]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT ((0)) FOR [IssuerAccountId]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT (N'') FOR [InternalDetails]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Override]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [BaseEndTimestamp]
GO
ALTER TABLE [dbo].[Suspensions] ADD  DEFAULT (CONVERT([real],(0))) FOR [Multiplier]
GO
ALTER TABLE [dbo].[Tokens] ADD  DEFAULT (N'') FOR [Data]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Clans_ClanId] FOREIGN KEY([ClanId])
REFERENCES [dbo].[Clans] ([ClanId])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Clans_ClanId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_PlayerSeasonStatsGrimmsCrossing_PlayerSeasonStatsGrimmsCrossingId] FOREIGN KEY([PlayerSeasonStatsGrimmsCrossingId])
REFERENCES [dbo].[PlayerSeasonStatsGrimmsCrossing] ([PlayerSeasonStatsGrimmsCrossingId])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_PlayerSeasonStatsGrimmsCrossing_PlayerSeasonStatsGrimmsCrossingId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_PlayerSeasonStatsMidWars_PlayerSeasonStatsMidWarsId] FOREIGN KEY([PlayerSeasonStatsMidWarsId])
REFERENCES [dbo].[PlayerSeasonStatsMidWars] ([PlayerSeasonStatsMidWarsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_PlayerSeasonStatsMidWars_PlayerSeasonStatsMidWarsId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_PlayerSeasonStatsPublic_PlayerSeasonStatsPublicId] FOREIGN KEY([PlayerSeasonStatsPublicId])
REFERENCES [dbo].[PlayerSeasonStatsPublic] ([PlayerSeasonStatsPublicId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_PlayerSeasonStatsPublic_PlayerSeasonStatsPublicId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_PlayerSeasonStatsRanked_PlayerSeasonStatsRankedId] FOREIGN KEY([PlayerSeasonStatsRankedId])
REFERENCES [dbo].[PlayerSeasonStatsRanked] ([PlayerSeasonStatsRankedId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_PlayerSeasonStatsRanked_PlayerSeasonStatsRankedId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_PlayerSeasonStatsRankedCasual_PlayerSeasonStatsRankedCasualId] FOREIGN KEY([PlayerSeasonStatsRankedCasualId])
REFERENCES [dbo].[PlayerSeasonStatsRankedCasual] ([PlayerSeasonStatsRankedCasualId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_PlayerSeasonStatsRankedCasual_PlayerSeasonStatsRankedCasualId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CloudStorages]  WITH CHECK ADD  CONSTRAINT [FK_CloudStorages_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CloudStorages] CHECK CONSTRAINT [FK_CloudStorages_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Coupons]  WITH CHECK ADD  CONSTRAINT [FK_Coupons_AspNetUsers_ClaimedById] FOREIGN KEY([ClaimedById])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Coupons] CHECK CONSTRAINT [FK_Coupons_AspNetUsers_ClaimedById]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Accounts_FriendAccountId] FOREIGN KEY([FriendAccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Accounts_FriendAccountId]
GO
ALTER TABLE [dbo].[Guides]  WITH CHECK ADD  CONSTRAINT [FK_Guides_Accounts_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Accounts] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Guides] CHECK CONSTRAINT [FK_Guides_Accounts_AuthorId]
GO
ALTER TABLE [dbo].[IncidentReports]  WITH CHECK ADD  CONSTRAINT [FK_IncidentReports_Accounts_AccusedId] FOREIGN KEY([AccusedId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[IncidentReports] CHECK CONSTRAINT [FK_IncidentReports_Accounts_AccusedId]
GO
ALTER TABLE [dbo].[IncidentReports]  WITH CHECK ADD  CONSTRAINT [FK_IncidentReports_Accounts_ReporterId] FOREIGN KEY([ReporterId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[IncidentReports] CHECK CONSTRAINT [FK_IncidentReports_Accounts_ReporterId]
GO
ALTER TABLE [dbo].[IncidentReports]  WITH CHECK ADD  CONSTRAINT [FK_IncidentReports_Accounts_ReviewerId] FOREIGN KEY([ReviewerId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[IncidentReports] CHECK CONSTRAINT [FK_IncidentReports_Accounts_ReviewerId]
GO
ALTER TABLE [dbo].[Masteries]  WITH CHECK ADD  CONSTRAINT [FK_Masteries_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Masteries] CHECK CONSTRAINT [FK_Masteries_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[MasteryRewards]  WITH CHECK ADD  CONSTRAINT [FK_MasteryRewards_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MasteryRewards] CHECK CONSTRAINT [FK_MasteryRewards_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Suspensions]  WITH CHECK ADD  CONSTRAINT [FK_Suspensions_Accounts_IssuerAccountId] FOREIGN KEY([IssuerAccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[Suspensions] CHECK CONSTRAINT [FK_Suspensions_Accounts_IssuerAccountId]
GO
ALTER TABLE [dbo].[Suspensions]  WITH CHECK ADD  CONSTRAINT [FK_Suspensions_Accounts_OffenderAccountId] FOREIGN KEY([OffenderAccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO
ALTER TABLE [dbo].[Suspensions] CHECK CONSTRAINT [FK_Suspensions_Accounts_OffenderAccountId]
GO
