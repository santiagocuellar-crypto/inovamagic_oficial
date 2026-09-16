#!/usr/bin/env bash
set -e

echo "==> Instalando Angular CLI global..."
npm install -g @angular/cli

echo "==> Restaurando paquetes de .NET (backend)..."
cd /workspace/backend
dotnet restore || echo "Aun no hay .sln, se restaurara cuando se agregue"

echo "==> Instalando dependencias del frontend (si ya existe package.json)..."
cd /workspace/frontend
if [ -f "package.json" ]; then
  npm install
else
  echo "El proyecto Angular todavia no ha sido creado. Corre: ng new frontend --directory . --routing --style=scss"
fi

echo "==> Listo. Entorno preparado."
