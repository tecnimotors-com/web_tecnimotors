using ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly string _carpetaImagenes = Path.Combine(Directory.GetCurrentDirectory(), "Imagen");
        private readonly IMaestroClasificado _maestroClasificadoRepository;

        public ImagesController(IMaestroClasificado maestroClasificadoRepository)
        {
            _maestroClasificadoRepository = maestroClasificadoRepository;

            // Crear la carpeta si no existe
            if (!Directory.Exists(_carpetaImagenes))
            {
                Directory.CreateDirectory(_carpetaImagenes);
            }
        }

        [HttpPost("subir")]
        public async Task<IActionResult> SubirImagenes([FromForm] List<IFormFile> archivos, [FromForm] string codigoInterno)
        {
            // Validar si el código interno está vacío
            if (string.IsNullOrWhiteSpace(codigoInterno))
            {
                return BadRequest(new
                {
                    codigointerno = codigoInterno,
                    Mensaje = "El código interno está vacío."
                });
            }

            // Validar el código interno en la base de datos
            var existeCodigo = await _maestroClasificadoRepository.ExisteCodigoInterno(codigoInterno.Trim());

            if (!existeCodigo)
            {
                return BadRequest("El código interno no existe.");
            }

            // Validar si la lista de archivos está vacía
            if (archivos == null || archivos.Count == 0)
            {
                return BadRequest(new
                {
                    codigointerno = codigoInterno,
                    Mensaje = "No se subieron imágenes. La lista de archivos está vacía."
                });
            }

            // Crear la carpeta para el código interno
            var carpetaCodigoInterno = Path.Combine(_carpetaImagenes, codigoInterno);
            if (!Directory.Exists(carpetaCodigoInterno))
            {
                Directory.CreateDirectory(carpetaCodigoInterno);
            }

            // Obtener las imágenes existentes en la carpeta
            var imagenesExistentes = Directory.GetFiles(carpetaCodigoInterno);
            var rutasExistentes = imagenesExistentes.ToList(); // Guardar rutas completas
            int cantidadExistentes = rutasExistentes.Count;

            var rutasArchivosNuevos = new List<string>();
            var archivosDuplicados = new List<string>();
            int imagenesSubidas = 0;

            foreach (var archivo in archivos)
            {
                if (archivo == null || archivo.Length == 0)
                {
                    continue; // Ignorar archivos no válidos
                }

                var rutaArchivo = Path.Combine(carpetaCodigoInterno, archivo.FileName);

                // Verificar si el archivo ya existe
                if (System.IO.File.Exists(rutaArchivo))
                {
                    // Si el archivo ya existe, agregarlo a la lista de duplicados
                    archivosDuplicados.Add(archivo.FileName);
                    continue; // Ignorar archivos duplicados
                }

                try
                {
                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    rutasArchivosNuevos.Add(rutaArchivo); // Guardar ruta completa
                    imagenesSubidas++;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción (puedes registrar el error si es necesario)
                    Console.WriteLine($"Error al subir la imagen {archivo.FileName}: {ex.Message}");
                }
            }

            // Combinar las rutas existentes y las nuevas
            var todasLasRutasOriginales = rutasExistentes.Concat(rutasArchivosNuevos).ToList(); // Rutas completas
            var todasLasRutas = rutasExistentes
                .Select(ruta => Path.GetFileName(ruta)) // Obtener solo el nombre de los archivos existentes
                .Concat(rutasArchivosNuevos.Select(ruta => Path.GetFileName(ruta))) // Obtener solo el nombre de los nuevos archivos
                .ToList(); // Solo nombres de archivos

            // Mensaje de respuesta
            if (imagenesSubidas == 0 && archivosDuplicados.Count == archivos.Count)
            {
                return Ok(new
                {
                    codigointerno = codigoInterno,
                    Mensaje = "No se subieron imágenes. Todos los archivos eran duplicados.",
                    ImágenesExistentes = cantidadExistentes,
                    RutasExistentes = rutasExistentes,
                    RutasSubidas = rutasArchivosNuevos,
                    ArchivosDuplicados = archivosDuplicados
                });
            }
            else
            {
                return Ok(new
                {
                    Mensaje = "Imágenes subidas con éxito.",
                    TotalImágenes = cantidadExistentes + imagenesSubidas, // Total de imágenes subidas
                    RutasOriginal = todasLasRutasOriginales, // Rutas completas
                    Rutas = todasLasRutas, // Solo nombres de archivos
                    ArchivosDuplicados = archivosDuplicados // Archivos que eran duplicados
                });
            }
        }


        // Mensaje de respuesta
        /*
        if (imagenesSubidas < archivos.Count)
        {
            return Ok(new
            {
                codigointerno = codigoInterno,
                Mensaje = $"Imágenes interrumpidas. Solo se subieron {imagenesSubidas} de {archivos.Count}.",
                ImágenesExistentes = cantidadExistentes,
                RutasExistentes = rutasExistentes,
                RutasSubidas = rutasArchivosNuevos
            });
        }

        return Ok(new
        {
            Mensaje = "Imágenes subidas con éxito.",
            TotalImágenes = cantidadExistentes + imagenesSubidas, // Total de imágenes subidas
            RutasOriginal = todasLasRutasOriginales, // Rutas completas
            Rutas = todasLasRutas // Solo nombres de archivos
        });
        */
    }
}
