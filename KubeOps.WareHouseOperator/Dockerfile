# Build the operator
FROM mcr.microsoft.com/dotnet/core/sdk:latest as build
WORKDIR /operator

COPY ./ ./
RUN dotnet publish -c Release -o out KubeOps.TestOperator.csproj

# The runner for the application
FROM mcr.microsoft.com/dotnet/core/aspnet:latest as final
WORKDIR /operator

COPY --from=build /operator/out/ ./

CMD [ "dotnet", "KubeOps.TestOperator.dll" ]
