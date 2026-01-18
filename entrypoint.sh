#!/bin/bash
set -e

echo "Aguardando PostgreSQL ficar disponível..."
until pg_isready -h $POSTGRES_HOST -U $POSTGRES_USER 2>/dev/null; do
  sleep 1
done

echo "PostgreSQL está disponível!"

# Instalar dotnet-ef se não estiver disponível
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
  echo "Instalando Entity Framework CLI..."
  dotnet tool install --global dotnet-ef
fi

# Executar migrations
echo "Executando migrations..."
export PATH="$PATH:/root/.dotnet/tools"
dotnet ef database update --project /app/CSharpApi.csproj || true

echo "Iniciando aplicação..."
exec dotnet CSharpApi.dll
