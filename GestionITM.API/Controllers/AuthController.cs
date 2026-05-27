using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionITM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEstudianteRepository _estudianteRepository;

        public AuthController(IConfiguration configuration, IEstudianteRepository estudianteRepository)
        {
            _configuration = configuration;
            _estudianteRepository = estudianteRepository;
        }

        /// <summary>
        /// Autentica un estudiante y genera un token JWT
        /// </summary>
        /// <param name="loginDto">Credenciales del estudiante</param>
        /// <returns>Token JWT si las credenciales son válidas</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Buscar el estudiante por correo en la base de datos
            var estudiante = await _estudianteRepository.ObtenerPorCorreoAsync(loginDto.Correo);

            if (estudiante == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas. Usuario no encontrado." });
            }

            // Nota: En un entorno de producción, se debería validar la contraseña 
            // con hash (bcrypt, etc.). Por simplicidad del taller, validamos
            // que la contraseña sea igual al nombre del estudiante (demo purposes)
            // En producción: usar BCrypt.Verify(loginDto.Contraseña, estudiante.PasswordHash)
            
            // Para el taller: la contraseña válida es "itm2026" para cualquier estudiante registrado
            if (loginDto.Contraseña != "itm2026")
            {
                return Unauthorized(new { Message = "Credenciales inválidas. Contraseña incorrecta." });
            }

            // Generar el token JWT
            var token = GenerarTokenJwt(estudiante.Id, estudiante.Nombre, estudiante.Correo, "Estudiante");

            return Ok(new LoginResponseDto
            {
                Token = token,
                NombreUsuario = estudiante.Nombre,
                Rol = "Estudiante",
                UserId = estudiante.Id
            });
        }

        private string GenerarTokenJwt(int userId, string nombre, string correo, string rol)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key no está configurado en appsettings.json");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Email, correo),
                new Claim(ClaimTypes.Role, rol)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8), // Token válido por 8 horas
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
