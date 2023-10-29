using EasyWater.Domain;
using EasyWater.Service.Core.Models;
using System;
using System.Threading.Tasks;

namespace EasyWater.Service.Core.Services
{
    public interface ISensoresService
    {
        Task<long> SaveHumidade(HumidadeModel model, long floraId);
        Task<long> SaveHumidadeSolo(HumidadeSoloModel model, long floraId);
        Task<long> SaveTemperatura(TemperaturaModel model, long floraId);
    }

    public class SensoresService : ISensoresService
    {
        readonly IFreeSql _freeSql;

        public SensoresService(IFreeSql freeSql) 
        {
            _freeSql = freeSql;
        }

        public async Task<long> SaveHumidade(HumidadeModel model, long floraId)
        {
            Humidade data = ValidateHumidade(model, floraId);

            return await _freeSql.Insert(data).ExecuteIdentityAsync();
        }

        private Humidade ValidateHumidade(HumidadeModel model, long floraId)
        {
            ValidateFlora(floraId);

            return new Humidade
            {
                DonoId = floraId,
                Valor = model.Humidade
            };
        }

        public async Task<long> SaveHumidadeSolo(HumidadeSoloModel model, long floraId)
        {
            HumidadeSolo data = ValidateHumidadeSolo(model, floraId);

            return await _freeSql.Insert(data).ExecuteIdentityAsync();
        }

        private HumidadeSolo ValidateHumidadeSolo(HumidadeSoloModel model, long floraId)
        {
            ValidateFlora(floraId);

            return new HumidadeSolo
            {
                DonoId = floraId,
                Valor = model.Humidade,
                LigouIrrigacao = model.AbriuBomba,
            };
        }

        public async Task<long> SaveTemperatura(TemperaturaModel model, long floraId)
        {
            Temperatura data = ValidateTemperature(model, floraId);

            return await _freeSql.Insert(data).ExecuteIdentityAsync();
        }

        private Temperatura ValidateTemperature(TemperaturaModel model, long floraId)
        {
            ValidateFlora(floraId);

            return new Temperatura
            {
                DonoId = floraId,
                Valor = model.Temperatura
            };
        }

        private void ValidateFlora(long floraId)
        {
            var floraQuery = _freeSql.GetRepository<Flora>()
                .Where(c => c.Id == floraId);
            
            if (!floraQuery.Any())
            {
                throw new Exception("Planta não cadastrada para receber informações dos sensores.");
            }
        }
    }
}
