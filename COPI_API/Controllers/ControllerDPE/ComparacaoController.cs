using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static COPI_API.Models.DTO.DPEdto;

namespace COPI_API.Controllers.ControllerEmentario
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComparacaoController : ControllerBase
    {
        [HttpGet("{afastamentoId}")]
        public ActionResult<ComparacaoResultadoDto> Comparar(int afastamentoId)
        {
            return Ok(new ComparacaoResultadoDto
            {
                PossuiInconsistencias = false,
                CamposDivergentes = new List<string>()
            });
        }
    }

}
