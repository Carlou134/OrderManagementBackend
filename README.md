# Order Management — Backend API

> API REST para gestión de órdenes y productos, construida en .NET 8 con Clean Architecture — proyecto de prueba técnica.

![swagger](docs/screenshots/swagger.png)

---

## 🧩 Problema / Contexto

Prueba técnica que pide un sistema de gestión de órdenes: crear/editar/eliminar órdenes, asociarles productos con cantidad y precio, y cambiar su estado (`Pending` → `InProgress` → `Completed`), con la regla de que una orden `Completed` no puede volver a modificarse. El enunciado permitía Express/Node como base, con .NET Core como bonus track — se optó directamente por .NET 8 para mostrar Clean Architecture, SOLID y buenas prácticas de una API productiva.

---

## 🛠️ Stack

| Capa            | Tecnología       |
|-----------------|------------------|
| Backend         | .NET 8 / ASP.NET Core Web API |
| ORM             | Entity Framework Core 8 + Pomelo.EntityFrameworkCore.MySql |
| Base de datos   | MySQL |
| Validación      | FluentValidation |
| Mapeo           | AutoMapper |
| Auth            | Ninguna (requisito explícito del enunciado — endpoints sin login/token) |
| Testing         | xUnit + Moq (15 tests unitarios sobre reglas de negocio) |

---

## 🏗️ Arquitectura

Clean Architecture en 4 proyectos, con la regla de dependencias apuntando siempre hacia adentro:

```
Domain          → sin dependencias (entidades, interfaces de repositorio, excepciones de dominio)
Application     → depende solo de Domain (servicios, DTOs, validadores FluentValidation)
Infrastructure  → depende solo de Domain (EF Core, repositorios, DbContext)
Api             → depende de Application + Infrastructure (composition root)
```

- **Repository Pattern** (`IOrderRepository`/`IProductRepository`) para aislar el acceso a datos del resto de las capas.
- **Auditoría automática**: interfaz `IAuditableEntity` (`CreatedAt`/`UpdatedAt`/`CreatedBy`/`UpdatedBy`) estampada centralizadamente en `OrdersContext.SaveChanges`, en vez de repetir la lógica en cada servicio.
- **Manejo de errores unificado con `ProblemDetails`** (RFC 7807): un `IExceptionHandler` nativo de .NET 8 traduce excepciones de dominio (`BusinessRuleException`) a 409, y cualquier error inesperado a 500 sin filtrar detalles internos al cliente.
- **Validación centralizada**: un `ValidationFilter` global resuelve automáticamente el validador de FluentValidation correspondiente a cada request, sin repetir `ValidateAsync` en cada endpoint.
- **Paginación + filtros** en los listados (`GetOrders` por `Status`, `GetProducts` por `Name`), con límite de tamaño de página para evitar respuestas sin cota.

---

## 🧠 Retos técnicos y decisiones

- **Problema:** `Application` referenciaba directamente `Infrastructure`, violando la regla de dependencias de Clean Architecture (aunque en el código no se usaba nada de ahí). → **Solución:** se cambió la referencia de proyecto para que `Application` dependa de `Domain` directamente. → **Por qué:** Dependency Inversion — las capas internas no pueden depender de las externas, ni siquiera "sin querer".

- **Problema:** las reglas de negocio (ej. "no modificar una orden `Completed`") se lanzaban como `InvalidOperationException`, un tipo genérico del BCL que también usa el runtime para casos totalmente ajenos — cualquier bug real que tirara esa excepción se hubiera "disfrazado" de regla de negocio y filtrado su mensaje interno al cliente. → **Solución:** excepción de dominio propia (`BusinessRuleException`), capturada específicamente por un `IExceptionHandler` que la traduce a 409, dejando el 500 solo para errores realmente inesperados. → **Por qué:** separar "esto lo rechazamos a propósito" de "esto se rompió".

- **Problema:** al editar una orden, el código borraba y recreaba todos sus `OrderProduct` (`Clear()` + lista nueva), lo que reseteaba su fecha real de creación en cada edición. → **Solución:** diff por `ProductId` — se actualizan in-place los que ya existían, se agregan los nuevos y se remueven los que ya no vienen. → **Por qué:** preservar la auditoría real sin sacrificar la simplicidad del endpoint.

- **Problema:** el connection string de MySQL (con usuario y password reales) estaba commiteado en `appsettings.json`. → **Solución:** se destrackeó del repo, se agregó `appsettings.example.json` como plantilla sin secretos, y el valor real vive en User Secrets (desarrollo) o variables de entorno (producción). → **Por qué:** eliminar el secreto del control de versiones sin romper el flujo de desarrollo de nadie que clone el repo.

---

## 🚀 Cómo correrlo

Requisitos: .NET 8 SDK, MySQL corriendo en local.

```bash
git clone git@github.com:Carlou134/TaskManagerBackend.git
cd TaskManagerBackend

# Configurar tu connection string local (no se commitea)
cp OrderManagementBackend.Api/appsettings.example.json OrderManagementBackend.Api/appsettings.Development.json
# Editar appsettings.Development.json con tus credenciales de MySQL local

# Aplicar las migraciones
dotnet ef database update --project OrderManagementBackend.Infrastructure --startup-project OrderManagementBackend.Api

# Correr la API
dotnet run --project OrderManagementBackend.Api
```

La API queda disponible en `https://localhost:7197` (o el puerto que asigne tu perfil de `launchSettings.json`), con Swagger en `/swagger`.

### Correr en Visual Studio

1. Abrir `OrderManagementBackend.Api.sln` con Visual Studio.
2. Repite el paso del connection string local: copia `OrderManagementBackend.Api/appsettings.example.json` a `OrderManagementBackend.Api/appsettings.Development.json` y completa tus credenciales de MySQL.
3. Aplica las migraciones desde la **Package Manager Console** (`Tools > NuGet Package Manager > Package Manager Console`), con `OrderManagementBackend.Infrastructure` como *Default project*:
   ```powershell
   Update-Database -Project OrderManagementBackend.Infrastructure -StartupProject OrderManagementBackend.Api
   ```
4. Selecciona el perfil de arranque `https` (o `http`) en el dropdown de la barra de herramientas y presiona **F5** — se abre el navegador directo en Swagger.

---

## 🔗 Links relacionados

Frontend: https://github.com/Carlou134/OrderManagementFrontEnd
