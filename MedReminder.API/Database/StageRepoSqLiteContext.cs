using System;
using MedReminder.Library.Models; 
using Microsoft.Data.Sqlite;


namespace MedReminder.API.Database
{
    class StageRepoSqliteContext
    {
        private readonly string _connectionString;

        public StageRepoSqliteContext(string connectionString)
        {
            _connectionString = connectionString; ;
            
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Stages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    SymptomId INTEGER NOT NULL,
                    FOREIGN KEY(SymptomId) REFERENCES Symptoms(Id)
                );
            ";
            command.ExecuteNonQuery();
        }
        
        public List<Stage> GetBySymptomId(int symptomId)
        {
            var stages = new List<Stage>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Stages WHERE SymptomId = @symptomId";
            command.Parameters.AddWithValue("@symptomId", symptomId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                stages.Add(new Stage
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    SymptomId = reader.GetInt32(reader.GetOrdinal("SymptomId"))
                });
            }

            return stages;
        }
    }
}

    
