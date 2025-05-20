# -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:9.0.200 AS build
WORKDIR /app

# Copy sln and supporting files
COPY .editorconfig ./
COPY Directory.* ./
COPY global.json ./
COPY *.slnx ./

# Copy source
COPY aspire/CabaVS.ExpenseTracker.ServiceDefaults/ ./aspire/CabaVS.ExpenseTracker.ServiceDefaults/
COPY projects/expense-tracker/src/ ./projects/expense-tracker/src/

# Publish (build + restore in one go)
WORKDIR /app/projects/expense-tracker/src/CabaVS.ExpenseTracker.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CabaVS.ExpenseTracker.API.dll"]
