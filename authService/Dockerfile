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
RUN openssl req -new -x509 -nodes -days 365 -newkey rsa:2048 -keyout /authService/localhost.key -out /authService/localhost.crt -config /authService/localhost.conf -subj '/CN=www.mydom.com/O=My Company Name LTD./C=US'
RUN openssl pkcs12 -export -out /authService/localhost.pfx -inkey /authService/localhost.key -in /authService/localhost.crt --password pass:authservice
RUN openssl verify -CAfile /authService/localhost.crt /authService/localhost.crt
RUN cp /authService/localhost.crt /usr/local/share/ca-certificates
RUN update-ca-certificates
RUN openssl verify /authService/localhost.crt