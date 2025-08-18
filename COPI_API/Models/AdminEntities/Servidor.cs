using COPI_API.Models.PIBPEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace COPI_API.Models.AdminEntities
{
    public class Servidor
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Usuario";
        public string? Celular { get; set; }
        public string? Status { get; set; }
        public string? CargoOuFuncao { get; set; }
        public string? PontoSei { get; set; }
        public string? ChefiaImediata { get; set; }
        public string? RF { get; set; }
        public string? HorarioEntrada { get; set; }
        public string? HorarioSaida { get; set; }
        [ForeignKey("Divisao")]
        public int DivisaoId { get; set; }
        public Divisao? Divisao { get; set; }
        public ICollection<UnidadeKPI>? UnidadesKpiResponsaveis { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcesso { get; set; }
    }
}
