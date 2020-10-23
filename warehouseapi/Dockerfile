FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.4 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.300 AS build
COPY *.sln .
COPY k8sdemo/k8sdemo.csproj ./k8sdemo/k8sdemo.csproj

RUN dotnet restore ./k8sdemo/k8sdemo.csproj

COPY . .

FROM build AS publish
WORKDIR k8sdemo
RUN dotnet publish k8sdemo.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "k8sdemo.dll"]
