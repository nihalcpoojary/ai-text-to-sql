using Dapper;
using Microsoft.Data.SqlClient;

namespace AiTextToSql.Api.Service
{
    public class SchemaService
    {
        private readonly IConfiguration _configuration;

        public SchemaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetDatabaseSchemaAsync()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("AiConnection"));

            var tables = await connection.QueryAsync<string>(
                "SELECT TABLE_NAME FROM " +
                "INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_TYPE = 'BASE TABLE'");

            var schemaText = "";

            foreach (var table in tables)
            {
                schemaText += $"Table: {table}\n";

                var columns = await connection.QueryAsync(@"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName
            ", new { TableName = table });

                foreach (var column in columns)
                {
                    schemaText += $" - {column.COLUMN_NAME} ({column.DATA_TYPE})\n";
                }

                schemaText += "\n";
            }

            return schemaText;
        }
    }
}
