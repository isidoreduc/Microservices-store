FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ordering/Ordering.API/Ordering.API.csproj", "src/ordering/Ordering.API/"]
COPY ["src/ordering/Ordering.Application/Ordering.Application.csproj", "src/ordering/Ordering.Application/"]
COPY ["src/ordering/Ordering.Core/Ordering.Core.csproj", "src/ordering/Ordering.Core/"]
COPY ["src/shared/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/shared/EventBusRabbitMQ/"]
COPY ["src/ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/ordering/Ordering.Infrastructure/"]
RUN dotnet restore "src/ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
