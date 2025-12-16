# WhatHaveYouPlayed
Short description of functionality:
- User registration, login and role-based access using ASP.NET Core Identity.
- Optional JWT authentication for API scenarios.
- Adding, editing and deleting games in a personal collection
- Tracking game status (e.g., Playing, Completed, Planning, Dropped), rating and playtime
- Browsing the game catalog with filtering and search


Technology used: .NET 6 - Razor Pages (ASP.NET Core)
Project type - MVC 5

Additional libraries used:
- Microsoft.AspNetCore.Authentication.JwtBearer 6.0.36
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 6.0.9
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 6.0.10
- Microsoft.AspNetCore.Identity.UI 6.0.10
- Microsoft.EntityFrameworkCore 6.0.36
- Microsoft.EntityFrameworkCore.SqlServer 6.0.10
- Microsoft.EntityFrameworkCore.Sqlite 6.0.36 (for docker images)
- Microsoft.EntityFrameworkCore.Tools 6.0.10
- Microsoft.VisualStudio.Web.CodeGeneration.Design 6.0.10

Setup (Visual Studio):
1. Clone / download and unzip the repository.
2. Open `WhatHaveYouPlayed.sln` in Visual Studio 2022+ or load the project with the CLI.
3. Restore packages: `dotnet restore` or use __Tools > NuGet Package Manager__ -> __Manage NuGet Packages for Solution__.
4. In `appsettings.json` set the database connection under `ConnectionStrings:DefaultConnection`. Examples:
   - LocalDB (Visual Studio): `Server=(localdb)\\mssqllocaldb;Database=YOUR_DB_NAME;Trusted_Connection=True;MultipleActiveResultSets=true`
   - SQL Server Express: `Server=.\\SQLEXPRESS;Database=YOUR_DB_NAME;Trusted_Connection=True;MultipleActiveResultSets=true`
   Note: application uses SQL Server (LocalDB or SQLEXPRESS are both supported depending on the connection string).
5. Apply EF Core migrations:
   - In Visual Studio use __Package Manager Console__ and run `Update-Database`
   - Or in CLI run `dotnet ef database update` (install `dotnet-ef` tool if needed).
6. Populate initial data: run `init-values.sql` against the created database.
7. Configure JWT secrets if needed using __Manage User Secrets__ or `dotnet user-secrets` (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`).
8. Run the application (`dotnet run` or __Debug > Start Debugging__ in Visual Studio).
NOTE: Use exact package versions if you need to match the environment; otherwise `dotnet restore` will restore compatible versions.

Setup (Docker):
1. Use proper file from repository: `whyp-app-amd64.tar` or `whyp-app-arm64.tar` based on which architecture you use.
2. From localization with proper file run `docker load` in order to load docker image. Examples:
   - docker load -i .\whyp-app-amd64.tar (Windows)
   - docker load < whyp-app-amd64.tar (MacOS)
3. Run new container with loaded image. For example use `docker run -d --name whyp -p 80:80 -p 443:443 whathaveyouplayed:1.0`
NOTE: This is only for testing purposes - database fill is included into docker image (Sqlite)!
Important:
- Temporary admin user created for development: Login=`admin`, Password=`Polska11!`.


Short description of functionality:
- User registration, login and role-based access using ASP.NET Core Identity.
- Optional JWT authentication for API scenarios.
- CRUD management of user's games: add, edit, delete, and track status (e.g., Playing, Completed, Planning).
- Initial lookup and seed data provided via `init-values.sql`.
- Basic UI built with Razor Pages and Bootstrap; static assets in `wwwroot`.

## Author: Bartosz Zięba
