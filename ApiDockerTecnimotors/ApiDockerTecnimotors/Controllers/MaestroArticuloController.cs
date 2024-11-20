using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroArticuloController(IMaestroArticuloRepository IMaestroArticuloRepository) : ControllerBase
    {
        private readonly IMaestroArticuloRepository imaestroArticuloRepository = IMaestroArticuloRepository;

        [HttpGet("GetArticulos")]
        public async Task<ActionResult> GetArticulos()
        {
            var articulos = await imaestroArticuloRepository.GetArticulosAsync();
            return Ok(articulos);
        }

        [HttpGet("GetMarca")]
        public async Task<ActionResult> GetMarca()
        {
            var articulos = await imaestroArticuloRepository.GetMarcaAsync();
            return Ok(articulos);
        }

        [HttpPost("GetAllSinFiltroArticulo")]
        public async Task<ActionResult> GetAllSinFiltroArticulo([FromBody] Trfrom trfrom)
        {
            var articulos = await imaestroArticuloRepository.GetAllSinFiltroArticulo(trfrom.Value!);
            return Ok(articulos);
        }

        [HttpPost("GetAllFiltroMarcaCocada")]
        public async Task<ActionResult> GetAllFiltroMarcaCocada([FromBody] TrFormvalue trform)
        {
            var articulos = await imaestroArticuloRepository.GetAllFiltroMarcaCocada(trform.Value!, trform.Cocada!);
            return Ok(articulos);
        }
    }
}
