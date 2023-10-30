using EasyWater.Service.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EasyWater.Service.Functions
{
    public class FunctionAuth
    {
        readonly IAuthService _authService;
        public FunctionAuth(IAuthService authService)
        {
            _authService = authService;
        }

        [FunctionName("RunTokenGet")]        
        public async Task<IActionResult> RunGetToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "auth/get")] HttpRequest req,
            ILogger log)
        {
            int.TryParse(req.Query["codigo"], out int codigo);

            log.LogInformation("[RunGetToken] Beginning with code: {1}", codigo);

            var token = _authService.GetOrGenerateNewToken(codigo);

            log.LogInformation("[RunGetToken] Result: {0}", token.Chave);

            log.LogInformation("[RunGetToken] Finish.");

            return new OkObjectResult(token);
        }

        [FunctionName("RunTokenRenew")]
        public async Task<IActionResult> RunRenewToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "auth/renew")] HttpRequest req,
            ILogger log)
        {
            Guid.TryParse(req.Query["chave"], out Guid codigo);

            log.LogInformation("[RunGetToken] Beginning with code: {1}", codigo);

            var token = _authService.RenewToken(codigo);

            log.LogInformation("[RunGetToken] Result: {0}", token.Expiracao);

            log.LogInformation("[RunGetToken] Finish.");

            return new OkObjectResult(token);
        }

        [FunctionName("RunTokenCheckExpired")] // at 1:30 AM every day
        public void RunCheckExpiredToken([TimerTrigger("0 30 1 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("[RunCheckExpiredToken] Beginning...");

            _authService.CheckExpired();

            log.LogInformation("[RunCheckExpiredToken] Finish.");
        }
    }
}
