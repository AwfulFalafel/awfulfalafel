FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine3.21 AS build
WORKDIR /app
COPY ./awfulfalafel.slnx main.db ./
COPY ./app-cs ./app-cs
COPY ./common ./common
COPY ./admin ./admin
RUN dotnet restore
RUN dotnet publish ./admin/AwfulFalafelAdmin.csproj

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.21
WORKDIR /app
COPY --from=build /app/admin/bin/Release/net9.0/publish /app/main.db ./
ENTRYPOINT [ "dotnet", "AwfulFalafelAdmin.dll" ]