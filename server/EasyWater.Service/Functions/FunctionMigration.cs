using EasyWater.Service.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EasyWater.Service.Functions
{
    public class FunctionMigration
    {
        readonly IFreeSql _freeSql;
        readonly IMigrationService _migrationService;
        public FunctionMigration(IFreeSql freeSql, IMigrationService migrationService)
        {
            _migrationService = migrationService;
            _freeSql = freeSql;
        }

        [FunctionName("RunDatabaseMigration")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[RunMigration] Beginning:");

            var result = _migrationService.Run();

            log.LogInformation("[RunMigration] Result: {0}", result);

            log.LogInformation("[RunMigration] Finish.");

            return new OkObjectResult("");
        }
    }
}
