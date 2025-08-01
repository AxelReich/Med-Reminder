using System;
using System.Reflection.Metadata;
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
                var stage = new Stage
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    SymptomId = reader.GetInt32(reader.GetOrdinal("SymptomId")),
                    Medication = new List<Medication>()
                };

                stage.Medication = GetMedicationsByStageId(stage.Id);
                stages.Add(stage);
            }



            return stages;
        }

        public List<Medication> GetMedicationsByStageId(int stageId)
        {
            var medications = new List<Medication>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Medications WHERE StageId = @stageId";
            command.Parameters.AddWithValue("@stageId", stageId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var med = new Medication
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IntervalHours = reader.IsDBNull(reader.GetOrdinal("IntervalHours")) ? null : reader.GetInt32(reader.GetOrdinal("IntervalHours")),
                    TotalDays = reader.IsDBNull(reader.GetOrdinal("TotalDays")) ? null : reader.GetInt32(reader.GetOrdinal("TotalDays")),
                    QuantityMg = reader.IsDBNull(reader.GetOrdinal("QuantityMg")) ? null : reader.GetInt32(reader.GetOrdinal("QuantityMg")),
                    TreatmentId = reader.IsDBNull(reader.GetOrdinal("TreatmentId")) ? null : reader.GetInt32(reader.GetOrdinal("TreatmentId")),
                    StageId = reader.IsDBNull(reader.GetOrdinal("StageId")) ? null : reader.GetInt32(reader.GetOrdinal("StageId")),
                    // You can skip navigation properties here or handle if needed
                };

                medications.Add(med);
            }
            return medications;
        }
    }
}

    
