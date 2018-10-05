using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public class StartUp
{
    public static void Main()
    {
        var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
        connection.Open();

        using (connection)
        {
            var minionId = int.Parse(Console.ReadLine());

            var connString = string.Empty;
            var queryProc = @"SELECT * FROM SYSOBJECTS WHERE TYPE='P' AND name='usp_GetOlder'";
            var spExists = false;

            using (var commandProc = new SqlCommand(queryProc, connection))
            {
                using (var readerProc = commandProc.ExecuteReader())
                {
                    while (readerProc.Read())
                    {
                        spExists = true;
                        break;
                    }
                }
            }

            if (!spExists)
            {
                var procQuery = "CREATE PROC [dbo].[usp_GetOlder] (@minionId INT) " +
                                "AS " +
                                "BEGIN " +
                                    "BEGIN TRANSACTION " +
                                        "DECLARE @IsMinionExist INT = (SELECT COUNT(*) FROM Minions " +
                                        "WHERE Id = @minionId) " +
                                        "IF(@IsMinionExist = 0) " +
                                        "BEGIN " +
                                            "ROLLBACK " +
                                            "RETURN " +
                                        "END " +
                                        "UPDATE Minions " +
                                        "SET Age += 1 " +
                                        "WHERE Id = @minionId " +
                                    "COMMIT " +
                                "END";

                var selectMinionsCommand = new SqlCommand(procQuery, connection);
                selectMinionsCommand.ExecuteNonQuery();
            }

            var command =
                new SqlCommand("usp_GetOlder", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@minionId", minionId);

            var affectedRows = command.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                var query = @"SELECT Name, Age FROM Minions WHERE Id = @minionId";
                var getMinionsData = new SqlCommand(query, connection);
                getMinionsData.Parameters.AddWithValue("@minionId", minionId);

                var reader = getMinionsData.ExecuteReader();
                reader.Read();
                Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
            }
        }
    }
}