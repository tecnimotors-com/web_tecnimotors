using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroClasificadoController(IMaestroClasificado IMaestroClasificadoRepository) : ControllerBase
    {
        private readonly IMaestroClasificado imaestroclasificado = IMaestroClasificadoRepository;
        private readonly string _carpetaImagenes = Path.Combine(Directory.GetCurrentDirectory(), "Imagen");

        [HttpGet("ListadoCategoriaVehiculos")]
        public async Task<ActionResult> ListadoCategoriaVehiculos()
        {
            var articulos = await imaestroclasificado.ListadoCategoriaVehiculos();
            return Ok(articulos);
        }

        [HttpGet("ListadoGeneralCategoria/{motocicleta}")]
        public async Task<ActionResult> ListadoGeneralCategoria(string motocicleta)
        {
            var articulos = await imaestroclasificado.ListadoGeneralCategoria(motocicleta);
            return Ok(articulos);
        }

        [HttpPost("ListadoGeneralVehiculos")]
        public async Task<ActionResult> ListadoGeneralVehiculos([FromBody] GeneralVehiculo Trgeneral)
        {
            var articulos = await imaestroclasificado.ListadoGeneralVehiculos(Trgeneral.Categoria!, Trgeneral.Medida!, Trgeneral.Marca!);

            return Ok(new { count = articulos.Count(), list = articulos });
        }

        [HttpPost("ListadoModeloVehiculo")]
        public async Task<ActionResult> ListadoModeloVehiculo([FromBody] GeneralModelo TrModelo)
        {

            var modelo = await imaestroclasificado.ListadoModeloVehiculo(TrModelo.Categoria!, TrModelo.Marca!);

            return Ok(new { count = modelo.Count(), list = modelo });
        }

        [HttpPost("ListarMarcaVehiculo")]
        public async Task<ActionResult> ListarMarcaVehiculo([FromBody] GeneralMarca TrModelo)
        {

            var modelo = await imaestroclasificado.ListarMarcaVehiculo(TrModelo.Categoria!, TrModelo.Medida!);

            return Ok(new { count = modelo.Count(), list = modelo });
        }

        [HttpGet("DetalleVehiculo/{Id}")]
        public async Task<ActionResult> DetalleVehiculo(int Id)
        {
            // Obtener los detalles del vehículo
            var detailArt = await imaestroclasificado.DetalleVehiculo(Id);

            // Verificar si se obtuvo el detalle
            if (detailArt == null)
            {
                return NotFound("El vehículo no fue encontrado.");
            }

            // Obtener el código interno del detalle
            var codigoInterno = detailArt.Codigo; // Asegúrate de que el código interno esté en el modelo

            // Crear la ruta de la carpeta para el código interno
            var carpetaCodigoInterno = Path.Combine(Directory.GetCurrentDirectory(), "Imagen", codigoInterno!);

            // Verificar si la carpeta existe
            if (!Directory.Exists(carpetaCodigoInterno))
            {
                return Ok(new
                {
                    listado = detailArt,
                    codigointerno = codigoInterno,
                    TotalImagenes = 0,
                    RutasOriginales = new List<string>(), // Lista vacía si no hay imágenes
                    PrimeraRutaOriginal = "" // Asignar cadena vacía si no hay ruta
                });
            }

            // Obtener las imágenes existentes en la carpeta
            var imagenesExistentes = Directory.GetFiles(carpetaCodigoInterno);
            var rutasOriginales = imagenesExistentes.ToList(); // Guardar rutas completas
            int totalImagenes = rutasOriginales.Count;

            // Obtener la primera ruta, si existe
            //var primeraRuta = rutasOriginales.FirstOrDefault() ?? "";
            var primeraRuta = rutasOriginales
                .Where(ruta => Path.GetFileName(ruta).StartsWith("1")) // Filtrar por nombre de archivo
                .OrderBy(ruta => ruta) // Ordenar si es necesario
                .FirstOrDefault() ?? "";// Asignar cadena vacía si no hay ruta

            // Devolver la información
            return Ok(new
            {
                listado = detailArt,
                codigointerno = codigoInterno,
                TotalImagenes = totalImagenes,
                RutasOriginales = rutasOriginales,
                PrimeraRutaOriginal = primeraRuta // Solo la primera ruta
            });
        }
        [HttpPost("ActualizarPathImagen2")]
        public async Task<ActionResult> ActualizarPathImagen2()
        {
            // Obtener todos los artículos
            var articulos = await imaestroclasificado.ListadoGeneralArticulo();
            var actualizaciones = new List<(string Codigo, string PathImagen)>();

            foreach (var articulo in articulos)
            {
                var carpetaCodigoInterno = Path.Combine(_carpetaImagenes, articulo.Codigo!);

                if (Directory.Exists(carpetaCodigoInterno))
                {
                    // Obtener la lista de imágenes en la carpeta
                    var imagenes = Directory.GetFiles(carpetaCodigoInterno);

                    // Filtrar las imágenes que comienzan con un número
                    var imagenSeleccionada = imagenes
                        .Where(imagen => Path.GetFileName(imagen).StartsWith("1-")) // Cambia "1-" por el número que necesites
                        .FirstOrDefault();

                    // Asignar la ruta de la imagen seleccionada o vacía si no se encuentra
                    articulo.Pathimagen = imagenSeleccionada ?? "";
                }
                else
                {
                    articulo.Pathimagen = "";
                }

                // Acumular las actualizaciones
                actualizaciones.Add((articulo.Codigo!, articulo.Pathimagen));
            }

            // Actualizar la base de datos en un solo paso
            await imaestroclasificado.ActualizarPathImagenBatch(actualizaciones);

            return Ok(new { mensaje = "Rutas de imágenes actualizadas correctamente." });
        }

        [HttpGet("GetBanner1/{ruta}")]

        public ActionResult GetBanner1(string ruta)
        {
            FileStream stream = System.IO.File.OpenRead(ruta);
            return File(stream, "image/jpeg");
        }

        [HttpGet("GetBanner2")]

        public ActionResult GetBanner2(string ruta) 
        {
            FileStream stream = System.IO.File.OpenRead(ruta);
            return File(stream, "image/jpeg");
        }

        [HttpPost("GetBanner3")]

        public ActionResult GetBanner3([FromForm] Trruta ruta)
        {
            FileStream stream = System.IO.File.OpenRead(ruta.Ruta!);
            return File(stream, "image/jpeg");
        }

        [HttpPost("GetBanner4")]

        public ActionResult GetBanner4([FromBody] Trruta ruta)
        {
            FileStream stream = System.IO.File.OpenRead(ruta.Ruta!);
            return File(stream, "image/jpeg");
        }

        [HttpGet("TipoMarcaAceite")]
        public async Task<ActionResult> TipoMarcaAceite()
        {
            var result = await imaestroclasificado.TipoMarcaAceite();
            return Ok(result);
        }

        [HttpPost("ListadoGeneralAceite")]
        public async Task<ActionResult> ListadoGeneralAceite([FromBody] TrAceite traceite)
        {
            var result = await imaestroclasificado.ListadoGeneralAceite(traceite.TipoMarca!);
            return Ok(result);
        }

        [HttpGet("ListadoRepuestoCategoria")]
        public async Task<ActionResult> ListadoRepuestoCategoria()
        {
            var result = await imaestroclasificado.ListadoRepuestoCategoria();
            return Ok(result);
        }

        [HttpPost("ListadoRepuestoMarca")]
        public async Task<ActionResult> ListadoRepuestoMarca(TrCatrepo trcat)
        {
            var result = await imaestroclasificado.ListadoRepuestoMarca(trcat.Categoria!);
            return Ok(result);
        }

        [HttpPost("ListadoGeneralRepuesto")]
        public async Task<ActionResult> ListadoGeneralRepuesto(TrMarrepo trmar)
        {
            var result = await imaestroclasificado.ListadoGeneralRepuesto(trmar.Categoria!, trmar.Marca!);
            return Ok(result);
        }

        [HttpGet("ListadoTipoCamaras")]
        public async Task<ActionResult> ListadoTipoCamaras()
        {
            var result = await imaestroclasificado.ListadoTipoCamaras();
            return Ok(result);
        }

        [HttpPost("ListadoCamaraMarca")]
        public async Task<ActionResult> ListadoCamaraMarca(TrCatrepo trcat)
        {
            var result = await imaestroclasificado.ListadoCamaraMarca(trcat.Categoria!);
            return Ok(result);
        }

        [HttpPost("ListadoGeneralCamara")]
        public async Task<ActionResult> ListadoGeneralCamara(TrMarrepo trmar)
        {
            var result = await imaestroclasificado.ListadoGeneralCamara(trmar.Categoria!, trmar.Marca!);
            return Ok(result);
        }

        /*---------------------------------------------------------------------------------------------------------------------*/
        [HttpGet("ListadoAnchoPerfilLLANTA")]
        public async Task<ActionResult> ListadoAnchoPerfilLLANTA()
        {
            var articulos = await imaestroclasificado.ListadoAnchoPerfilLLANTA();
            return Ok(articulos);
        }

        [HttpPost("AllfiltroPrincipalLLanta")]
        public async Task<ActionResult> AllfiltroPrincipalLLanta([FromBody] TrFrombodyLlanta trForm)
        {
            /*------------------------------------------------------------------string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso-------*/
            var listaroawait = await imaestroclasificado.AllListadoCocadaAroLLANTA(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listcocada = await imaestroclasificado.AllListadoCocadaCocadaLLANTA(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listmarca = await imaestroclasificado.AllListadoCocadaMarcaLLANTA(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listtipouso = await imaestroclasificado.AllListadoCocadaTipoUsoLLANTA(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            var listarticulo = await imaestroclasificado.AllListadoCocadaArticuloLLANTA(trForm.Ancho!, trForm.Perfil!, trForm.Aro!, trForm.Cocada!, trForm.Marca!, trForm.TipoUso!);
            SearchDataLlanta tldata = new()
            {
                Listaro = (List<LstmodelAro>)listaroawait,
                Listcocada = (List<LstmodelCodada>)listcocada,
                Listmarca = (List<LstmodelMarca>)listmarca,
                LisTtipouso = (List<LstmodelTipoUso>)listtipouso,
                ListArticulo = (List<TlArticulo>)listarticulo,
            };

            return Ok(tldata);
        }

        [HttpPost("ListadoLLanta")]
        public async Task<ActionResult> ListadoLLanta([FromBody] TlModelsFilter trmdelfilter)
        {
            var medidas = await imaestroclasificado.ListadoLLanta("medida", trmdelfilter);
            var modelos = await imaestroclasificado.ListadoLLanta("modelo", trmdelfilter);
            var marcas = await imaestroclasificado.ListadoLLanta("marca", trmdelfilter);
            var categorias = await imaestroclasificado.ListadoLLanta("categoria", trmdelfilter);
            var generallanta = await imaestroclasificado.ListadoGeneralLlantas(trmdelfilter);

            return Ok(new
            {
                medidas,
                modelos,
                marcas,
                categorias,
                generallanta,
            });
        }

        [HttpPost("ListadoGeneralLlantas")]
        public async Task<ActionResult> ListadoGeneralLlantas([FromBody] TlModelsFilter trmdelfilter)
        {
            var LlantaGeneral = await imaestroclasificado.ListadoGeneralLlantas(trmdelfilter);
            return Ok(LlantaGeneral);
        }
    }
    public class TrCatrepo
    {
        public string? Categoria { get; set; }
    }
    public class TrMarrepo
    {
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
    }

    public class Trruta
    {
        public string? Ruta { get; set; }
    }
    public class GeneralVehiculo
    {
        public string? Categoria { get; set; }
        public string? Medida { get; set; }
        public string? Marca { get; set; }
    }

    public class GeneralModelo
    {
        public string? Categoria { get; set; }
        public string? Marca { get; set; }

    }
    public class GeneralMarca
    {
        public string? Categoria { get; set; }
        public string? Medida { get; set; }
    }
}
