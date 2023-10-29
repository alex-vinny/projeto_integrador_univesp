using EasyWater.Service.Core.Entities;
using FreeSql;

namespace EasyWater.Service.Core.Services
{
    public abstract class BaseService<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly IFreeSql _freeSql;
        protected readonly IBaseRepository<TEntity> _repository;
        public BaseService(IFreeSql freeSql)
        {
            _freeSql = freeSql;
            _repository = freeSql.GetRepository<TEntity>();
        }
    }
}
