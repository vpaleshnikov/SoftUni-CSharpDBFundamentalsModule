using System;
using System.Data.SqlClient;

public class StartUp
{
    public static void Main()
    {
        var builder = new SqlConnectionStringBuilder
        {
            ["Data Source"] = "(localdb)\\MSSQLLocalDB",
            ["Initial Catalog"] = "MinionsDB",
            ["Integrated Security"] = true
        };

        var connection = new SqlConnection(builder.ToString());
        connection.Open();

        using (connection)
        {
            var query = "SELECT v.[Name], COUNT(mv.MinionId) AS MinionsCount " +
                        "FROM Villains AS v " +
                        "INNER JOIN MinionsVillains AS mv ON mv.VillainId = v.Id " +
                        "GROUP BY v.[Name] " +
                        "HAVING COUNT(mv.MinionId) > 3 " +
                        "ORDER BY MinionsCount DESC";

            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var villainName = (string)reader["Name"];
                var minionsCount = (int)reader["MinionsCount"];

                Console.WriteLine($"{villainName} - {minionsCount}");
            }
        }
    }
}