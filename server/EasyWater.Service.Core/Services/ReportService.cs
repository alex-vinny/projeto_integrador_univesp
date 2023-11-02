using EasyWater.Domain;
using EasyWater.Domain.Models.Api;
using EasyWater.Service.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWater.Service.Core.Services
{
    public interface IReportService
    {
        Task<TemperatureReport> GenerateTemperaturaReport(ReportFilter filter, long floraId);
        Task<MoistureReport> GenerateMoistureReport(ReportFilter filter, long floraId);
        Task<WateringReport> GenerateWateringReport(ReportFilter filter, long floraId);
        Task GenerateTestingReport(ReportTesting filter, long floraId);
    }

    public class ReportService : IReportService
    {
        readonly IFreeSql _freeSql;

        public ReportService(IFreeSql freeSql) 
        {
            _freeSql = freeSql;
        }

        private async Task<List<TEntity>> GenerateReport<TEntity>(ReportFilter filter, long floraId)
            where TEntity: OwnerEntity
        {
            Validate(filter);
            ValidateFlora(floraId);

            var select = _freeSql.Select<TEntity>()
                .Where(a => a.DonoId == floraId && a.Deletado == false)
                .Where(c => c.CriadoEm >= filter.dataIni && c.CriadoEm <= filter.dataFin);

            var total = await select.CountAsync();
            return await select.Page(filter.page, filter.pageSize).ToListAsync();
        }

        public async Task<MoistureReport> GenerateMoistureReport(ReportFilter filter, long floraId)
        {
            var list = await GenerateReport<Humidade>(filter, floraId);
            var temps = new List<Moisture>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    temps.Add(new Moisture
                    {
                        date = item.CriadoEm.Value,
                        id = item.Id,
                        value = item.Valor
                    });
                }
            }

            return new MoistureReport(temps);
        }

        public async Task<TemperatureReport> GenerateTemperaturaReport(ReportFilter filter, long floraId)
        {
            var list = await GenerateReport<Temperatura>(filter, floraId);
            var temps = new List<Temperature>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    temps.Add(new Temperature 
                    { 
                        date = item.CriadoEm.Value,
                        id = item.Id,
                        value = item.Valor
                    });
                }
            }

            return new TemperatureReport(temps);
        }

        public async Task<WateringReport> GenerateWateringReport(ReportFilter filter, long floraId)
        {
            var list = await GenerateReport<HumidadeSolo>(filter, floraId);
            var temps = new List<Watering>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    temps.Add(new Watering
                    {
                        date = item.CriadoEm.Value,
                        id = item.Id,
                        value = item.LigouIrrigacao
                    });
                }
            }

            return new WateringReport(temps);
        }

        public async Task GenerateTestingReport(ReportTesting filter, long floraId)
        {
            ValidateFlora(floraId);

            var temperaturas = new List<Temperatura>();
            var humidades = new List<Humidade>();
            var humidadesSolo = new List<HumidadeSolo>();

            var qtd = filter.total % (filter.dataFin - filter.dataIni).TotalDays;

            while (filter.dataIni.Date < filter.dataFin.Date)
            {
                temperaturas.Add(new Temperatura { CriadoEm = filter.dataIni,  DonoId = floraId, Valor = GenerateRandom });
                humidades.Add(new Humidade { CriadoEm = filter.dataIni, DonoId = floraId, Valor = GenerateRandom });
                humidadesSolo.Add(new HumidadeSolo { CriadoEm = filter.dataIni, DonoId = floraId, Valor = GenerateRandom, LigouIrrigacao = filter.dataIni.Day % 5 == 0 });

                filter.dataIni = filter.dataIni.AddDays(1);
            }

            var task1 = Task.Run(() => _freeSql.Insert(temperaturas).ExecuteAffrows());
            var task2 = Task.Run(() => _freeSql.Insert(humidades).ExecuteAffrows());
            var task3 = Task.Run(() => _freeSql.Insert(humidadesSolo).ExecuteAffrows());

            var taskList = new List<Task>
            {
                task1,
                task2,
                task3
            };

            await Task.WhenAll(taskList.ToArray());
        }

        private double GenerateRandom
        {
            get
            {
                return new Random().NextDouble() * new Random().Next(1, 100);
            }
        }

        private void Validate(ReportFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("Obrigatório informar um filtro para geração do relatório");

            if (filter.dataIni > filter.dataFin) throw new ArgumentException("A data inicial não pode ser maior que a data final.");

            if (filter.dataFin.Year - filter.dataFin.Year > 1) throw new ArgumentException("O período de geração do relatório não pode ser maior que 1 ano.");
        }

        private void ValidateFlora(long floraId)
        {
            var floraQuery = _freeSql.GetRepository<Flora>()
                .Where(c => c.Id == floraId);

            if (!floraQuery.Any())
            {
                throw new Exception("Planta não cadastrada para gerar informações do relatório.");
            }
        }
    }
}
