FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./Cefalo.TechDaily.Api/Cefalo.TechDaily.Api.csproj" --disable-parallel
RUN dotnet publish "./Cefalo.TechDaily.Api/Cefalo.TechDaily.Api.csproj" -c release -o /app --no-restore

# serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Cefalo.TechDaily.Api.dll"]