namespace COPI_API.Models.DTO
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    // DTO para registro
    public class RegisterDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int DivisaoId { get; set; } // ✅ Adicionado
    }

    // DTO para promoção
    public class PromoverDTO
    {
        public string Email { get; set; }
        public string NovaRole { get; set; } // "Admin", "Gestor", "UsuarioPlus", "UsuarioComum"
    }
    public class AlterarSenhaDTO
    {
        public string Email { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }

    public class ServidorDadosProfissionaisDTO
    {
        public string Celular { get; set; }
        public string Status { get; set; }
        public string CargoOuFuncao { get; set; }
        public string PontoSei { get; set; }
        public string ChefiaImediata { get; set; }
        public string RF { get; set; }
        public string? HorarioEntrada { get; set; }
        public string? HorarioSaida { get; set; }
    }




}
