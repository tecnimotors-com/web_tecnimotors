namespace ApiDockerTecnimotors.Repositories.MaestroClasificado.Model
{
    public class ModelLlanta
    {
    }
    public class TlMedida
    {
        public string? Medida { get; set; }
    }
    public class TlModelo
    {
        public string? Modelo { get; set; }
    }
    public class TlMarca
    {
        public string? Marca { get; set; }
    }
    public class TlCategoria
    {
        public string? Categoria { get; set; }
    }

    public class TlModelsFilter
    {
        public string? Medida { get; set; }
        public string? Modelo { get; set; }
        public string? Marca { get; set; }
        public string? Categoria { get; set; }
    }

    public class ListadoRequest
    {
        public string? Tipo { get; set; }
        public TlModelsFilter? Filtro { get; set; }
    }
}
