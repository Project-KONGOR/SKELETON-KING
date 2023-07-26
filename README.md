# SKELETON-KING &ensp; ![Github Workflow Status Badge](https://github.com/N-E-W-E-R-T-H/SKELETON-KING/actions/workflows/dotnet.yml/badge.svg)

Modularized HoN masterserver

For more details, see the [wiki](https://github.com/N-E-W-E-R-T-H/SKELETON-KING/wiki).

### Quick start

##### Prerequisites
* [Microsoft SQL Server](https://go.microsoft.com/fwlink/p/?linkid=2215158&clcid=0x409&culture=en-us&country=us)
* [Dotnet 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* [Fork the project](https://github.com/N-E-W-E-R-T-H/SKELETON-KING/fork)

##### Set up the database
1. Create a new database named`BOUNTY`
2. From the directory `SKELETON-KING/SKELETON-KING/` run `dotnet ef database update` from the CLI or `Update-Database` from Visual Studio
