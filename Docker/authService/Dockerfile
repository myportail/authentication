FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /src
COPY ./authlib ./authlib
COPY ./authService ./authService
WORKDIR /src/authService
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app
COPY ./authService/localhost.conf /app/localhost.conf

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as final
COPY --from=publish /app /authService

#RUN openssl req -new -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -config /authService/localhost.conf -subj '/CN=www.mydom.com/O=My Company Name LTD./C=US'
#RUN openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt
#RUN openssl verify -CAfile localhost.crt localhost.crt
#RUN copy localhost.crt /usr/local/share/ca-certificates
#RUN update-ca-certificates
#RUN openssl verify localhost.crt
