using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COPI_API.Models
{
    public class Servidor
    {
        [Key]
        public int Id { get; set; }

        // Identificação
        public string? Nome { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public string Role { get; set; } = "Usuario"; // padrão

        // Dados de Contato e Funcionamento
        public string? Celular { get; set; }
        public string? Status { get; set; }
        public string? CargoOuFuncao { get; set; }
        public string? PontoSei { get; set; }
        public string? ChefiaImediata { get; set; }
        public string? RF { get; set; }
        public string? HorarioEntrada { get; set; }
        public string? HorarioSaida { get; set; }

        // Relacionamento
        public int DivisaoId { get; set; }
        public Divisao? Divisao { get; set; }

        public ICollection<UnidadeKPI>? UnidadesKpiResponsaveis { get; set; }

        // Auditoria (opcional)
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? UltimoAcesso { get; set; }
    }
}
