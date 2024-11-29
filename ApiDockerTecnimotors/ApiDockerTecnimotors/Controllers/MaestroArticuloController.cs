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

        [HttpGet("GetArticulos")]
        public async Task<ActionResult> GetArticulos()
        {
            var articulos = await imaestroArticuloRepository.GetArticulosAsync();
            return Ok(articulos);
        }

        [HttpGet("GetArticulosAsync")]
        public async Task<ActionResult> GetArticulosAsync()
        {
            var articulos = await imaestroArticuloRepository.GetArticulosAsync();
            return Ok(articulos);
        }

        [HttpGet("GetPrincipalAsync/{limit}/{offset}")]
        public async Task<ActionResult> GetPrincipalAsync(string limit, string offset)
        {
            var articulos = await imaestroArticuloRepository.GetPrincipalAsync(limit, offset);
            return Ok(articulos);
        }

        [HttpPost("FiltroPrincipal")]
        public async Task<ActionResult> FiltroPrincipal([FromBody] TrFrombody trFrombody)
        {
            var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfil(trFrombody.Ancho!, trFrombody.Perfil!);
            var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfil(trFrombody.Ancho!, trFrombody.Perfil!);
            var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfil(trFrombody.Ancho!, trFrombody.Perfil!);
            var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfil(trFrombody.Ancho!, trFrombody.Perfil!);
            var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfil(trFrombody.Ancho!, trFrombody.Perfil!);

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


        [HttpPost("FiltroPrincipalAro")]
        public async Task<ActionResult> FiltroPrincipalAro([FromBody] TrFrombody trFrombody)
        {
            var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfilAro(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Aro!);
            var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfilAro(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Aro!);
            var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfilAro(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Aro!);
            var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfilAro(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Aro!);
            var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfilAro(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Aro!);

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


        [HttpPost("FiltroPrincipalCocada")]
        public async Task<ActionResult> FiltroPrincipalCocada([FromBody] TrFrombody trFrombody)
        {
            var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfilCocada(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Cocada!);
            var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfilCocada(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Cocada!);
            var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfilCocada(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Cocada!);
            var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfilCocada(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Cocada!);
            var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfilCocada(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Cocada!);

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

        [HttpPost("FiltroPrincipalMarca")]
        public async Task<ActionResult> FiltroPrincipalMarca([FromBody] TrFrombody trFrombody)
        {
            var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfilMarca(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Marca!);
            var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfilMarca(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Marca!);
            var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfilMarca(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Marca!);
            var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfilMarca(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Marca!);
            var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfilMarca(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.Marca!);

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

        [HttpPost("FiltroPrincipalTU")]
        public async Task<IActionResult> FiltroPrincipalTU([FromBody] TrFrombody trFrombody)
        {
            var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfilTL(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.TipoUso!);
            var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfilTL(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.TipoUso!);
            var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfilTL(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.TipoUso!);
            var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfilTL(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.TipoUso!);
            var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfilTL(trFrombody.Ancho!, trFrombody.Perfil!, trFrombody.TipoUso!);

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

            /*
            if (trForm.Ancho != "" && trForm.Perfil != "" && trForm.Aro == "" && trForm.Cocada != "" && trForm.Marca == "" && trForm.TipoUso == "")
            {
                var listaroawait = await imaestroArticuloRepository.ListadoAroAnchoPerfilCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!);
                var listcocada = await imaestroArticuloRepository.ListadoCocadaAnchoPerfilCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!);
                var listmarca = await imaestroArticuloRepository.ListadoMarcaAnchoPerfilCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!);
                var listtipouso = await imaestroArticuloRepository.ListadoTipoUsoAnchoPerfilCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!);
                var listarticulo = await imaestroArticuloRepository.ListadoArticuloAnchoPerfilCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!);

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
            else if (trForm.Ancho != "" && trForm.Perfil != "" && trForm.Aro != "" && trForm.Cocada != "" && trForm.Marca == "" && trForm.TipoUso == "")
            {
                var listaroawait = await imaestroArticuloRepository.AllListadoCocadaAro(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!);
                var listcocada = await imaestroArticuloRepository.AllListadoCocadaCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!);
                var listmarca = await imaestroArticuloRepository.AllListadoCocadaMarca(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!);
                var listtipouso = await imaestroArticuloRepository.AllListadoCocadaTipoUso(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!);
                var listarticulo = await imaestroArticuloRepository.AllListadoCocadaArticulo(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!);

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
            else if (trForm.Ancho != "" && trForm.Perfil != "" && trForm.Aro != "" && trForm.Cocada != "" && trForm.Marca != "" && trForm.TipoUso == "")
            {
                var listaroawait = await imaestroArticuloRepository.AllListadoCocadaAro(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!);
                var listcocada = await imaestroArticuloRepository.AllListadoCocadaCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!);
                var listmarca = await imaestroArticuloRepository.AllListadoCocadaMarca(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!);
                var listtipouso = await imaestroArticuloRepository.AllListadoCocadaTipoUso(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!);
                var listarticulo = await imaestroArticuloRepository.AllListadoCocadaArticulo(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!);

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
            else if (trForm.Ancho != "" && trForm.Perfil != "" && trForm.Aro != "" && trForm.Cocada != "" && trForm.Marca != "" && trForm.TipoUso != "")
            {
                var listaroawait = await imaestroArticuloRepository.AllListadoCocadaAro(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!, trForm.TipoUso!);
                var listcocada = await imaestroArticuloRepository.AllListadoCocadaCocada(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!, trForm.TipoUso!);
                var listmarca = await imaestroArticuloRepository.AllListadoCocadaMarca(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!, trForm.TipoUso!);
                var listtipouso = await imaestroArticuloRepository.AllListadoCocadaTipoUso(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!, trForm.TipoUso!);
                var listarticulo = await imaestroArticuloRepository.AllListadoCocadaArticulo(trForm.Ancho!, trForm.Perfil!, trForm.Cocada!, trForm.Aro!, trForm.Marca!, trForm.TipoUso!);

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

            else
            {
                return BadRequest();
            }
            */
        }

        [HttpGet("GetBanner")]
        public ActionResult GetBanner(string ruta)
        {
            FileStream stream = System.IO.File.OpenRead(ruta);
            return File(stream, "image/jpeg");
        }
    }
}