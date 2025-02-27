using ApiDockerTecnimotors.Repositories.Distribuidores.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistribuidoresController(IDistribuidoresRepository IDistribuidoresReposi) : ControllerBase
    {
        private readonly IDistribuidoresRepository idistribuidore = IDistribuidoresReposi;

        [HttpGet("ListadoDistribuidores")]
        public async Task<ActionResult> ListadoDistribuidores()
        {
            var result = await idistribuidore.ListadoDistribuidores();
            return Ok(result);
        }

        [HttpGet("DetailDistribuidores/{idDistribuidores}")]
        public async Task<ActionResult> DetailDistribuidores(int idDistribuidores)
        {
            var result = await idistribuidore.DetailDistribuidores(idDistribuidores);
            return Ok(result);
        }

        [HttpGet("ListadoDetalleDistribuidore/{Depa}/{Provin}/{Distri}")]
        public async Task<ActionResult> ListadoDetalleDistribuidore(string Depa, string Provin, string Distri)
        {
            var result = await idistribuidore.ListadoDetalleDistribuidore(Depa, Provin, Distri);
            return Ok(result); 
        }
    }
}
