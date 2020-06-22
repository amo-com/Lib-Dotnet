set /p a=Version:
dotnet build ../Cache/Cache.csproj -c Release
dotnet build ../DataBase/DataBase.csproj -c Release
dotnet build ../Lib/Lib.csproj -c Release
dotnet build ../RestClient/RestClient.csproj -c Release
dotnet pack ../Lib/Lib.csproj -c Release -p:NuspecProperties="version=%a%""
dotnet nuget push ../Lib/bin/Release/Amo.Lib.%a%.nupkg -k oy2mza4fgrx3fqhtz5dargrumy2xl3yeli2hq7aqgc5c4u -s https://api.nuget.org/v3/index.json
echo %a%>Amo.Lib.Version.log