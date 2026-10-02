using Microsoft.Data.SqlClient;
using ProjKlitov.Models;

namespace ProjKlitov.Controllers
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(
                    "SELECT Id, Name, Price, Category FROM Products", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        decimal price = reader.GetDecimal(2);
                        string? category = reader.IsDBNull(3) ? null : reader.GetString(3);

                        products.Add(new Product(id, name, price, category));
                    }
                }
            }

            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(
                    "SELECT Id, Name, Price, Category FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int productId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        decimal price = reader.GetDecimal(2);
                        string? category = reader.IsDBNull(3) ? null : reader.GetString(3);

                        return new Product(productId, name, price, category);
                    }
                }
            }

            return null;
        }

    }

}
