FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY src/ContactRegister.Domain/*.csproj ./src/ContactRegister.Domain/
COPY src/ContactRegister.Application.Ddd/*.csproj ./src/ContactRegister.Application.Ddd/
COPY src/ContactRegister.Infrastructure.Ddd/*.csproj ./src/ContactRegister.Infrastructure.Ddd/
COPY src/ContactRegister.Api.Ddd/*.csproj ./src/ContactRegister.Api.Ddd/
COPY shared/ContactRegister.Shared/*.csproj ./src/ContactRegister.Shared/

RUN dotnet restore ./src/ContactRegister.Api.Ddd/ContactRegister.Api.Ddd.csproj

COPY . .

WORKDIR /app/src/ContactRegister.Api.Ddd
RUN dotnet publish ContactRegister.Api.Ddd.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=build /app/src/ContactRegister.Api.Ddd/out ./

ENV ASPNETCORE_HTTP_PORTS=80

EXPOSE 80 443

ENTRYPOINT ["dotnet", "ContactRegister.Api.Ddd.dll"]