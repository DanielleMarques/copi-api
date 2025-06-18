using COPI_API.Models;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace COPI_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var servidor = await _context.Servidores
                .FirstOrDefaultAsync(s => s.Email == loginDto.Email);

            if (servidor == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, servidor.PasswordHash))
                return Unauthorized("Credenciais inválidas");

            var token = GenerateJwtToken(servidor);

            return Ok(new
            {
                token,
                role = "Servidor", // Se quiser expandir para roles diferentes, pode usar um campo na tabela
                nome = servidor.Nome,
                email = servidor.Email
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (await _context.Servidores.AnyAsync(s => s.Email == dto.Email))
                return BadRequest("Já existe um usuário com este email.");

            var servidor = new Servidor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "UsuarioComum"
            };

            _context.Servidores.Add(servidor);
            await _context.SaveChangesAsync();

            return Ok("Servidor registrado com sucesso.");
        }

        [HttpPost("promover")]
        public async Task<IActionResult> PromoverUsuario([FromBody] PromoverDTO dto)
        {
            var solicitanteRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (solicitanteRole != "Admin")
                return Forbid("Apenas administradores podem promover usuários.");

            var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Email == dto.Email);
            if (servidor == null)
                return NotFound("Usuário não encontrado.");

            servidor.Role = dto.NovaRole;
            _context.Servidores.Update(servidor);
            await _context.SaveChangesAsync();

            return Ok($"Usuário {servidor.Nome} promovido para {dto.NovaRole}.");
        }

        private string GenerateJwtToken(Servidor servidor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "sua_chave_super_secreta");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, servidor.Id.ToString()),
                new Claim(ClaimTypes.Email, servidor.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, "Servidor")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

