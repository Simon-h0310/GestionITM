#  CHECKLIST DE PRUEBAS - PROYECTO GESTIONITM

**Antes de entregar el proyecto, verifica que TODO esté funcionando:**

---

##  PASO 1: COMPILACIÓN

```bash
# Desde la raíz del proyecto
dotnet build
```

✅ **Resultado esperado:** Build succeeded. 0 Error(s)

---

##  PASO 2: DOCKER COMPOSE

### 2.1 Levantar los contenedores

```bash
docker compose up -d
```

✅ **Resultado esperado:**
```
✔ Container itm-database   Started
✔ Container itm-api        Started
```

### 2.2 Verificar que estén corriendo

```bash
docker ps
```

✅ **Deberías ver:**
- `itm-database` (SQL Server 2022)
- `itm-api` (Tu API)

### 2.3 Probar la API

Abre tu navegador y ve a:
```
http://localhost:8080/swagger
```

✅ **Resultado esperado:** Swagger UI con todos tus endpoints documentados.

---

##  PASO 3: PROBAR LA APP MÓVIL

### 3.1 Configurar el Emulador

1. En Visual Studio 2022, selecciona el proyecto `GestionITM.AppMovil` como proyecto de inicio.
2. En la barra superior, selecciona un emulador de Android (por ejemplo, "Pixel 5 - API 33").

### 3.2 Ejecutar la App

Presiona **F5** o el botón "▶ Start".

✅ **Resultado esperado:** El emulador se abre y carga la pantalla de Login.

---

##  PASO 4: FLUJO DE PRUEBAS COMPLETO

### ✅ Test 1: Login
1. En la pantalla de Login, ingresa:
   - **Email:** `estudiante@itm.edu.co` (o el correo de un usuario real en tu BD)
   - **Password:** `123456` (o la contraseña correcta)
2. Presiona "Entrar".

**Resultado esperado:**
- ✅ Si las credenciales son correctas: navegas a la pantalla de Cursos.
- ❌ Si son incorrectas: aparece un popup "Credenciales inválidas".

---

###  Test 2: Listar Cursos (Paginación)
1. Una vez en la pantalla de Cursos, deberías ver los primeros 10 cursos (si los hay en la BD).
2. Desliza hacia abajo lentamente con el dedo (o mouse).

**Resultado esperado:**
- ✅ Al llegar cerca del final, aparece un `ActivityIndicator` girando.
- ✅ Se cargan automáticamente los siguientes 10 cursos.
- ✅ No se duplican los cursos.

---

###  Test 3: Matricularse en un Curso (Happy Path)
1. Selecciona un curso que tenga cupos disponibles.
2. Presiona el botón "Matricular".
3. Confirma en el popup "¿Deseas matricularte en...?"

**Resultado esperado:**
- ✅ Aparece un popup "Éxito: Matrícula exitosa."
- ✅ El número de `CuposDisponibles` del curso disminuye en 1.

---

###  Test 4: Intentar Matricularse Sin Cupos (Error 400)
1. Encuentra un curso que tenga `CuposDisponibles = 0` (o matricúlate 30 veces en el mismo curso).
2. Intenta matricularte.

**Resultado esperado:**
- ❌ Aparece un popup "Error: No hay cupos disponibles para este curso."
- ✅ La app NO se cierra abruptamente.

---

###  Test 5: Ver Profesores (Clase 20)
1. Navega a la pantalla de Profesores (si tienes un botón en el Shell).
2. Presiona "Descargar Profesores".

**Resultado esperado:**
- ✅ Se muestra una lista de profesores con nombre y especialidad.
- ✅ El título de la página cambia a "Se cargaron X profesores".

---

##  PASO 5: VERIFICAR LOGS (Serilog)

### 5.1 Ubicación de los Logs
En tu proyecto Backend, deberías tener una carpeta llamada:
```
GestionITM.API/Logs/
```

### 5.2 Abrir el Archivo
Busca un archivo con el formato:
```
api-log-20260522.txt
```
(La fecha será la de hoy)

### 5.3 Contenido Esperado
Deberías ver líneas como:
```
[10:35:12 INF] Arrancando el servidor GestionITM API...
[10:35:15 INF] Application started. Press Ctrl+C to shut down.
[10:35:20 INF] HTTP POST /api/auth/login responded 200 in 123 ms
[10:35:25 INF] HTTP GET /api/curso/paginado?pageNumber=1&pageSize=10 responded 200 in 45 ms
```

✅ **Si ves estas líneas:** Serilog está funcionando correctamente.

---

##  PASO 6: VERIFICAR SEGURIDAD

### 6.1 Intentar Acceder Sin Token
Abre Postman y envía una petición **sin el header `Authorization`**:
```
GET http://localhost:8080/api/matricula/mis-matriculas
```

**Resultado esperado:**
- ❌ Código de respuesta: **401 Unauthorized**

---

### 6.2 Acceder Con Token
1. Haz login desde Postman:
   ```
   POST http://localhost:8080/api/auth/login
   Body (JSON):
   {
	 "correo": "estudiante@itm.edu.co",
	 "contraseña": "123456"
   }
   ```
2. Copia el `token` de la respuesta.
3. Envía una nueva petición:
   ```
   GET http://localhost:8080/api/matricula/mis-matriculas
   Headers:
   Authorization: Bearer {TU_TOKEN_AQUI}
   ```

**Resultado esperado:**
- ✅ Código de respuesta: **200 OK**
- ✅ Recibes la lista de matrículas del estudiante.

---

##  PASO 7: GRABAR EL VIDEO DEMOSTRATIVO

### Contenido del Video (Máximo 3 minutos)

1. **[00:00 - 00:20]** Mostrar la pantalla de inicio del proyecto en Visual Studio.
2. **[00:20 - 00:40]** Ejecutar `docker compose up` en la terminal y mostrar que ambos contenedores arrancan.
3. **[00:40 - 01:00]** Abrir el navegador en `http://localhost:8080/swagger` y mostrar los endpoints.
4. **[01:00 - 01:30]** Ejecutar la App MAUI en el emulador de Android y hacer Login.
5. **[01:30 - 02:00]** Hacer scroll infinito en la lista de Cursos.
6. **[02:00 - 02:30]** Matricularse en un curso exitosamente.
7. **[02:30 - 03:00]** Intentar matricularse sin cupos y mostrar el mensaje de error.

### Herramientas para Grabar
- **Windows:** Xbox Game Bar (Win + G)
- **OBS Studio:** [obsproject.com](https://obsproject.com/)
- **Loom:** [loom.com](https://www.loom.com/)

---

##  PASO 8: PREPARAR EL REPOSITORIO GITHUB

### 8.1 Verificar que estos archivos ESTÉN en .gitignore

```
*.log
Logs/
bin/
obj/
.vs/
appsettings.Development.json
```

### 8.2 Hacer el último commit

```bash
git add .
git commit -m "✅ Proyecto final completo - GestionITM Nivel 5"
git push origin main
```

---

##  PASO 9: CREAR EL DOCUMENTO DE ENTREGA

Crea un archivo PDF con:

1. **Portada:**
   - Título: "Proyecto Final - GestionITM"
   - Tu nombre completo
   - Fecha

2. **Enlace al Repositorio:**
   ```
   https://github.com/TU-USUARIO/GestionITM
   ```

3. **Enlace al Video:**
   ```
   https://youtu.be/XXXXXXX (Oculto)
   ```

4. **Capturas de Pantalla:**
   - Docker Compose corriendo
   - Swagger UI
   - App MAUI en Login
   - App MAUI con scroll infinito
   - Popup de error 400

5. **Colección de Postman:**
   - Exporta tu colección de Postman como JSON.
   - Súbela a tu repositorio en una carpeta llamada `Postman/`.
   - En el PDF, pon el enlace:
	 ```
	 https://github.com/TU-USUARIO/GestionITM/blob/main/Postman/GestionITM.postman_collection.json
	 ```

---

##  CHECKLIST FINAL DE ENTREGA

Antes de subir tu PDF a la plataforma, verifica:

- [ ] ✅ El proyecto compila sin errores (`dotnet build`)
- [ ] ✅ `docker compose up` levanta SQL Server y la API
- [ ] ✅ La App MAUI corre en el emulador de Android
- [ ] ✅ El Login funciona y guarda el token en `SecureStorage`
- [ ] ✅ El scroll infinito carga los cursos de a 10
- [ ] ✅ El botón "Matricular" funciona
- [ ] ✅ El manejo de errores 400 muestra el mensaje del backend
- [ ] ✅ Serilog genera archivos en la carpeta `Logs/`
- [ ] ✅ El código está en GitHub (sin contraseñas reales en el código)
- [ ] ✅ El video demostrativo está en YouTube (Oculto)
- [ ] ✅ La colección de Postman está exportada

---

##  ¡FELICITACIONES!

Si todos los checkboxes están marcados, tu proyecto está **listo para ser entregado**.

Eres oficialmente un **Arquitecto Full-Stack Nivel 5**.

---

**Última revisión:** 2026
**Autor:** Daniel Villamizar - [GitHub](https://github.com/danielvillamizar)