from mcr.microsoft.com/dotnet/sdk:8.0 as build
workdir /app

copy *.csproj ./
run dotnet restore

copy . ./

run dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BookStoreRest.dll"]