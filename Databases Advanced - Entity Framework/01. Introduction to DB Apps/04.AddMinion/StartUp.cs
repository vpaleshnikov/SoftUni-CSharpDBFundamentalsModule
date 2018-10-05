using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public class StartUp
{
    public static void Main()
    {
        var connection = new SqlConnection(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
        connection.Open();

        var minionInput = Console.ReadLine()
            .Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        var minionName = minionInput[1].Trim();
        var minionAge = int.Parse(minionInput[2].Trim());
        var minionTown = minionInput[3].Trim();

        var villainInput = Console.ReadLine()
            .Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        var villainName = villainInput[1].Trim();

        using (connection)
        {
            TownProcedure(connection, minionTown);

            VillainProcedure(connection, villainName);

            AddMinionToVallainProcedure(connection, minionName, minionAge, minionTown, villainName);
        }
    }

    private static void AddMinionToVallainProcedure(SqlConnection connection, string minionName, int minionAge, string minionTown, string villainName)
    {
        var connString = string.Empty;
        var query = @"SELECT * FROM SYSOBJECTS WHERE TYPE='P' AND name='p_AddMinionToVallainMapTable'";
        var spExists = false;

        using (var command = new SqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    spExists = true;
                    break;
                }
            }
        }

        if (!spExists)
        {
            var selectMinionsQuery = File.ReadAllText("Procedure_AddMinionToVallainMapTable.sql");
            var selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);
            selectMinionsCommand.ExecuteNonQuery();
        }

        var addMinionToVallainCommand =
            new SqlCommand("p_AddMinionToVallainMapTable", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

        addMinionToVallainCommand.Parameters.Add("@minionName", SqlDbType.VarChar).Value = minionName;
        addMinionToVallainCommand.Parameters.Add("@minionAge", SqlDbType.Int).Value = minionAge;
        addMinionToVallainCommand.Parameters.Add("@minionTown", SqlDbType.VarChar).Value = minionTown;
        addMinionToVallainCommand.Parameters.Add("@villainName", SqlDbType.VarChar).Value = villainName;

        addMinionToVallainCommand.ExecuteNonQuery();

        Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}");
    }

    private static void VillainProcedure(SqlConnection connection, string villainName)
    {
        var connString = string.Empty;
        var query = @"SELECT * FROM SYSOBJECTS WHERE TYPE='P' AND name='p_AddVillainToVillains'";
        var spExists = false;

        using (var command = new SqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    spExists = true;
                    break;
                }
            }
        }

        if (!spExists)
        {
            var selectMinionsQuery = File.ReadAllText("Procedure_AddVillainToVillains.sql");
            var selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);
            selectMinionsCommand.ExecuteNonQuery();
        }

        var villainCommand = new SqlCommand("p_AddVillainToVillains", connection) { CommandType = CommandType.StoredProcedure };
        villainCommand.Parameters.AddWithValue("@villainName", villainName);

        var affectedVillainRow = villainCommand.ExecuteNonQuery();

        if (affectedVillainRow == 1)
        {
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }
    }

    private static void TownProcedure(SqlConnection connection, string minionTown)
    {
        var connString = string.Empty;
        var query = @"SELECT * 
                               FROM SYSOBJECTS 
                              WHERE TYPE='P' 
                                AND name='p_AddTownToTowns'";
        var spExists = false;

        using (var command = new SqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    spExists = true;
                    break;
                }
            }
        }

        if (!spExists)
        {
            var selectMinionsQuery = File.ReadAllText("Procedure_AddTownToTowns.sql");
            var selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);
            selectMinionsCommand.ExecuteNonQuery();
        }

        var townCommand = new SqlCommand("p_AddTownToTowns", connection) { CommandType = CommandType.StoredProcedure };
        townCommand.Parameters.AddWithValue("@townName", minionTown);

        var affectedTownRow = townCommand.ExecuteNonQuery();

        if (affectedTownRow == 1)
        {
            Console.WriteLine($"Town {minionTown} was added to the database.");
        }
    }
}