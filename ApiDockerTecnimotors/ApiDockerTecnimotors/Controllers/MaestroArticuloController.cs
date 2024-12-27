using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroArticuloController(IMaestroArticuloRepository IMaestroArticuloRepository) : ControllerBase
    {
        private readonly IMaestroArticuloRepository imaestroArticuloRepository = IMaestroArticuloRepository;

        [HttpGet("ListArticulosGeneral")]
        public async Task<ActionResult> ListArticulosGeneral()
        {
            var articulos = await imaestroArticuloRepository.ListArticulosGeneral();
            return Ok(articulos);
        }

        [HttpGet("GetBanner")]
        public ActionResult GetBanner(string ruta)
        {
            FileStream stream = System.IO.File.OpenRead(ruta);
            return File(stream, "image/jpeg");

        }
        /*---------------------------------------------------------------------------------------------------------------------*/
        [HttpPost("AllfiltroPrincipalCocada")]
        public async Task<ActionResult> AllfiltroPrincipalCocada([FromBody] TrFrombody trForm)
        {
            /*------------------------------------------------------------------string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso-------*/
            var listaroawait = await imaestroArticuloRepository.AllListadoCocadaAro(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listcocada = await imaestroArticuloRepository.AllListadoCocadaCocada(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listmarca = await imaestroArticuloRepository.AllListadoCocadaMarca(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listtipouso = await imaestroArticuloRepository.AllListadoCocadaTipoUso(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listarticulo = await imaestroArticuloRepository.AllListadoCocadaArticulo(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            SearchData tldata = new()
            {
                Listaro = (List<LstmodelAro>)listaroawait,
                Listcocada = (List<LstmodelCodada>)listcocada,
                Listmarca = (List<LstmodelMarca>)listmarca,
                LisTtipouso = (List<LstmodelTipoUso>)listtipouso,
                ListArticulo = (List<TlArticulo>)listarticulo,
            };

            return Ok(tldata);
        }

        [HttpGet("DetalleArticulo/{Id}")]
        public async Task<ActionResult> DetalleArticulo(int Id)
        {
            var detailArt = await imaestroArticuloRepository.DetalleArticulo(Id);
            return Ok(detailArt);
        }

        /*--------------------Camara----------------------*/
        [HttpGet("ListCategorieCamara")]
        public async Task<ActionResult> ListCategorieCamara()
        {
            var detailArt = await imaestroArticuloRepository.ListCategorieCamara();
            return Ok(detailArt);
        }

        [HttpGet("ListModeloCamara/{txtcategoria}/{txtmarca}")]
        public async Task<ActionResult> ListModeloCamara(string txtcategoria, string txtmarca)
        {
            var detailArt = await imaestroArticuloRepository.ListModeloCamara(txtcategoria, txtmarca);
            return Ok(detailArt);
        }

        [HttpGet("ListadoCamaraGeneral/{txtcategoria}/{txtmarca}")]
        public async Task<ActionResult> ListadoCamaraGeneral(string txtcategoria, string txtmarca)
        {
            var detailArt = await imaestroArticuloRepository.ListadoCamaraGeneral(txtcategoria, txtmarca);
            return Ok(detailArt);
        }
    }
}