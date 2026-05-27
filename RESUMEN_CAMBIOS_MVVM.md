# 📋 Resumen de Cambios - Implementación MVVM en App Móvil

## 📅 Fecha: Mayo 20, 2026
## 👨‍💻 Autor: Daniel Villamizar
## 🎓 Proyecto: GestionITM - Instituto Tecnológico Metropolitano

---

## 🎯 ¿Qué se hizo?

Se implementó el **patrón arquitectónico MVVM (Model-View-ViewModel)** en la aplicación móvil del proyecto GestionITM usando **.NET MAUI** y **CommunityToolkit.Mvvm** con **Source Generators**.

---

## 📦 Commits Realizados

### 1️⃣ Commit Principal: `af975a1`
**Título:** `feat: Implementación de patrón MVVM en app móvil .NET MAUI`

**Archivos creados:**
- ✅ Proyecto completo `GestionITM.AppMovil/`
- ✅ `Models/ProfesorModel.cs` - Modelo de datos
- ✅ `ViewModels/ProfesoresViewModel.cs` - Lógica de presentación
- ✅ `Views/ProfesoresPage.xaml` - Interfaz de usuario
- ✅ Configuración de plataformas (Android, iOS, Windows, Mac)
- ✅ Recursos (imágenes, fuentes, estilos)

**Archivos modificados:**
- ✅ `GestionITM.slnx` - Agregado proyecto móvil a la solución

**Líneas de código:** 1,096 insertions

---

### 2️⃣ Commit de Documentación: `81f4608`
**Título:** `docs: Agregada documentación completa de MVVM para estudiantes`

**Archivos creados:**
- ✅ `GestionITM.AppMovil/README_MVVM.md` - Documentación detallada para estudiantes

**Líneas de código:** 369 insertions

---

## 🏗️ Arquitectura Implementada

```
┌──────────────────────────────────────────────────────────┐
│                    MVVM PATTERN                          │
├──────────────────────────────────────────────────────────┤
│                                                          │
│  ┌─────────┐      ┌─────────────┐      ┌────────┐      │
│  │  MODEL  │◄─────│  VIEWMODEL  │◄────►│  VIEW  │      │
│  │ (Datos) │      │  (Lógica)   │      │ (XAML) │      │
│  └─────────┘      └─────────────┘      └────────┘      │
│                           │                             │
│                           ▼                             │
│                  ┌──────────────────┐                   │
│                  │ Source Generators │                   │
│                  │  (Código Auto)   │                   │
│                  └──────────────────┘                   │
└──────────────────────────────────────────────────────────┘
```

---

## 🔧 Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| .NET MAUI | 10.0 | Framework multiplataforma |
| C# | 13.0 | Lenguaje de programación |
| CommunityToolkit.Mvvm | 8.4.0 | Simplificación de MVVM |
| Source Generators | - | Generación automática de código |
| XAML | - | Diseño de interfaces |

---

## 📁 Estructura de Carpetas Creada

```
GestionITM.AppMovil/
│
├── 📱 App.xaml                      # Configuración global de la app
├── 🚪 AppShell.xaml                 # Navegación de la app
├── ⚙️ MauiProgram.cs                # Configuración de servicios (DI)
│
├── 📦 Models/
│   └── ProfesorModel.cs             # Entidad Profesor
│
├── 🎭 ViewModels/
│   └── ProfesoresViewModel.cs       # Lógica de lista de profesores
│
├── 🖼️ Views/
│   ├── ProfesoresPage.xaml          # Interfaz de lista
│   └── ProfesoresPage.xaml.cs       # Code-behind (mínimo)
│
├── 🎨 Resources/
│   ├── Images/                      # Imágenes de la app
│   ├── Fonts/                       # Fuentes personalizadas
│   ├── Styles/                      # Estilos XAML globales
│   └── AppIcon/                     # Íconos de la app
│
├── 🚀 Platforms/
│   ├── Android/                     # Código específico Android
│   ├── iOS/                         # Código específico iOS
│   ├── Windows/                     # Código específico Windows
│   └── MacCatalyst/                 # Código específico Mac
│
└── 📚 README_MVVM.md                # Documentación para estudiantes
```

---

## 🎯 Características Implementadas

### ✅ 1. Patrón MVVM Completo
- Separación clara de responsabilidades (Model-View-ViewModel)
- Uso de `ObservableObject` como clase base
- Implementación de `INotifyPropertyChanged` automático

### ✅ 2. Source Generators
- `[ObservableProperty]` para propiedades reactivas
- `[RelayCommand]` para comandos ejecutables desde XAML
- Generación automática de código en tiempo de compilación

### ✅ 3. Inyección de Dependencias
- ViewModels registrados como `Singleton`
- Views registradas como `Transient`
- Configuración en `MauiProgram.cs`

### ✅ 4. Navegación y Shell
- Configuración de `AppShell` para navegación
- Registro de rutas para navegación por código

### ✅ 5. Recursos y Estilos
- Estilos globales en `Colors.xaml` y `Styles.xaml`
- Fuentes personalizadas (OpenSans)
- Íconos de app multiplataforma

---

## 🎓 Explicación para Estudiantes

### 🧠 Conceptos Clave

#### 1. **Model (Modelo)**
```csharp
public class ProfesorModel
{
	public int Id { get; set; }
	public string Nombre { get; set; }
	public string Apellido { get; set; }
	public string Email { get; set; }
}
```
- 📦 **Analogía:** Una ficha de biblioteca que describe un libro
- 🎯 **Responsabilidad:** Solo contener datos, sin lógica

#### 2. **ViewModel (El Titiritero)**
```csharp
public partial class ProfesoresViewModel : ObservableObject
{
	[ObservableProperty]
	private bool estaCargando;  // ⬅️ Genera automáticamente: EstaCargando

	[RelayCommand]
	private async Task CargarProfesores()
	{
		// Lógica aquí
	}
}
```
- 🎭 **Analogía:** El titiritero que controla las marionetas
- 🎯 **Responsabilidad:** Contiene toda la lógica de presentación
- ⚡ **Magia:** Los Source Generators crean propiedades públicas automáticamente

#### 3. **View (La Marioneta)**
```xml
<ContentPage ...>
	<ActivityIndicator IsRunning="{Binding EstaCargando}" />
	<Button Text="Cargar" Command="{Binding CargarProfesoresCommand}" />
</ContentPage>
```
- 🖼️ **Analogía:** La marioneta que obedece al titiritero
- 🎯 **Responsabilidad:** Solo mostrar datos, sin lógica
- 🔗 **Binding:** Conexión automática con el ViewModel

---

## 🎬 Flujo de Trabajo Implementado

```
👤 Usuario abre ProfesoresPage
	↓
🖼️ View se carga con BindingContext = ProfesoresViewModel
	↓
🎭 ViewModel ejecuta CargarProfesoresCommand automáticamente
	↓
⚙️ ViewModel cambia EstaCargando = true
	↓
🔔 INotifyPropertyChanged notifica a la View
	↓
🔄 View actualiza: ActivityIndicator empieza a girar
	↓
🌐 ViewModel simula carga de datos (2 segundos)
	↓
📦 ViewModel agrega profesores a la lista observable
	↓
⚙️ ViewModel cambia EstaCargando = false
	↓
🔔 INotifyPropertyChanged notifica a la View
	↓
🖼️ View actualiza: Muestra lista de profesores
```

---

## 🔑 Código Clave Explicado

### Source Generator: [ObservableProperty]

**Código que escribiste:**
```csharp
[ObservableProperty]
private bool estaCargando;
```

**Código que el compilador genera automáticamente:**
```csharp
public bool EstaCargando
{
	get => estaCargando;
	set
	{
		if (estaCargando != value)
		{
			estaCargando = value;
			OnPropertyChanged(nameof(EstaCargando));
		}
	}
}
```

**¿Por qué es importante?**
- ⚡ Ahorra 10+ líneas de código por cada propiedad
- 🐛 Elimina errores comunes (olvidar llamar `OnPropertyChanged`)
- 📝 Código más limpio y legible
- 🚀 Aumenta productividad del desarrollador

---

### Source Generator: [RelayCommand]

**Código que escribiste:**
```csharp
[RelayCommand]
private async Task CargarProfesores()
{
	EstaCargando = true;
	await Task.Delay(2000);
	EstaCargando = false;
}
```

**Código que el compilador genera automáticamente:**
```csharp
private AsyncRelayCommand? cargarProfesoresCommand;

public IAsyncRelayCommand CargarProfesoresCommand =>
	cargarProfesoresCommand ??= new AsyncRelayCommand(CargarProfesores);
```

**¿Por qué es importante?**
- 🎯 No necesitas crear comandos manualmente
- 🔒 Soporte automático para `CanExecute`
- ⏳ Manejo automático de estados asíncronos
- 🧹 Menos código repetitivo (boilerplate)

---

## 🐛 Problemas Comunes Documentados

### 1. Error: "Propiedad 'EstaCargando' no existe"
**Causa:** No compilaste después de agregar `[ObservableProperty]`

**Solución:**
```
Compilar → Recompilar Solución
```

---

### 2. Error: "CS0260: Missing partial modifier"
**Causa:** Olvidaste `partial` en la clase ViewModel

```csharp
// ❌ MAL
public class ProfesoresViewModel : ObservableObject

// ✅ BIEN
public partial class ProfesoresViewModel : ObservableObject
```

---

### 3. Error: XAML marca errores en rojo pero compila bien
**Causa:** El IntelliSense de XAML no detecta código generado

**Solución:**
```
1. Compilar solución
2. Cerrar y reabrir el archivo XAML
3. Si persiste: Limpiar solución + Recompilar
```

---

## 📚 Documentación Creada

### 1. **README_MVVM.md** (369 líneas)
Ubicación: `GestionITM.AppMovil/README_MVVM.md`

**Contenido:**
- 🧩 Explicación de los 3 componentes MVVM
- ⚡ La magia de los Source Generators
- 🎨 Atributos mágicos (ObservableProperty, RelayCommand)
- 🚀 Flujo de datos en MVVM
- 🐛 Problemas comunes y soluciones
- 🎓 Ejercicios para practicar
- 🙋 Preguntas frecuentes (FAQ)
- 📖 Recursos adicionales

---

## 📊 Estadísticas del Proyecto

| Métrica | Valor |
|---------|-------|
| Archivos creados | 37 |
| Líneas de código | 1,465+ |
| Clases creadas | 3 (Model, ViewModel, View) |
| Comandos MVVM | 1 (CargarProfesoresCommand) |
| Propiedades observables | 1 (EstaCargando) |
| Páginas XAML | 1 (ProfesoresPage) |
| Plataformas soportadas | 4 (Android, iOS, Windows, Mac) |

---

## 🔄 Próximos Pasos Sugeridos

1. ✅ ~~Implementar MVVM básico~~
2. 🔲 **Conectar con la API REST** (GestionITM.API)
3. 🔲 **Implementar servicio HTTP** para llamadas a API
4. 🔲 **Agregar manejo de errores** con try-catch
5. 🔲 **Implementar navegación** a página de detalle
6. 🔲 **Agregar CRUD completo** (Crear, Editar, Eliminar)
7. 🔲 **Implementar búsqueda** con SearchBar
8. 🔲 **Agregar RefreshView** para pull-to-refresh
9. 🔲 **Implementar caché local** con SQLite
10. 🔲 **Agregar autenticación** con JWT

---

## 🎯 Objetivos de Aprendizaje Alcanzados

Para los estudiantes, este proyecto demuestra:

✅ **Separación de responsabilidades** (MVVM)  
✅ **Generación automática de código** (Source Generators)  
✅ **Inyección de dependencias** (DI)  
✅ **Data binding** (enlace de datos)  
✅ **Comandos en XAML** (RelayCommand)  
✅ **Notificaciones de cambios** (INotifyPropertyChanged)  
✅ **Desarrollo multiplataforma** (.NET MAUI)  
✅ **Arquitectura limpia** (Clean Architecture)  

---

## 📖 Referencias y Recursos

### Documentación Oficial:
1. **.NET MAUI:** https://learn.microsoft.com/dotnet/maui/
2. **MVVM Toolkit:** https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/
3. **Source Generators:** https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/source-generators-overview

### Tutoriales Recomendados:
1. **James Montemagno - .NET MAUI Basics:** https://www.youtube.com/@JamesMontemagno
2. **Microsoft Learn - MVVM:** https://learn.microsoft.com/training/modules/implement-mvvm-pattern/

---

## 🙏 Créditos

**Desarrollado por:** Daniel Villamizar  
**Institución:** Instituto Tecnológico Metropolitano (ITM)  
**Fecha:** Mayo 2026  
**Framework:** .NET MAUI 10.0  
**Patrón:** MVVM con CommunityToolkit.Mvvm  

---

## 📞 Contacto y Soporte

Para consultas sobre este proyecto:
- **Profesor:** [Nombre del profesor]
- **Repositorio:** https://github.com/CSA-DanielVillamizar/580304006-9
- **Issues:** https://github.com/CSA-DanielVillamizar/580304006-9/issues

---

**🎓 Este proyecto es material educativo del ITM**  
*Última actualización: Mayo 20, 2026*
