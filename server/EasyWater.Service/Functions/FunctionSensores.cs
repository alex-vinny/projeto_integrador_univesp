using EasyWater.Domain;
using EasyWater.Service.Core.Models;
using EasyWater.Service.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EasyWater.Service.Functions
{
    public class FunctionSensores
    {
        readonly ISensoresService _sensoresService;
        readonly IAuthService _authService;
        public FunctionSensores(ISensoresService sensoresService, IAuthService authService)
        {
            _authService = authService;
            _sensoresService = sensoresService;
        }

        [FunctionName("RunSensoresTemperatura")]
        public async Task<IActionResult> RunTemperatura(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sensor/temperatura")] [FromBody] TemperaturaModel model,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[RunTemperatura] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var id = await _sensoresService.SaveTemperatura(model, token.DonoId);

            log.LogInformation("[RunTemperatura] Result: {0}", id);

            log.LogInformation("[RunTemperatura] Finish.");

            return new OkObjectResult(new { erro = false, id });
        }

        [FunctionName("RunSensoresHumidade")]
        public async Task<IActionResult> RunHumidade(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sensor/humidade")] [FromBody] HumidadeModel model,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[RunHumidade] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var id = await _sensoresService.SaveHumidade(model, token.DonoId);

            log.LogInformation("[RunHumidade] Result: {0}", id);

            log.LogInformation("[RunHumidade] Finish.");

            return new OkObjectResult(new { erro = false, id });
        }

        [FunctionName("RunSensoresHumidadeSolo")]
        public async Task<IActionResult> RunHumidadeSolo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sensor/irrigacao")] [FromBody] HumidadeSoloModel model,
            HttpRequest req,           
            ILogger log)
        {
            log.LogInformation("[RunHumidadeSolo] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var id = await _sensoresService.SaveHumidadeSolo(model, token.DonoId);

            log.LogInformation("[RunHumidade] Result: {0}", id);

            log.LogInformation("[RunHumidadeSolo] Finish.");

            return new OkObjectResult(new { erro = false, id });
        }
    }
}
