using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static COPI_API.Models.DTO.DPEdto;

namespace COPI_API.Controllers.ControllerDPE
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        [HttpPost("dashboard")]
        [Authorize(Roles = "Admin,Gestor,Visualizador")]
        public IActionResult Dashboard([FromBody] RelatorioFiltroDto filtro)
        {
            return Ok();
        }

        [HttpPost("exportar")]
        [Authorize(Roles = "Admin,Gestor")]
        public IActionResult Exportar([FromBody] RelatorioFiltroDto filtro)
        {
            return Ok();
        }
    }

}
