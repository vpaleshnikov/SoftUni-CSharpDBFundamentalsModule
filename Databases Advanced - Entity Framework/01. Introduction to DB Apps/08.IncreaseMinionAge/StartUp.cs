using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class Startup
{
    public static void Main()
    {
        var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
        connection.Open();

        using (connection)
        {
            var minionsIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            foreach (var currentId in minionsIds)
            {
                var connString = string.Empty;
                var query = @"SELECT * FROM SYSOBJECTS WHERE TYPE='P' AND name='usp_GetOlder'";
                var spExists = false;

                using (var commandProc = new SqlCommand(query, connection))
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

                var command = new SqlCommand("usp_GetOlder", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@minionId", currentId);

                var affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    var queryGetName = @"SELECT Name FROM Minions WHERE Id = @minionId";
                    var getMinionName = new SqlCommand(queryGetName, connection);
                    getMinionName.Parameters.AddWithValue("@minionId", currentId);

                    var minionName = (string)getMinionName.ExecuteScalar();
                    var partsOfName = minionName.Split().ToArray();
                    var newPart = string.Empty;
                    var partsOfNewName = new List<string>();

                    foreach (var part in partsOfName)
                    {
                        if (char.IsUpper(part.First()))
                        {
                            newPart = char.ToLower(part.First()) + part.Substring(1);
                        }
                        else
                        {
                            newPart = char.ToUpper(part.First()) + part.Substring(1);
                        }

                        partsOfNewName.Add(newPart);
                    }
                    var newName = string.Join(" ", partsOfNewName);

                    var queryUpdateName = @"UPDATE Minions SET Name = @newName WHERE Id = @minionId";
                    var updateMinionName = new SqlCommand(queryUpdateName, connection);
                    updateMinionName.Parameters.AddWithValue("@minionId", currentId);
                    updateMinionName.Parameters.AddWithValue("@newName", newName);
                    updateMinionName.ExecuteNonQuery();
                }
            }

            var getMinionNameAndAgeQuery = @"SELECT Name, Age FROM Minions";
            var getMinionNameAndAgeCommand = new SqlCommand(getMinionNameAndAgeQuery, connection);
            var reader = getMinionNameAndAgeCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
            }
        }
    }
}