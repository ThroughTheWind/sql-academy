FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props SqlAcademy.slnx global.json ./
COPY src/apps/SqlAcademy.Api/SqlAcademy.Api.csproj src/apps/SqlAcademy.Api/
COPY src/apps/SqlAcademy.Worker/SqlAcademy.Worker.csproj src/apps/SqlAcademy.Worker/
COPY src/libs/SqlAcademy.Domain/SqlAcademy.Domain.csproj src/libs/SqlAcademy.Domain/
COPY src/libs/SqlAcademy.Persistence/SqlAcademy.Persistence.csproj src/libs/SqlAcademy.Persistence/
COPY src/libs/SqlAcademy.Observability/SqlAcademy.Observability.csproj src/libs/SqlAcademy.Observability/
COPY src/libs/SqlAcademy.SharedKernel/SqlAcademy.SharedKernel.csproj src/libs/SqlAcademy.SharedKernel/
COPY src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj src/libs/SqlAcademy.Migrations/

RUN dotnet restore src/apps/SqlAcademy.Worker/SqlAcademy.Worker.csproj

COPY . .
RUN dotnet publish src/apps/SqlAcademy.Worker/SqlAcademy.Worker.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SqlAcademy.Worker.dll"]