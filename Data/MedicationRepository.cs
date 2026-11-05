using Pharmacy_Inventory_System.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Pharmacy_Inventory_System.Data
{
    public class MedicationRepository
    {
        private readonly string _connectionString;

        public MedicationRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is missing.");
        }

        public async Task<List<Medication>> GetAllAsync()
        {
            var medications = new List<Medication>();

            const string sql = """
                SELECT Id, Name, [Description], [image],
                       Price, StockQuantity, [status]
                FROM dbo.medications
                ORDER BY Id DESC;
                """;

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medications.Add(MapMedication(reader));
            }

            return medications;
        }

        public async Task<Medication?> GetByIdAsync(int id)
        {
            const string sql = """
                SELECT Id, Name, [Description], [image],
                       Price, StockQuantity, [status]
                FROM dbo.medications
                WHERE Id = @Id;
                """;

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapMedication(reader);
            }

            return null;
        }

        public async Task<int> CreateAsync(Medication medication)
        {
            const string sql = """
                INSERT INTO dbo.medications
                    (Name, [Description], [image],
                     Price, StockQuantity, [status])
                OUTPUT INSERTED.Id
                VALUES
                    (@Name, @Description, @Image,
                     @Price, @StockQuantity, @Status);
                """;

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command = new SqlCommand(sql, connection);

            AddMedicationParameters(command, medication);

            await connection.OpenAsync();

            object? result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Medication medication)
        {
            const string sql = """
                UPDATE dbo.medications
                SET Name = @Name,
                    [Description] = @Description,
                    [image] = @Image,
                    Price = @Price,
                    StockQuantity = @StockQuantity,
                    [status] = @Status
                WHERE Id = @Id;
                """;

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command = new SqlCommand(sql, connection);

            AddMedicationParameters(command, medication);

            command.Parameters.Add("@Id", SqlDbType.Int).Value =
                medication.Id;

            await connection.OpenAsync();

            int rows = await command.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = """
                DELETE FROM dbo.medications
                WHERE Id = @Id;
                """;

            await using var connection =
                new SqlConnection(_connectionString);

            await using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            int rows = await command.ExecuteNonQueryAsync();

            return rows > 0;
        }

        private static Medication MapMedication(SqlDataReader reader)
        {
            return new Medication
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(
                    reader.GetOrdinal("Description")),
                Image = reader.GetString(reader.GetOrdinal("image")),
                Price = reader.GetInt32(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                Status = reader.GetString(reader.GetOrdinal("status"))
            };
        }

        private static void AddMedicationParameters(
            SqlCommand command, Medication medication)
        {
            command.Parameters.Add("@Name", SqlDbType.VarChar, 50)
                .Value = medication.Name;

            command.Parameters.Add("@Description", SqlDbType.VarChar, 50)
                .Value = medication.Description;

            command.Parameters.Add("@Image", SqlDbType.VarChar, 255)
                .Value = medication.Image;

            command.Parameters.Add("@Price", SqlDbType.Int)
                .Value = medication.Price;

            command.Parameters.Add("@StockQuantity", SqlDbType.Int)
                .Value = medication.StockQuantity;

            command.Parameters.Add("@Status", SqlDbType.VarChar, 100)
                .Value = medication.Status;
        }
    }
}
