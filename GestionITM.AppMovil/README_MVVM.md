# 📱 GestionITM - App Móvil .NET MAUI

## 🎯 ¿Qué es este proyecto?

Esta es una **aplicación móvil multiplataforma** (Android, iOS, Windows, Mac) desarrollada con **.NET MAUI** que implementa el **patrón MVVM** usando **CommunityToolkit.Mvvm** para gestionar profesores del ITM.

---

## 🏗️ Arquitectura del Proyecto

```
GestionITM.AppMovil/
│
├── 📦 Models/                     # Modelos de datos (entidades)
│   └── ProfesorModel.cs           # Clase que representa un profesor
│
├── 🎭 ViewModels/                 # Lógica de presentación (el "cerebro")
│   └── ProfesoresViewModel.cs     # Maneja la lista de profesores
│
├── 🖼️ Views/                      # Interfaces de usuario (XAML)
│   ├── ProfesoresPage.xaml        # Pantalla de lista de profesores
│   └── ProfesoresPage.xaml.cs     # Code-behind (mínimo)
│
├── 🔧 Services/                   # Servicios (futuro: API, DB)
│   └── (pendiente)
│
├── 🎨 Resources/                  # Recursos de la app
│   ├── Images/                    # Imágenes
│   ├── Fonts/                     # Fuentes
│   └── Styles/                    # Estilos XAML
│
└── 🚀 Platforms/                  # Código específico de plataforma
	├── Android/
	├── iOS/
	├── Windows/
	└── MacCatalyst/
```

---

## 🧠 ¿Qué es MVVM?

**MVVM** = Model - View - ViewModel

### Analogía del Teatro de Marionetas 🎭

Imaginen que están montando una obra de teatro con marionetas:

1. **Model (Modelo)** 📦
   - Es como el **guión** de la obra
   - Define QUÉ información existe (nombre, apellido, email del profesor)
   - NO tiene lógica, solo datos

2. **ViewModel (El Titiritero)** 🎭
   - Es el **titiritero** que controla las marionetas
   - Tiene toda la lógica: cargar datos, buscar, filtrar, etc.
   - Jala los "hilos" para que la marioneta se mueva

3. **View (La Marioneta)** 🖼️
   - Es la **marioneta** (la pantalla XAML)
   - Solo se muestra y responde a los hilos del titiritero
   - NO piensa, solo obedece

---

## ⚡ La Magia de los Source Generators

### ¿Qué son?

Son como **"elfos mágicos del compilador"** que escriben código por ti mientras compilas.

### Ejemplo:

**Lo que TÚ escribes:**
```csharp
public partial class ProfesoresViewModel : ObservableObject
{
	[ObservableProperty]
	private bool estaCargando;  // ⬅️ minúscula
}
```

**Lo que el Source Generator crea AUTOMÁTICAMENTE (invisible para ti):**
```csharp
public bool EstaCargando  // ⬅️ Mayúscula + PascalCase
{
	get => estaCargando;
	set
	{
		if (estaCargando != value)
		{
			estaCargando = value;
			OnPropertyChanged(nameof(EstaCargando));  // ⬅️ Notifica a la UI
		}
	}
}
```

---

## 🎨 Atributos Mágicos de CommunityToolkit.Mvvm

### 1. `[ObservableProperty]`

**¿Qué hace?** Convierte una variable privada en una propiedad pública con notificaciones automáticas.

```csharp
[ObservableProperty]
private string nombre;  // Genera: public string Nombre { get; set; } + INotifyPropertyChanged
```

**Reglas:**
- La variable DEBE ser `private`
- La variable DEBE estar en `camelCase` (minúscula)
- La propiedad generada será `PascalCase` (mayúscula)

---

### 2. `[RelayCommand]`

**¿Qué hace?** Convierte un método en un comando ejecutable desde XAML.

```csharp
[RelayCommand]
private async Task CargarProfesores()
{
	// Tu código aquí
}
```

Esto genera automáticamente:
- `CargarProfesoresCommand` (tipo `IAsyncRelayCommand`)
- Puedes usarlo en XAML así: `Command="{Binding CargarProfesoresCommand}"`

**Reglas:**
- El método NO debe tener sufijo "Command"
- Puede ser `void`, `Task`, `Task<T>`
- Puede recibir parámetros: `CargarProfesor(int id)`

---

## 🔧 Configuración de Inyección de Dependencias

En `MauiProgram.cs`:

```csharp
// ViewModels como Singleton (1 instancia compartida)
builder.Services.AddSingleton<ProfesoresViewModel>();

// Views como Transient (nueva instancia cada vez)
builder.Services.AddTransient<ProfesoresPage>();
```

**¿Por qué?**
- **Singleton:** El ViewModel mantiene su estado mientras navegas (no se reinicia)
- **Transient:** Cada pantalla es nueva y fresca

---

## 🚀 Flujo de Datos en MVVM

```
┌─────────────────────────────────────────────────────────────┐
│  Usuario toca botón "Cargar Profesores"                    │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  View (XAML) ejecuta CargarProfesoresCommand               │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  ViewModel cambia EstaCargando = true                       │
│  (Automáticamente notifica a la View)                       │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  View actualiza UI: ActivityIndicator empieza a girar      │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  ViewModel llama al servicio para obtener datos            │
│  (Futuro: llamada a API)                                    │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  ViewModel actualiza Profesores = listaDesdeAPI            │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  ViewModel cambia EstaCargando = false                      │
└──────────────────┬──────────────────────────────────────────┘
				   ↓
┌─────────────────────────────────────────────────────────────┐
│  View actualiza UI: Muestra lista de profesores            │
└─────────────────────────────────────────────────────────────┘
```

---

## ⚠️ Problemas Comunes y Soluciones

### ❌ Error: "La propiedad 'EstaCargando' no existe"

**Causa:** No compilaste después de agregar `[ObservableProperty]`

**Solución:**
1. Haz clic en **Compilar → Recompilar Solución**
2. Cierra y vuelve a abrir el archivo XAML
3. Si persiste, limpia la solución (**Compilar → Limpiar Solución**)

---

### ❌ Error: "CS0260: Missing partial modifier"

**Causa:** Olvidaste `partial` en la clase ViewModel

```csharp
// ❌ MAL
public class ProfesoresViewModel : ObservableObject

// ✅ BIEN
public partial class ProfesoresViewModel : ObservableObject
```

**¿Por qué `partial`?** Permite que el Source Generator agregue código a tu clase.

---

### ❌ Error: "Command not found in BindingContext"

**Causa:** No compilaste después de agregar `[RelayCommand]`

**Solución:**
1. Verifica que el método NO tenga sufijo "Command"
2. Recompila la solución
3. El comando debe llamarse: `NombreDelMétodoCommand`

---

## 🎓 Ejercicios para Practicar

### Ejercicio 1: Agregar Búsqueda de Profesores

1. Agrega esta propiedad al ViewModel:
```csharp
[ObservableProperty]
private string textoBusqueda;
```

2. Crea este comando:
```csharp
[RelayCommand]
private void BuscarProfesores()
{
	// Filtrar la lista de profesores
}
```

3. En XAML, agrega un `SearchBar`:
```xml
<SearchBar Text="{Binding TextoBusqueda}" 
		   SearchCommand="{Binding BuscarProfesoresCommand}" />
```

---

### Ejercicio 2: Agregar Manejo de Errores

1. Agrega estas propiedades:
```csharp
[ObservableProperty]
private bool hayError;

[ObservableProperty]
private string mensajeError;
```

2. Modifica el comando `CargarProfesores`:
```csharp
[RelayCommand]
private async Task CargarProfesores()
{
	try
	{
		EstaCargando = true;
		HayError = false;
		// ... cargar datos
	}
	catch (Exception ex)
	{
		HayError = true;
		MensajeError = ex.Message;
	}
	finally
	{
		EstaCargando = false;
	}
}
```

3. En XAML, muestra el error:
```xml
<Label Text="{Binding MensajeError}" 
	   IsVisible="{Binding HayError}"
	   TextColor="Red" />
```

---

## 📚 Recursos Adicionales

### Documentación Oficial:
- **.NET MAUI:** https://learn.microsoft.com/es-es/dotnet/maui/
- **MVVM Toolkit:** https://learn.microsoft.com/es-es/dotnet/communitytoolkit/mvvm/
- **Source Generators:** https://learn.microsoft.com/es-es/dotnet/csharp/roslyn-sdk/source-generators-overview

### Tutoriales en Video:
- **Canal de .NET en YouTube:** https://www.youtube.com/@dotnet
- **James Montemagno (creador de .NET MAUI):** https://www.youtube.com/@JamesMontemagno

---

## 🔄 Próximos Pasos

1. ✅ ~~Implementar MVVM básico~~
2. 🔲 Conectar con la API (GestionITM.API)
3. 🔲 Implementar navegación entre páginas
4. 🔲 Agregar CRUD completo (Crear, Editar, Eliminar profesores)
5. 🔲 Implementar búsqueda y filtros
6. 🔲 Agregar manejo de errores robusto
7. 🔲 Implementar caché local (SQLite)
8. 🔲 Agregar autenticación

---

## 🙋 Preguntas Frecuentes

**Q: ¿Por qué usar MVVM en lugar de code-behind?**  
**A:** MVVM separa la lógica de la UI, lo que hace el código:
- ✅ Más testeable (puedes probar ViewModels sin UI)
- ✅ Reutilizable (mismo ViewModel para iOS, Android, Windows)
- ✅ Mantenible (cambios en lógica no afectan XAML)
- ✅ Escalable (equipos grandes pueden trabajar en paralelo)

**Q: ¿Qué diferencia hay entre ObservableObject y ObservableCollection?**  
**A:**
- `ObservableObject`: Clase base para ViewModels (notifica cambios de propiedades)
- `ObservableCollection<T>`: Lista que notifica cuando se agregan/quitan items

**Q: ¿Cuándo usar Singleton vs Transient para ViewModels?**  
**A:**
- **Singleton:** Cuando quieres mantener el estado (ej: carrito de compras)
- **Transient:** Cuando cada instancia debe ser independiente (ej: detalle de producto)

---

## 🆘 Soporte

Si tienes dudas o encuentras errores:

1. **Revisa este README**
2. **Consulta la documentación oficial de Microsoft**
3. **Pregunta a tu profesor**
4. **Busca en Stack Overflow:** https://stackoverflow.com/questions/tagged/.net-maui

---

**🎓 Material educativo para estudiantes del ITM**  
*Última actualización: Mayo 2026*
