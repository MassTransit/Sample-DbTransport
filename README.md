# MassTransit Database Transport Sample


## Getting Started

The SQL Database Transport is currently in preview only. Support customers can request access to the 
NuGet packages by contacting support and providing a GitHub username that will be given access. 

The customer will also be issued a license file that will activate the transport packages. This file can either be:

1. Placed in a directory and the `MT_LICENSE_PATH` environment set to the full path of the file
2. The license text itself can be stored as an environment variable `MT_LICENSE`

> NOTE the license file is only for enabling the use of these preview packages. They are not required to use the
> publicly available MassTransit NuGet packages.

1. Update the NuGet.config GitHub package source credentials to access the required packages.
   1. Get a personal access token (classic) from GitHub to update the NuGet.config 
   2. Request access to the NuGet Package Repository (via support) 
2. Run `docker compose up --build` to start pgsql.
3. Open the solution.
4. Build it (the NuGet packages should restore)
5. Navigate to `http://localhost:5010/swagger` to interact with the API.

