# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy everything else and build app
COPY . .
RUN dotnet restore
WORKDIR /src
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0.2
WORKDIR /app
EXPOSE 5089
ENV ASPNETCORE_URLS=http://+:5089
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MyWallet.dll"] #troque APIx pelo nome do seu app
