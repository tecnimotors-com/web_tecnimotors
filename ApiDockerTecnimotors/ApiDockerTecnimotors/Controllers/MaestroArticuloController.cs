using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet("DetalleCamaraAll/{Id}")]
        public async Task<ActionResult> DetalleCamaraAll(int Id)
        {
            var detailArt = await imaestroArticuloRepository.DetalleCamaraAll(Id);
            return Ok(detailArt);
        }

        [HttpGet("ListadoCamaraGeneralModelo/{IdCamara}")]
        public async Task<ActionResult> ListadoCamaraGeneralModelo(string IdCamara)
        {
            var detailArt = await imaestroArticuloRepository.ListadoCamaraGeneralModelo(IdCamara);
            return Ok(detailArt);
        }

        /*--------------------Aceite----------------------*/
        [HttpGet("ListCategorieAceite")]
        public async Task<ActionResult> ListCategorieAceite()
        {
            var detailArt = await imaestroArticuloRepository.ListCategorieAceite();
            return Ok(detailArt);
        }

        [HttpGet("ListModeloAceite/{txtcategoria}")]
        public async Task<ActionResult> ListModeloAceite(string txtcategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListModeloAceite(txtcategoria);
            return Ok(detailArt);
        }

        [HttpGet("ListadoAceiteGeneral/{txtcategoria}")]
        public async Task<ActionResult> ListadoAceiteGeneral(string txtcategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListadoAceiteGeneral(txtcategoria);
            return Ok(detailArt);
        }
        /*--------------------Vehiculo----------------------*/
        [HttpGet("ListModeloVehiculo/{txtcategoria}")]
        public async Task<ActionResult> ListModeloVehiculo(string txtcategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListModeloVehiculo(txtcategoria);
            return Ok(detailArt);
        }

        [HttpGet("ListadoVehiculoGeneral/{txtcategoria}")]
        public async Task<ActionResult> ListadoVehiculoGeneral(string txtcategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListadoVehiculoGeneral(txtcategoria);
            return Ok(detailArt);
        }
        /*--------------------Repuesto----------------------*/
        [HttpGet("ListadoRepuestoTipoCategoria/{txtcategoria}")]
        public async Task<ActionResult> ListadoRepuestoTipoCategoria(string txtcategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListadoRepuestoTipoCategoria(txtcategoria);
            return Ok(detailArt);
        }
        [HttpGet("ListadoModeloRepuesto/{TipoCategoria}/{Categoria}")]
        public async Task<ActionResult> ListadoModeloRepuesto(string TipoCategoria, string Categoria)
        {
            var detailArt = await imaestroArticuloRepository.ListadoModeloRepuesto(TipoCategoria, Categoria);
            return Ok(detailArt);
        }
        [HttpGet("ListadoRepuestoGeneralALl/{TipoCategoria}")]
        public async Task<ActionResult> ListadoRepuestoGeneralALl(string TipoCategoria)
        {
            var detailArt = await imaestroArticuloRepository.ListadoRepuestoGeneralALl(TipoCategoria);
            return Ok(detailArt);
        }
    }
}