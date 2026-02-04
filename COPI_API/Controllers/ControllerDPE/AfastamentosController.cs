using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static COPI_API.Models.DTO.DPEdto;

namespace COPI_API.Controllers.ControllerEmentario.ControllerEmentario
{
    [ApiController]
    [Route("api/[controller]")]
    public class AfastamentosController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,Operador")]
        public IActionResult Criar([FromBody] AfastamentoCreateDto dto)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Excluir(int id)
        {
            return NoContent();
        }
    }
}
