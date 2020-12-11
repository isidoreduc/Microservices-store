FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ApiGateway/OcelotAPI/OcelotAPI.csproj", "src/ApiGateway/OcelotAPI/"]
RUN dotnet restore "src/ApiGateway/OcelotAPI/OcelotAPI.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway/OcelotAPI"
RUN dotnet build "OcelotAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotAPI.dll"]
