using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseEfficiencyProjekt
{
    class TestDatabaseEfficiency
    {
        private SqlConnection _connection;
        private DateTime _startTime;
        private DateTime _endTime;

        public static void Main()
        {
            var testdatabaseEfficiency = new TestDatabaseEfficiency();
            testdatabaseEfficiency.TestQuerryEfficiency();
        }

        private void Start() => _startTime = DateTime.UtcNow;

        private void End()
        {
            _endTime = DateTime.UtcNow;
            TimeSpan time = _endTime - _startTime;
            Console.WriteLine($"Time: {time}\n");
        }

        private void TestQuerryEfficiency()
        {
            var connetionString = @"Data Source=DESKTOP-8RCFKEE;Initial Catalog=DatabaseEfficiency;Trusted_Connection=True";
            _connection = new SqlConnection(connetionString);
            _connection.Open();

            Console.WriteLine("******************************************************\n");

            Console.WriteLine("Select * from [tblAuthors] and save id in list<int>");
            Start();
            SelectEverythingAndSaveIdToList();
            End();

            Console.WriteLine("Select * from [tblAuthors] and save id in int[]");
            Start();
            SelectEverythingAndSaveIdToArray();
            End();

            Console.WriteLine("Select id from [tblAuthors] and save id in list<int>");
            Start();
            SelectIdAndSaveIdToList();
            End();

            Console.WriteLine("Select id from [tblAuthors] and save id in int[]");
            Start();
            SelectIdAndSaveIdToArray();
            End();

            Console.WriteLine("******************************************************\n");

            // With the Entity Framework queries, only one can be executed at the a time otherwise the results will be corrupted.
            Console.WriteLine("Select id from [tblAuthors] and save id in list<int> with Entity Framework");
            Start();
            SelectIdToListWithEntityFramework();
            End();

            Console.WriteLine("Select id from [tblAuthors] and save id in int[] with Entity Framework");
            Start();
            // SelectIdToArrayWithEntityFramework();
            End();

            Console.WriteLine("******************************************************\n");

            Console.WriteLine("SELECT COUNT(*) FROM [tblAuthors] and save count as int");
            Start();
            SelectCountEverything();
            End();

            Console.WriteLine("SELECT COUNT(id) FROM [tblAuthors] and save count as int");
            Start();
            SelectCountId();
            End();

            Console.WriteLine("******************************************************\n");

            Console.WriteLine("SELECT id) FROM [tblAuthors] where Author_id == 1-6 with Join");
            Start();
            SelectIdFromBooksWithJoin();
            End();

            Console.WriteLine("SELECT id) FROM [tblAuthors] where Author_id == 1-6 with IN");
            Start();
            SelectIdFromBooksWithIn();
            End();

            Console.WriteLine("******************************************************\n");
            Console.ReadLine();
        }

        private void SelectEverythingAndSaveIdToList()
        {
            var command = new SqlCommand("Select * from [tblAuthors]", _connection);
            var authorsId = new List<int>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    authorsId.Add((int)reader["id"]);
                }
            }
        }

        private void SelectEverythingAndSaveIdToArray()
        {
            var command = new SqlCommand("Select * from [tblAuthors]", _connection);
            var authorsId = new int[10000000];
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    authorsId[count] = (int)reader["id"];
                    count++;
                }
            }
        }

        private void SelectIdAndSaveIdToList()
        {
            var command = new SqlCommand("Select id from [tblAuthors]", _connection);
            var authorsId = new List<int>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    authorsId.Add((int)reader["id"]);
                }
            }
        }

        private void SelectIdAndSaveIdToArray()
        {
            var command = new SqlCommand("Select id from [tblAuthors]", _connection);
            var authorsId = new int[10000000];
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    authorsId[count] = (int)reader["id"];
                    count++;
                }
            }
        }

        private void SelectIdToListWithEntityFramework()
        {
            var authorsId = new List<int>();
            using (var dbConntext = new DatabaseConntext())
            {
                var authors = dbConntext.tblAuthors.AsNoTracking();
                var authorsIdQerry = (from author in authors select author.Id);
                authorsId = authorsIdQerry.ToList();
            }
        }

        private void SelectIdToArrayWithEntityFramework()
        {
            var authorsId = new int[10000000];
            using (var dbConntext = new DatabaseConntext())
            {
                var authors = dbConntext.tblAuthors.AsNoTracking();
                var authorsIdQerry = (from author in authors select author.Id);
                authorsId = authorsIdQerry.ToArray();
            }
        }

        private void SelectCountEverything()
        {
            var command = new SqlCommand("SELECT COUNT(*) FROM [tblAuthors]", _connection);
            var count = 0;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    count = (int)reader[0];
                }
            }
        }

        private void SelectCountId()
        {
            var command = new SqlCommand("SELECT  COUNT(Id) FROM [tblAuthors]", _connection);
            var count = 0;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    count = (int)reader[0];
                }
            }
        }

        private void SelectIdFromBooksWithJoin()
        {
            var command = new SqlCommand("SELECT Id FROM [tblBooks] INNER JOIN(VALUES(1), (2), (3), (4), (5),(6)) AS Data(Item) ON [tblBooks].Auhthor_id = Data.Item", _connection);
            var authorsId = new int[10000000];
            var count = 0;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    authorsId[count] = (int)reader["id"];
                    count++;
                }
            }
        }

        private void SelectIdFromBooksWithIn()
        {
            var command = new SqlCommand("SELECT Id FROM [tblBooks] WHERE Auhthor_id IN(1, 2, 3, 4, 5, 6)", _connection);
            var authorsId = new int[10000000];
            var count = 0;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    authorsId[count] = (int)reader["id"];
                    count++;
                }
            }
        }
    }
}
