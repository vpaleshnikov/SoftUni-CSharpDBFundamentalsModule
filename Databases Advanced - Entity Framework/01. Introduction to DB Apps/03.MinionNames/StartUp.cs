using System;
using System.Data.SqlClient;

public class StartUp
{
    public static void Main()
    {
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MinionsDB;Integrated Security=True";
        var connection = new SqlConnection(connectionString);

        var villainId = int.Parse(Console.ReadLine());

        connection.Open();
        using (connection)
        {
            var villainQuery = "SELECT Name FROM Villains WHERE Id = @villainId";
            var villainCommand = new SqlCommand(villainQuery, connection);
            villainCommand.Parameters.AddWithValue("@villainId", villainId);
            var villainName = (string)villainCommand.ExecuteScalar();
            
            if (villainName == null)
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                return;
            }

            Console.WriteLine($"Villain: {villainName}");
            
            var minionQuery = "SELECT m.Name, m.Age FROM Minions AS m " +
                              "INNER JOIN MinionsVillains AS mv " +
                              "ON mv.MinionId = m.Id " +
                              "WHERE mv.VillainId = @villainId " +
                              "ORDER BY m.Name";
            var minionCommand = new SqlCommand(minionQuery, connection);
            minionCommand.Parameters.AddWithValue("@villainId", villainId);
            var reader = minionCommand.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("(no minions)");
                return;
            }

            var counter = 1;
            while (reader.Read())
            {
                Console.WriteLine($"{counter}. {reader[0]} {reader[1]}");
                counter++;
            }
        }
    }
}