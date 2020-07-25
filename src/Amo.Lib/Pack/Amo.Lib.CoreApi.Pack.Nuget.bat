set /p a=Version:
dotnet build ../CoreApi/CoreApi.csproj -c Release
dotnet pack ../CoreApi/CoreApi.csproj -c Release -p:NuspecProperties="version=%a%""
dotnet nuget push ../CoreApi/bin/Release/Amo.Lib.CoreApi.%a%.nupkg -k oy2mza4fgrx3fqhtz5dargrumy2xl3yeli2hq7aqgc5c4u -s https://api.nuget.org/v3/index.json
echo %a%>Amo.Lib.CoreApi.Version.log