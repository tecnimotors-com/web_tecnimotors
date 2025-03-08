namespace ApiDockerTecnimotors.Context
{
    public class PostgreSQLConfiguration(string connectionString)
    {
        public string ConnectionString { get; set; } = connectionString;
    }
}