FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BankTechAccountSavings.API/BankTechAccountSavings.API.csproj", "BankTechAccountSavings.API/"]
COPY ["BankTechAccountSavings.Application/BankTechAccountSavings.Application.csproj", "BankTechAccountSavings.Application/"]
COPY ["BankTechAccountSavings.Domain/BankTechAccountSavings.Domain.csproj", "BankTechAccountSavings.Domain/"]
COPY ["BankTechAccountSavings.Infrastructure/BankTechAccountSavings.Infrastructure.csproj", "BankTechAccountSavings.Infrastructure/"]
RUN dotnet restore "./BankTechAccountSavings.API/./BankTechAccountSavings.API.csproj"
COPY . .
WORKDIR "/src/BankTechAccountSavings.API"
RUN dotnet build "./BankTechAccountSavings.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BankTechAccountSavings.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankTechAccountSavings.API.dll"]