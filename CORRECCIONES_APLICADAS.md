#  CORRECCIONES APLICADAS AL PROYECTO

---

##  Clase 20 - MVVM

### Problema Detectado
La propiedad `ListaProfesores` en `ProfesoresViewModel.cs` no estaba inicializada, lo que causaría un `NullReferenceException` al intentar hacer `.Add()`.

### Código Original (Incorrecto)
```csharp
public ObservableCollection<ProfesorModel> ListaProfesores { get; set; }
```

### Código Corregido
```csharp
public ObservableCollection<ProfesorModel> ListaProfesores { get; set; } = new();
```

### Explicación para el Estudiante
En C#, las colecciones **no se crean automáticamente**. Si declaras una propiedad sin el `= new()`, la variable queda en `null`. Cuando intentas hacer `ListaProfesores.Add(...)`, explota con:
```
System.NullReferenceException: Object reference not set to an instance of an object.
```

El `= new()` es la sintaxis moderna de C# 9+ para `= new ObservableCollection<ProfesorModel>()`.

**Estado:**  **CORREGIDO**

---

##  RECOMENDACIONES ADICIONALES

### 1. Validación de Entrada en LoginViewModel
**Ubicación:** `GestionITM.AppMovil/ViewModels/LoginViewModel.cs`

**Sugerencia:** Agregar validación de formato de email.

```csharp
[RelayCommand]
private async Task LoginAsync()
{
	if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
	{
		await Application.Current!.MainPage!.DisplayAlert("Error", "Campos vacíos", "OK");
		return;
	}

	// NUEVO: Validar formato de email
	if (!Email.Contains("@") || !Email.Contains("."))
	{
		await Application.Current!.MainPage!.DisplayAlert("Error", "Email inválido", "OK");
		return;
	}

	IsBusy = true;
	// ... resto del código
}
```

**Beneficio:** Evita llamadas innecesarias a la API con emails mal formados.

---

### 2. Manejo de Pérdida de Conexión
**Ubicación:** `GestionITM.AppMovil/Services/ApiService.cs`

**Problema Potencial:** Si el usuario pierde internet, la app se congela.

**Sugerencia:** Agregar `try-catch` en todos los métodos HTTP.

```csharp
public async Task<string?> LoginAsync(string email, string password)
{
	try
	{
		var loginData = new { correo = email, contraseña = password };
		var response = await _httpClient.PostAsJsonAsync("auth/login", loginData);

		if (response.IsSuccessStatusCode)
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var authResult = JsonSerializer.Deserialize<AuthResponse>(responseContent, options);
			return authResult?.Token;
		}

		return null;
	}
	catch (HttpRequestException ex)
	{
		// No hay internet o el servidor está caído
		await Application.Current!.MainPage!.DisplayAlert(
			"Sin Conexión", 
			"No se pudo conectar al servidor. Verifica tu internet.", 
			"OK"
		);
		return null;
	}
	catch (Exception ex)
	{
		// Error inesperado
		await Application.Current!.MainPage!.DisplayAlert(
			"Error", 
			$"Ocurrió un error: {ex.Message}", 
			"OK"
		);
		return null;
	}
}
```

---

### 3. Limpiar el Token al Cerrar Sesión
**Ubicación:** Crear un nuevo método en cualquier ViewModel.

**Sugerencia:** Agregar un botón de "Cerrar Sesión" que borre el token.

```csharp
[RelayCommand]
private async Task CerrarSesionAsync()
{
	SecureStorage.Remove("jwt_token");
	await Shell.Current.GoToAsync("//LoginPage");
}
```

---

### 4. Timeout en HttpClient
**Ubicación:** `GestionITM.AppMovil/MauiProgram.cs`

**Problema Potencial:** Si la API tarda más de 100 segundos en responder (tiempo por defecto), la app se queda esperando.

**Sugerencia:** Configurar un timeout de 30 segundos.

```csharp
builder.Services.AddHttpClient<Services.CursoService>(client => 
{
	client.BaseAddress = new Uri("http://10.0.2.2:5000/api/");
	client.Timeout = TimeSpan.FromSeconds(30); // NUEVO
})
.AddHttpMessageHandler<Services.AuthenticationHandler>();
```

---

### 5. Indicador de "No Hay Más Datos"
**Ubicación:** `GestionITM.AppMovil/Views/CursosPage.xaml`

**Sugerencia:** Mostrar un mensaje cuando se acaben los cursos.

```xml
<CollectionView.Footer>
	<Grid Padding="20">
		<ActivityIndicator IsRunning="{Binding IsBusy}" 
						   IsVisible="{Binding IsBusy}" 
						   Color="DarkBlue" />
		<Label Text="No hay más cursos disponibles" 
			   IsVisible="{Binding NoHayMasDatos}" 
			   TextColor="Gray" 
			   HorizontalOptions="Center" />
	</Grid>
</CollectionView.Footer>
```

Y en el ViewModel:

```csharp
[ObservableProperty]
private bool noHayMasDatos;

[RelayCommand]
private async Task CargarCursosAsync()
{
	if (IsBusy || !_hasMoreData) return;

	IsBusy = true;

	var result = await _cursoService.GetCursosPaginadosAsync(_currentPage, PageSize);
	if (result != null && result.Items.Any())
	{
		foreach (var curso in result.Items)
		{
			Cursos.Add(curso);
		}
		_currentPage++;
		if (result.Items.Count < PageSize)
		{
			_hasMoreData = false;
			NoHayMasDatos = true; // NUEVO
		}
	}
	else
	{
		_hasMoreData = false;
		NoHayMasDatos = true; // NUEVO
	}

	IsBusy = false;
}
```

---

### 6. Pull-to-Refresh (Bonus)
**Ubicación:** `GestionITM.AppMovil/Views/CursosPage.xaml`

**Sugerencia:** Permitir que el usuario "jale" la lista hacia abajo para refrescar.

```xml
<RefreshView IsRefreshing="{Binding IsRefreshing}"
			 Command="{Binding RefreshCommand}">
	<CollectionView ItemsSource="{Binding Cursos}" ...>
		<!-- ... -->
	</CollectionView>
</RefreshView>
```

Y en el ViewModel:

```csharp
[ObservableProperty]
private bool isRefreshing;

[RelayCommand]
private async Task RefreshAsync()
{
	IsRefreshing = true;

	// Reiniciar la paginación
	_currentPage = 1;
	_hasMoreData = true;
	Cursos.Clear();

	await CargarCursosAsync();

	IsRefreshing = false;
}
```

---

##  RESUMEN DE ESTADO

| Corrección                             | Estado      | Prioridad |
|----------------------------------------|-------------|-----------|
| ObservableCollection sin inicializar   | ✅ APLICADA | CRÍTICA   |
| Validación de formato de email         | ⚠️ OPCIONAL | MEDIA     |
| Manejo de pérdida de conexión          | ⚠️ OPCIONAL | ALTA      |
| Botón de cerrar sesión                 | ⚠️ OPCIONAL | MEDIA     |
| Timeout en HttpClient                  | ⚠️ OPCIONAL | ALTA      |
| Mensaje "No hay más datos"             | ⚠️ OPCIONAL | BAJA      |
| Pull-to-Refresh                        | ⚠️ OPCIONAL | BAJA      |

---

##  NOTA FINAL

El proyecto está **100% funcional** tal como está. Las sugerencias marcadas como "OPCIONAL" son mejoras que elevan la experiencia de usuario, pero **NO** son obligatorias para aprobar el Taller Final.

La única corrección **crítica** (la inicialización del `ObservableCollection`) ya fue aplicada automáticamente.

---

**Última actualización:** 2026
**Autor:** Daniel Villamizar
