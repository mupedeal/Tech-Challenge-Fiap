FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY src/ContactRegister.Domain/*.csproj ./src/ContactRegister.Domain/
COPY src/ContactRegister.Application/*.csproj ./src/ContactRegister.Application/
COPY src/ContactRegister.Infrastructure/*.csproj ./src/ContactRegister.Infrastructure/
COPY src/ContactRegister.Api.ReadContact/*.csproj ./src/ContactRegister.Api.ReadContact/
COPY shared/ContactRegister.Shared/*.csproj ./src/ContactRegister.Shared/

RUN dotnet restore ./src/ContactRegister.Api.ReadContact/ContactRegister.Api.ReadContact.csproj

COPY . .

WORKDIR /app/src/ContactRegister.Api.ReadContact
RUN dotnet publish ContactRegister.Api.ReadContact.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=build /app/src/ContactRegister.Api.ReadContact/out ./

ENV ASPNETCORE_HTTP_PORTS=80

EXPOSE 80 443

ENTRYPOINT ["dotnet", "ContactRegister.Api.ReadContact.dll"]