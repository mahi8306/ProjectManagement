using MySqlConnector;
using ProductManagement.Models;

public class AdoProductRepository
{
    private readonly string _connStr;
    public AdoProductRepository(IConfiguration config)
        => _connStr = config.GetConnectionString("DefaultConnection")!;

    public List<Product> GetLowStock(int threshold)
    {
        var list = new List<Product>();
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand(
            "SELECT Id, Name, Price, Stock FROM Products WHERE Stock < @t", conn);
        cmd.Parameters.AddWithValue("@t", threshold);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Stock = reader.GetInt32(3)
            });
        }
        return list;
    }

    public decimal GetTotalInventoryValue()
    {
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand(
            "SELECT IFNULL(SUM(Price * Stock), 0) FROM Products", conn);
        conn.Open();
        return Convert.ToDecimal(cmd.ExecuteScalar());
    }
}