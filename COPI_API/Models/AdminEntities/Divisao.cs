using System.Collections.Generic;
namespace COPI_API.Models.AdminEntities
{
    public class Divisao
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<Servidor>? Servidores { get; set; }
    }
}
