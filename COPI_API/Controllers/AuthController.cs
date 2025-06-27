using COPI_API.Models;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
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
                role = servidor.Role,
                nome = servidor.Nome,
                email = servidor.Email
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // Verifica se o e-mail já existe
            if (await _context.Servidores.AnyAsync(s => s.Email == dto.Email))
                return BadRequest("Já existe um usuário com este email.");

            // Verifica se a divisão informada existe
            var divisaoExiste = await _context.Divisoes.AnyAsync(d => d.Id == dto.DivisaoId);
            if (!divisaoExiste)
                return BadRequest("A divisão informada não existe.");

            // Cria o servidor
            var servidor = new Servidor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Usuario",
                DivisaoId = dto.DivisaoId 
            };

            _context.Servidores.Add(servidor);
            await _context.SaveChangesAsync();

            return Ok("Servidor registrado com sucesso.");
        }


        [HttpPost("promover")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> PromoverUsuario([FromBody] PromoverDTO dto)
        {
            var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Email == dto.Email);
            if (servidor == null)
                return NotFound("Usuário não encontrado.");

            servidor.Role = dto.NovaRole;
            _context.Servidores.Update(servidor);
            await _context.SaveChangesAsync();

            return Ok($"Usuário {servidor.Nome} promovido para {dto.NovaRole}.");
        }

        [HttpPost("alterar-senha")]
        [Authorize]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Email == email);
            if (servidor == null)
                return NotFound("Usuário não encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, servidor.PasswordHash))
                return BadRequest("Senha atual incorreta.");

            servidor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            _context.Servidores.Update(servidor);
            await _context.SaveChangesAsync();

            return Ok("Senha alterada com sucesso.");
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMeuPerfil()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var servidor = await _context.Servidores
                .Include(s => s.Divisao)
                .FirstOrDefaultAsync(s => s.Email == email);

            if (servidor == null) return NotFound();

            return Ok(new ServidorDTO
            {
                Id = servidor.Id,
                Nome = servidor.Nome,
                Email = servidor.Email,
                DivisaoId = servidor.DivisaoId,
                Celular = servidor.Celular,
                Status = servidor.Status,
                CargoOuFuncao = servidor.CargoOuFuncao,
                PontoSei = servidor.PontoSei,
                ChefiaImediata = servidor.ChefiaImediata,
                RF = servidor.RF,
                HorarioEntrada = servidor.HorarioEntrada,
                HorarioSaida = servidor.HorarioSaida,
                NomeDivisao = servidor.Divisao?.Nome
            });
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] ServidorDTO dados)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Email == email);

            if (servidor == null) return NotFound();

            servidor.Nome = dados.Nome;
            servidor.Celular = dados.Celular;
            servidor.Status = dados.Status;
            servidor.CargoOuFuncao = dados.CargoOuFuncao;
            servidor.PontoSei = dados.PontoSei;
            servidor.ChefiaImediata = dados.ChefiaImediata;
            servidor.RF = dados.RF;
            servidor.HorarioEntrada = dados.HorarioEntrada;
            servidor.HorarioSaida = dados.HorarioSaida;
            servidor.DivisaoId = dados.DivisaoId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("todos")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTodosServidores()
        {
            var servidores = await _context.Servidores
                .Include(s => s.Divisao)
                .Select(s => new ServidorDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email,
                    DivisaoId = s.DivisaoId,
                    Role = s.Role,
                    Celular = s.Celular,
                    Status = s.Status,
                    CargoOuFuncao = s.CargoOuFuncao,
                    PontoSei = s.PontoSei,
                    ChefiaImediata = s.ChefiaImediata,
                    RF = s.RF,
                    HorarioEntrada = s.HorarioEntrada,
                    HorarioSaida = s.HorarioSaida,
                    NomeDivisao = s.Divisao.Nome
                })
                .ToListAsync();

            return Ok(servidores);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteServidor(int id)
        {
            var servidor = await _context.Servidores.FindAsync(id);
            if (servidor == null)
                return NotFound();

            _context.Servidores.Remove(servidor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("me/dados-profissionais")]
        [Authorize]
        public async Task<IActionResult> AtualizarDadosProfissionais([FromBody] ServidorDadosProfissionaisDTO dados)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Email == email);

            if (servidor == null) return NotFound();

            servidor.Celular = dados.Celular;
            servidor.Status = dados.Status;
            servidor.CargoOuFuncao = dados.CargoOuFuncao;
            servidor.PontoSei = dados.PontoSei;
            servidor.ChefiaImediata = dados.ChefiaImediata;
            servidor.RF = dados.RF;
            servidor.HorarioEntrada = dados.HorarioEntrada;
            servidor.HorarioSaida = dados.HorarioSaida;

            await _context.SaveChangesAsync();

            return Ok("Dados profissionais atualizados com sucesso.");
        }

        [HttpPost("resetar-senha/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetarSenha(int id)
        {
            var servidor = await _context.Servidores.FindAsync(id);
            if (servidor == null)
                return NotFound("Servidor não encontrado.");

            string senhaPadrao = "Trocar123!";
            servidor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(senhaPadrao);

            _context.Servidores.Update(servidor);
            await _context.SaveChangesAsync();

            return Ok($"Senha do usuário {servidor.Nome} foi redefinida para a senha padrão.");
        }



        private string GenerateJwtToken(Servidor servidor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "sua_chave_super_secreta");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, servidor.Id.ToString()),
                new Claim(ClaimTypes.Email, servidor.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, servidor.Role)
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
