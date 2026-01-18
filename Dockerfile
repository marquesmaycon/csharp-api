FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copiar arquivos de projeto
COPY ["CSharpApi.csproj", "./"]

# Restaurar dependências
RUN dotnet restore "CSharpApi.csproj"

# Copiar código
COPY . .

# Build
RUN dotnet build "CSharpApi.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "CSharpApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Instalar ferramentas necessárias
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Copiar script de entrypoint
COPY entrypoint.sh ./
RUN chmod +x ./entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["./entrypoint.sh"]
