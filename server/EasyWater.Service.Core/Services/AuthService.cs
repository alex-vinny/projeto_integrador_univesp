using EasyWater.Domain;
using System;
using System.Linq.Expressions;

namespace EasyWater.Service.Core.Services
{
    public interface IAuthService
    {
        Token GetOrGenerateNewToken(int code);
        Token RenewToken(Guid chave);
        public void CheckExpired();
        Token ValidateToken(String token);
    }

    public class AuthService : BaseService<Token>, IAuthService
    {
        public AuthService(IFreeSql freeSql)
            : base(freeSql)
        {
        }

        private void ValidateFlora()
        {
            var floraRepository = _freeSql.GetRepository<Flora>();

            if (!floraRepository.Select.Any())
            {
                throw new Exception("Cadastrar uma planta para gerenciamento");
            }
        }

        private void ValidateFloraById(int code)
        {
            var floraRepository = _freeSql.GetRepository<Flora>();

            if (!floraRepository.Where(c => c.Id == code).Any())
            {
                throw new Exception("Cadastrar uma planta para gerenciamento");
            }
        }

        private DateTime CalcularExpiracao(DateTime dataAtual)
        {
            return dataAtual.Date.AddDays(30);
        }

        private Token GetByCodigo(int code)
        {
            return Get(c => c.Dono.Codigo == code);
        }

        private Token GetByChave(Guid chave)
        {
            return Get(c => c.Chave == chave);
        }

        private Flora GetFlora()
        {
            var floraRepository = _freeSql.GetRepository<Flora>();

            return floraRepository.Select.First();
        }

        private Token Get(Expression<Func<Token, bool>> condition)
        {
            var query = _repository
                .Where(c => c.Expiracao > DateTime.Now.Date && c.Deletado == false);
            query = query.InnerJoin(a => a.Dono.Id == a.DonoId)
                .Where(condition);

            if (query.Any())
            {
                return query.First();
            }

            return null;
        }

        private Token GenerateToken()
        {
            var flora = GetFlora();

            var token = new Token
            {
                DonoId = flora.Id,
                Expiracao = CalcularExpiracao(DateTime.Now)
            };

            return _repository.Insert(token);
        }

        public void CheckExpired()
        {
            var list = _freeSql.Select<Token>().Where(c => c.Expiracao < DateTime.Now.Date).ToList();

            if (list != null && list.Count > 0)
            {
                var repo = _freeSql.GetRepository<Token>();
                repo.BeginEdit(list);

                foreach (var item in list) 
                {
                    item.Expirado = true;
                    item.AtualizadoEm = DateTime.Now;
                }

                repo.EndEdit();
            }
        }

        public Token GetOrGenerateNewToken(int code)
        {
            ValidateFlora();
            ValidateFloraById(code);

            var exists = GetByCodigo(code);
            if (exists != null) 
            {
                return exists;
            }

            return GenerateToken();
        }

        public Token RenewToken(Guid chave)
        {
            var exists = GetByChave(chave);
            if (exists != null)
            {
                exists.AtualizadoEm = DateTime.Now;
                exists.Expiracao = CalcularExpiracao(exists.AtualizadoEm.Value);

                return exists;
            }

            throw new Exception($"Token '{chave}' não localizada para realizar a renovação");
        }

        public Token ValidateToken(String stringKey)
        {
            if (Guid.TryParse(stringKey, out var chave))
            {
                var token = _repository.Where(c => c.Chave == chave).First();

               if (token != null)
               {
                    if (token.Expiracao < DateTime.Now.Date || token.Expirado || token.Deletado)
                    {
                        throw new Exception($"Chave '{stringKey}' expirado!");
                    }

                    return token;
               }

                throw new Exception($"Chave '{stringKey}' não localizada!");
            }
            else
            {
                throw new Exception($"Chave '{stringKey}' não é válida!");
            }
        }
    }
}
