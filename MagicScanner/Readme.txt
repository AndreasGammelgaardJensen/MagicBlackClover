Database migration: 

1. Open Package Manager Console

2. Target MagicScanner.API project

3. Add migration by running command:
dotnet ef migrations add test --project ..\DataAccess\DataAccess.csproj --startup-project ..\MagicScanner.API\MagicScanner.API.csproj -o ..\DataAccess\Data


4. Opdate Database By Runneing: 
dotnet ef database update --project ..\DataAccess\DataAccess.csproj --startup-project ..\MagicScanner.API\MagicScanner.API.csproj