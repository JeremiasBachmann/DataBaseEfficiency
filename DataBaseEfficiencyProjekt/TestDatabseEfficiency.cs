using DataBaseEfficiencyProjekt;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseEfficiencyProjekt
{
    class TestDatabaseEfficiency
    {
        private SqlConnection conn;
        private DateTime startTime;
        private DateTime endTime;

        public static void Main()
        {
            TestDatabaseEfficiency databaseEfficiency = new TestDatabaseEfficiency();
            databaseEfficiency.TestQuerryEfficiency();
        }

        private void Start()
        {
            startTime = DateTime.UtcNow;
        }

        private void End()
        {
            endTime = DateTime.UtcNow;
            var time = endTime - startTime;
            Console.WriteLine($"Time: {time}");
        }

        private void TestQuerryEfficiency()
        {
            string connetionString;
            connetionString = @"Data Source=DESKTOP-8RCFKEE;Initial Catalog=DatabaseEfficiency;Trusted_Connection=True";
            conn = new SqlConnection(connetionString);
            conn.Open();

            //******************************************************

            Console.WriteLine("Select * from [tblAuthors] and save id in list<int>");
            Start();
            SelectEverythingAndSaveIdToList();
            End();
            Console.WriteLine("");

            Console.WriteLine("Select * from [tblAuthors] and save id in int[]");
            Start();
            SelectEverythingAndSaveIdToArray();
            End();
            Console.WriteLine("");

            Console.WriteLine("Select id from [tblAuthors] and save id in list<int>");
            Start();
            SelectIdAndSaveIdToList();
            End();
            Console.WriteLine("");

            Console.WriteLine("Select id from [tblAuthors] and save id in int[]");
            Start();
            SelectIdAndSaveIdToArray();
            End();
            Console.WriteLine("");

            //******************************************************

            // With the Entity Framework queries, only one can be executed at the a time otherwise the results will be corrupted
            Console.WriteLine("Select id from [tblAuthors] and save id in list<int> with Entity Framework");
            Start();
            SelectIdToListWithEntityFramework();
            End();
            Console.WriteLine("");

            Console.WriteLine("Select id from [tblAuthors] and save id in int[] with Entity Framework");
            Start();
            //SelectIdToArrayWithEntityFramework();
            End();
            Console.WriteLine("");

            //******************************************************

            Console.WriteLine("SELECT COUNT(*) FROM [tblAuthors] and save count as int");
            Start();
            SelectCountEverything();
            End();
            Console.WriteLine("");

            Console.WriteLine("SELECT COUNT(id) FROM [tblAuthors] and save count as int");
            Start();
            SelectCountId();
            End();
            Console.WriteLine("");

            //******************************************************

            Console.WriteLine("SELECT id) FROM [tblAuthors] where Author_id == 1-6 with Join");
            Start();
            SelectIdFromBooksWithJoin();
            End();
            Console.WriteLine("");

            Console.WriteLine("SELECT id) FROM [tblAuthors] where Author_id == 1-6 with IN");
            Start();
            SelectIdFromBooksWithIn();
            End();
            Console.WriteLine("");

            //******************************************************
            Console.ReadLine();
        }

        private void SelectEverythingAndSaveIdToList()
        {
            SqlCommand command = new SqlCommand("Select * from [tblAuthors]", conn);
            List<int> authorsId = new List<int>();
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
            SqlCommand command = new SqlCommand("Select * from [tblAuthors]", conn);
            int[] authorsId = new int[10000000];
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
            SqlCommand command = new SqlCommand("Select id from [tblAuthors]", conn);
            List<int> authorsId = new List<int>();
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
            SqlCommand command = new SqlCommand("Select id from [tblAuthors]", conn);
            int[] authorsId = new int[10000000];
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
            List<int> authorsId = new List<int>();
            using (var _DBConntext = new DatabaseConntext())
            {
                var authors = _DBConntext.tblAuthors.AsNoTracking();
                var authorsIdQerry = (from author in authors select author.Id);
                authorsId = authorsIdQerry.ToList();
            }
        }

        private void SelectIdToArrayWithEntityFramework()
        {
            int[] authorsId = new int[10000000];
            using (var _DBConntext = new DatabaseConntext())
            {
                var authors = _DBConntext.tblAuthors.AsNoTracking();
                var authorsIdQerry = (from author in authors select author.Id);
                authorsId = authorsIdQerry.ToArray();
            }
        }

        private void SelectCountEverything()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [tblAuthors]", conn);
            int count = 0;
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
            SqlCommand command = new SqlCommand("SELECT  COUNT(Id) FROM [tblAuthors]", conn);
            int count = 0;
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
            SqlCommand command = new SqlCommand("SELECT Id FROM [tblBooks] INNER JOIN(VALUES(1), (2), (3), (4), (5),(6)) AS Data(Item) ON [tblBooks].Auhthor_id = Data.Item", conn);
            int[] authorsId = new int[10000000];
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

        private void SelectIdFromBooksWithIn()
        {
            SqlCommand command = new SqlCommand("SELECT Id FROM [tblBooks] WHERE Auhthor_id IN(1, 2, 3, 4, 5, 6)", conn);
            int[] authorsId = new int[10000000];
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
    }
}
