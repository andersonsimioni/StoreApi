set /P migrationName=Migration name?
dotnet ef migrations add %migrationName%