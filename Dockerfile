# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy solution and csproj files first (leverages Docker cache)
COPY ["WEXTransactions.sln", "."]
COPY ["WEX.Transaction.Domain/WEX.TransactionAPI.Domain.csproj", "WEX.Transaction.Domain/"]
COPY ["WEX.TransactionAPI.Application/WEX.TransactionAPI.Application.csproj", "WEX.TransactionAPI.Application/"]
COPY ["WEX.TransactionAPI.Infrastructure/WEX.TransactionAPI.Infrastructure.csproj", "WEX.TransactionAPI.Infrastructure/"]
COPY ["WEX.TransactionAPI.Tests/WEX.TransactionAPI.Tests.csproj", "WEX.TransactionAPI.Tests/"]
COPY ["wex-transactions-api/WEX.TransactionAPI.Web.csproj", "wex-transactions-api/"]

# Restore dependencies
RUN dotnet restore "WEXTransactions.sln"

# Copy all source code
COPY . .

# Stage 3: Build the Web project
WORKDIR "/src/wex-transactions-api"
RUN dotnet build "WEX.TransactionAPI.Web.csproj" -c Release -o /app/build

# Stage 5: Publish
FROM build AS publish
WORKDIR "/src/wex-transactions-api"
RUN dotnet publish "WEX.TransactionAPI.Web.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Stage 6: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WEX.TransactionAPI.Web.dll"]
