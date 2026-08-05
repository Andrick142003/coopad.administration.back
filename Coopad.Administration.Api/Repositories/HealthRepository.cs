using Coopad.Administration.Api.Infrastructure.Database;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;

namespace Coopad.Administration.Api.Repositories
{
    public class HealthRepository : IHealthRepository
    {

        private readonly IAseConnectionFactory _connectionFactory;

        public HealthRepository(IAseConnectionFactory connectionFactory)
        {

            _connectionFactory = connectionFactory;
        }


        public Health? GetHealth()
        {

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "SELECT @@version AS Version";


            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                int versionIndex = reader.GetOrdinal("Version");

                return new Health
                {
                    Version = reader.GetString(versionIndex)
                };


            }

            return null;



        }

        public bool IsDatabaseAvailable()
        {
            return true;
        }
    }
}
