# 🏆 Guía de Solución: Taller Final Integrador (Módulo Matrículas)

Este documento detalla cómo se implementó la solución completa exigida en el **Taller Final Nivel 5**.

## ⚙️ 1. Fase A: Backend (API y Dominio)
Se respetó estrictamente la Arquitectura Limpia aislando responsabilidades:

- **DTOs (GestionITM.Domain/Dtos):** Se crearon CursoDto, MatriculaDto y MatriculaCreateDto para no exponer las entidades reales.
- **Interfaces y Repositorios (GestionITM.Infrastructure):** 
  - ICursoRepository expone QueryAll() devolviendo IQueryable<Curso>.
  - IMatriculaRepository incluye la lógica de contar cupos.
- **Reglas de Negocio en Servicios (GestionITM.Domain/Services):** 
  - MatriculaService tiene la lógica restrictiva: lanza InvalidOperationException si se excede el límite (30 cupos).
  - CursoService implementa la paginación con Skip y Take usando PagedResult<T>.
- **Seguridad y Controladores:** MatriculaController exige rol [Authorize(Roles = "Estudiante")] y extrae el ID directamente de los *Claims* del JWT.
- **Observabilidad:** Serilog se configuró en Program.cs y ppsettings.json para guardar logs en Logs/api-log-.txt.

## 📱 2. Fase B: Frontend MAUI (App Móvil)
- **Capa de Servicios y Seguridad:** 
  - AuthenticationHandler (Interceptor) inyecta el Bearer Token almacenado en SecureStorage en cada petición HTTP hacia la API.
- **Patrón MVVM:** 
  - Se crearon LoginViewModel y CursosViewModel.
  - Las vistas LoginPage y CursosPage se enlazan mediante BindingContext.
- **UX (Experiencia de Usuario):**
  - **Scroll Infinito:** El CollectionView utiliza RemainingItemsThreshold para invocar automáticamente CargarCursosCommand y pedir la página 2, 3, etc.
  - **Manejo de Errores Resiliente:** Si la API devuelve un Error HTTP 400 (Ej: _"No hay cupos disponibles"_), el MatriculaService atrapa el JSON y se muestra un DisplayAlert amable al usuario, evitando que se "crashee" la App.

## 🐋 3. Fase C: Infraestructura (DevOps)
El entorno está preparado para producción:
- El Dockerfile utiliza etapas de compilación (build) y publicación separadas del runtime final (imagen ligera de .NET 8).
- El docker-compose.yml orquesta nativamente la capa de SQL Server for Linux y la API, mapeando credenciales limpias y conectividad por red interna.