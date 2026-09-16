# Proyecto de grado — Plataforma E-commerce Multi-tenant

## Cómo levantar el entorno (sin instalar nada en tu PC)

1. Crea un repositorio nuevo en GitHub (puede estar vacío).
2. Copia y pega TODOS los archivos y carpetas de este paquete dentro del repo,
   respetando la misma estructura de carpetas (`.devcontainer/`, `backend/`, `frontend/`, etc.).
3. Haz commit y push.
4. En GitHub, entra al repo → botón verde **"Code"** → pestaña **"Codespaces"** →
   **"Create codespace on main"**.
5. Espera unos minutos: automáticamente se instala .NET, Node/Angular y se levanta
   un contenedor de SQL Server. No necesitas tocar nada más.
6. Cuando termine, tendrás una terminal en el navegador (VS Code en la nube) ya lista.

## Estructura del proyecto

```
backend/
  ProyectoGrado.sln
  src/
    Domain/          -> entidades y reglas de negocio puras (sin dependencias)
    Application/      -> casos de uso (CQRS con MediatR), validaciones
    Infrastructure/    -> EF Core, SQL Server, Identity, implementaciones
    API/               -> ASP.NET Core Web API (punto de entrada)
frontend/            -> aquí se generará el proyecto Angular (siguiente paso)
.devcontainer/        -> configuración para que Codespaces levante todo solo
```

## Estado actual

Este es el **esqueleto base** de la arquitectura (Clean Architecture, 4 capas)
y la infraestructura de desarrollo en la nube. Todavía NO tiene funcionalidad —
eso corresponde al siguiente paso acordado: el módulo **Identity + Tenants**.

## Próximo paso

Con el Codespace ya funcionando, seguimos construyendo módulo por módulo,
empezando por autenticación + gestión de tenants (organizaciones).
