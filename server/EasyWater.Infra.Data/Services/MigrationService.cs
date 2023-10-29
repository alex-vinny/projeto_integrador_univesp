using FluentMigrator.Runner;

namespace EasyWater.Service.Core.Services
{
    public interface IMigrationService
    {
        MigrationExecutionResults Run();
    }
    public enum MigrationExecutionResults
    {
        NotFound,
        Executed
    }


    public class MigrationService : IMigrationService
    {        
        readonly IMigrationRunner _runner;

        public MigrationService(IMigrationRunner runner)
        {
            _runner = runner;            
        }

        public MigrationExecutionResults Run()
        {
            if (_runner.HasMigrationsToApplyUp())
            {
                _runner.MigrateUp();
                return MigrationExecutionResults.Executed;
            }

            return MigrationExecutionResults.NotFound;
        }
    }
}
