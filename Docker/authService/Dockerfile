FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /src
COPY ./authlib ./authlib
COPY ./authService ./authService
WORKDIR /src/authService
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as final
COPY --from=publish /app /authService
RUN dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p "httpscert"
RUN dotnet dev-certs https --trust
