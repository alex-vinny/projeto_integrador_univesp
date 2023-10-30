using EasyWater.Domain;
using EasyWater.Domain.Models.Api;
using EasyWater.Service.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EasyWater.Service.Functions
{
    public class FunctionReport
    {
        readonly IReportService _reportService;
        readonly IAuthService _authService;
        public FunctionReport(IReportService reportService, IAuthService authService)
        {
            _authService = authService;
            _reportService = reportService;
        }

        [FunctionName("ReportTemperature")]
        public async Task<IActionResult> RunReportTemperature(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "report/temperature")] [FromQuery] ReportFilter filter,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[ReportTemperature] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var reportList = await _reportService.GenerateTemperaturaReport(filter, token.DonoId);

            log.LogInformation("[ReportTemperature] Result: {0}", reportList.PageSize);

            log.LogInformation("[ReportTemperature] Finish.");

            return new OkObjectResult(reportList);
        }

        [FunctionName("ReportMoisture")]
        public async Task<IActionResult> RunReportMoisture(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "report/moisture")] [FromQuery] ReportFilter filter,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[ReportMoisture] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var reportList = await _reportService.GenerateMoistureReport(filter, token.DonoId);

            log.LogInformation("[ReportMoisture] Result: {0}", reportList.PageSize);

            log.LogInformation("[ReportMoisture] Finish.");

            return new OkObjectResult(reportList);
        }

        [FunctionName("ReportWatering")]
        public async Task<IActionResult> RunReportWatering(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "report/watering")] [FromQuery] ReportFilter filter,
            HttpRequest req,           
            ILogger log)
        {
            log.LogInformation("[ReportWatering] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            var reportList = await _reportService.GenerateWateringReport(filter, token.DonoId);

            log.LogInformation("[ReportWatering] Result: {0}", reportList.PageSize);

            log.LogInformation("[ReportWatering] Finish.");

            return new OkObjectResult(reportList);
        }

        [FunctionName("ReportTesting")]
        public async Task<IActionResult> RunReportTesting(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "report/testing")][FromBody] ReportTesting filter,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[ReportTemperature] Beginning:");

            Token token = _authService.ValidateToken(req.Headers["x-token"].ToString());

            await _reportService.GenerateTestingReport(filter, token.DonoId);

            log.LogInformation("[ReportTemperature] Finish.");

            return new OkObjectResult("");
        }
    }
}
