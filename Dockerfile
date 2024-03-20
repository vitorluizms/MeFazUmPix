# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# copy everything else and build app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o dist

# final stage/image
FROM mcr.microsoft.com/dotnet/sdk:8.0
COPY --from=build-env /app/dist .
ENTRYPOINT ["dotnet", "MyWallet.dll"]
