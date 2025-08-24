# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Gateway.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# 確保 wwwroot 目錄被複製
COPY --from=build /src/wwwroot ./wwwroot
EXPOSE 80
ENTRYPOINT ["dotnet", "Gateway.dll"]
