FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
ARG version
RUN echo "version = ${version}"
WORKDIR /src
COPY ./authlib ./authlib
COPY ./authService ./authService
WORKDIR /src/authService
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Debug -o /app

FROM ubuntu:20.04 as uibuild
ARG version
RUN echo "version = ${version}"
RUN apt-get update
RUN apt-get install curl -y
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ./authService/UI/login .
RUN echo "${version}" > ./src/assets/version.txt 
RUN npm install
RUN npm run build

FROM build AS publish
RUN dotnet publish -c Release -o /app
COPY ./authService/localhost.conf /app/localhost.conf

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as final
COPY --from=uibuild /src/dist/login /authService/wwwroot
COPY --from=publish /app /authService
