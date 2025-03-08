using ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface;
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
        /*
        [HttpPost("ActualizarPathImagen")]
        public async Task<ActionResult> ActualizarPathImagen()
        {
            // Obtener todos los artículos
            var articulos = await imaestroclasificado.ListadoGeneralArticulo(); // Asegúrate de que este método devuelva todos los artículos

            foreach (var articulo in articulos)
            {
                // Crear la ruta de la carpeta para el código del artículo
                var carpetaCodigoInterno = Path.Combine(_carpetaImagenes, articulo.Codigo!);

                // Verificar si la carpeta existe
                if (Directory.Exists(carpetaCodigoInterno))
                {
                    // Obtener la lista de imágenes en la carpeta
                    var imagenes = Directory.GetFiles(carpetaCodigoInterno);

                    // Si hay imágenes, asignar la ruta de la primera imagen
                    if (imagenes.Length > 0)
                    {
                        articulo.Pathimagen = imagenes[0]; // Asignar la ruta completa de la primera imagen
                    }
                    else
                    {
                        articulo.Pathimagen = ""; // No hay imágenes, asignar vacio no null
                    }
                }
                else
                {
                    articulo.Pathimagen = ""; // La carpeta no existe,  asignar vacio no null
                }

                // Actualizar la base de datos con la nueva ruta de la imagen
                await imaestroclasificado.ActualizarPathImagen(articulo.Codigo!, articulo.Pathimagen);
            }

            return Ok(new { mensaje = "Rutas de imágenes actualizadas correctamente." });
        }
        */
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
