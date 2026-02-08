using AiTextToSql.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiTextToSql.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly SchemaService _schemaService;

        public SchemaController(SchemaService schemaService)
        {
            _schemaService = schemaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchema()
        {
            var schema = await _schemaService.GetDatabaseSchemaAsync();
            return Ok(schema);
        }
    }
}
