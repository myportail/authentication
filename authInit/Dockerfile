FROM microsoft/dotnet:3.1-sdk as build
WORKDIR /src
COPY ./ ./authInit
WORKDIR /src/authInit
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM microsoft/dotnet:3.1-runtime as final
COPY --from=publish /app /authInit

