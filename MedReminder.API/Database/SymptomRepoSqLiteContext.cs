using System;
using MedReminder.Library.Models; 
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace MedReminder.API.Database
{
    public class SymptomRepoSqliteContext
    {
        private readonly string _connectionString;

        public SymptomRepoSqliteContext(string connectionString)
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
                CREATE TABLE IF NOT EXISTS Symptoms (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 0
                );
            ";
            command.ExecuteNonQuery();
        }
        public List<Symptom> GetAll()
        {
            var symptoms = new List<Symptom>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Symptoms"; // Ensure Symptoms table exists

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                symptoms.Add(new Symptom
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    // Add other fields as needed
                });
            }
            return symptoms;
        }

        public Symptom? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            Symptom? symptom = null;

            // Get symptom
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Name, IsActive FROM Symptoms WHERE Id = @id;";
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    symptom = new Symptom
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        IsActive = reader.GetBoolean(2),
                        Stages = new List<Stage>()
                    };
                }
            }

            // Get stages for this symptom
            if (symptom != null)
            {
                using var stageCommand = connection.CreateCommand();
                stageCommand.CommandText = "SELECT Id, Name, SymptomId FROM Stages WHERE SymptomId = @symptomId;";
                stageCommand.Parameters.AddWithValue("@symptomId", id);

                using var stageReader = stageCommand.ExecuteReader();
                while (stageReader.Read())
                {
                    symptom.Stages?.Add(new Stage
                    {
                        Id = stageReader.GetInt32(0),
                        Name = stageReader.GetString(1),
                        SymptomId = stageReader.GetInt32(2)
                    });
                }
            }

            return symptom;
        }

        

        public Symptom AddOrUpdate(Symptom symptom)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();

            if (symptom.Id <= 0)
            {
                // Insert symptom
                command.CommandText = @"
                    INSERT INTO Symptoms (Name, IsActive)
                    VALUES (@name, @isActive);";

                command.Parameters.AddWithValue("@name", symptom.Name ?? string.Empty);
                command.Parameters.AddWithValue("@isActive", symptom.IsActive);

                command.ExecuteNonQuery();

                // Get last inserted ID
                command.CommandText = "SELECT last_insert_rowid();";
                command.Parameters.Clear();

                var result = command.ExecuteScalar();
                symptom.Id = Convert.ToInt32(result);

                
                InsertDefaultStages((int)symptom.Id!);
            }
            else
            {
                // Update symptom
                command.CommandText = @"
                    UPDATE Symptoms
                    SET Name = @name,
                        IsActive = @isActive
                    WHERE Id = @id;";

                command.Parameters.AddWithValue("@name", symptom.Name ?? string.Empty);
                command.Parameters.AddWithValue("@isActive", symptom.IsActive);
                command.Parameters.AddWithValue("@id", symptom.Id);

                command.ExecuteNonQuery();
            }

            return symptom;
        }

        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Symptoms WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            var affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        private void InsertDefaultStages(int SymptomId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var stageNames = new[] { "Early", "Mid", "Severe" };

            foreach (var stageName in stageNames)
            {
                using var stageCommand = connection.CreateCommand();

                stageCommand.CommandText = @"
                    INSERT INTO Stages (Name, SymptomId)
                    VALUES (@name, @SymptomId)
                ";

                stageCommand.Parameters.AddWithValue("@name", stageName); // Fixed: use stageName instead of stageNames
                stageCommand.Parameters.AddWithValue("@SymptomId", SymptomId);

                stageCommand.ExecuteNonQuery();
            }

        }
    }
}
