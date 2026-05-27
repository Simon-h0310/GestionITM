# 🚀 TALLER FINAL INTEGRADOR: "El Ecosistema ITM Nivel 5"

**Asignatura:** Programación de Software
**Modalidad:** Individual o Parejas (A criterio del docente)
**Calificación:** 0.0 a 5.0

---

## 🏢 Contexto del Negocio

El ITM ha quedado fascinado con el módulo de Profesores que construimos en clase. Ahora, la rectoría les ha asignado su primer proyecto como Arquitectos de Software oficiales: **El Módulo de Matrículas**.

Deberán construir la solución completa (Backend + Frontend Móvil) para que un Estudiante pueda ver los Cursos disponibles y matricularse desde su celular, respetando estrictamente los estándares de seguridad y arquitectura de la industria.

---

## 🛠️ REQUERIMIENTOS TÉCNICOS (Lo que deben programar)

### FASE A: El Motor (Backend API)

Su API debe extender el proyecto GestionITM y cumplir con lo siguiente:

1. **Arquitectura Limpia:** Construir los DTOs, Interfaces, Servicios y Repositorios para la entidad Matricula. (¡Prohibido inyectar el ApplicationDbContext directamente en el Controlador!).
2. **Regla de Negocio (El Chef):** Un estudiante NO puede matricularse en un curso si el curso ya no tiene cupos disponibles. Si lo intenta, el Servicio debe lanzar una excepción controlada.
3. **Seguridad JWT:** El endpoint para crear una matrícula (POST /api/matricula) debe estar protegido. Solo usuarios con el rol "Estudiante" pueden consumirlo.
4. **Paginación:** El endpoint para listar los cursos (GET /api/curso/paginado) debe usar el patrón PagedResult<T> y IQueryable para no colapsar la base de datos.
5. **Observabilidad:** Serilog debe estar configurado para guardar los errores graves en un archivo de texto dentro de la carpeta /Logs.

### FASE B: La Carrocería (Frontend MAUI)

Deberán crear el proyecto cliente GestionITM.AppMovil con las siguientes pantallas:

1. **Pantalla de Login:** Debe pedir Email y Password, consumir la API y guardar el Token usando SecureStorage.
2. **El Interceptor:** Un DelegatingHandler debe inyectar el token guardado en todas las peticiones automáticamente.
3. **Catálogo de Cursos (Scroll Infinito):** Una pantalla que liste los cursos disponibles usando CollectionView. Cuando el usuario llegue al final de la pantalla, debe cargar la página 2 automáticamente.
4. **Resiliencia (UX):** Si el usuario intenta matricularse y el Backend rechaza la petición (Ej. Error 400 por falta de cupos), la App debe leer el JSON de error y mostrar un DisplayAlert amable, sin cerrarse de golpe.

### FASE C: Infraestructura (DevOps)

1. **Dockerización:** La API debe tener su Dockerfile multietapa (SDK para compilar, Runtime para ejecutar).
2. **Orquestación:** Un archivo docker-compose.yml que levante la API y un contenedor de SQL Server simultáneamente, conectados por una red interna.

---

## 📦 ENTREGABLES (Qué deben subir a la plataforma)

El estudiante NO entregará archivos .zip (Eso es Nivel 1). Deberá entregar un documento PDF con lo siguiente:

1. **Enlace al Repositorio de GitHub:** El código fuente debe estar subido a GitHub.
2. **Evidencia de CI/CD (Opcional - Puntos Extra):** Una captura de pantalla de la pestaña "Actions" en GitHub mostrando el chulito verde (✅) del Pipeline ejecutando el comando dotnet test.
3. **Colección de Postman:** Un enlace público (o archivo exportado) con la colección de Postman documentando los endpoints construidos.
4. **Video Demostrativo (Máximo 3 minutos):** Un enlace a YouTube (Oculto) donde el estudiante muestre:
* El comando docker compose up funcionando.
* El celular (emulador) haciendo Login.
* El celular haciendo scroll infinito en los cursos.
* El celular intentando hacer una matrícula inválida y mostrando el mensaje de error.

---

## ⚖️ RÚBRICA DE EVALUACIÓN ESTRICTA (0.0 a 5.0)

| Criterio a Evaluar | Descripción Nivel 5 | Puntos |
| --- | --- | --- |
| **1. Arquitectura y Patrones** | Uso estricto de Interfaces, DTOs y AutoMapper. Cero lógica de negocio en los controladores. | **1.0** |
| **2. Paginación y Rendimiento** | Uso correcto de IQueryable, Skip, Take y PagedResult en el Backend. | **1.0** |
| **3. Seguridad y JWT** | El endpoint exige Token. La App MAUI usa SecureStorage y el Interceptor HTTP. | **1.0** |
| **4. UX y Consumo Móvil** | MAUI usa MVVM (Bindings correctos). El Scroll Infinito funciona. Los errores 400 se muestran en popups elegantes. | **1.0** |
| **5. Infraestructura DevOps** | El proyecto incluye Dockerfile y docker-compose.yml funcionales. Serilog genera archivos locales. | **1.0** |
| **Penalizaciones (Nivel 1)** | *Hacer commits con contraseñas reales. UI congelada por no usar 'async/await'. Enviar código que no compila.* | *-1.0 c/u* |