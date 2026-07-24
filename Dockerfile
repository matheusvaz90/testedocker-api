FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App
COPY . ./
RUN dotnet publish "TodoApi.csproj" -c Release -o /App/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
EXPOSE 8080