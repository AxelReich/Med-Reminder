using System;
using System.Collections.Generic;
using MedReminder.Library.Models;
using Microsoft.Data.Sqlite;

namespace MedReminder.API.Database
{
    public class MedicationRepoSqliteContext
    {
        private readonly string _connectionString;

        public MedicationRepoSqliteContext(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Medications (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    IntervalHours INTEGER,
                    TotalDays INTEGER,
                    QuantityMg INTEGER,
                    TreatmentId INTEGER,
                    StageId INTEGER NOT NULL,
                    FOREIGN KEY(StageId) REFERENCES Stages(Id)
                );
            ";
            command.ExecuteNonQuery();
        }

        public List<Medication> GetByStageId(int stageId)
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
                medications.Add(new Medication
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IntervalHours = reader.IsDBNull(reader.GetOrdinal("IntervalHours")) ? null : reader.GetInt32(reader.GetOrdinal("IntervalHours")),
                    TotalDays = reader.IsDBNull(reader.GetOrdinal("TotalDays")) ? null : reader.GetInt32(reader.GetOrdinal("TotalDays")),
                    QuantityMg = reader.IsDBNull(reader.GetOrdinal("QuantityMg")) ? null : reader.GetInt32(reader.GetOrdinal("QuantityMg")),
                    TreatmentId = reader.IsDBNull(reader.GetOrdinal("TreatmentId")) ? null : reader.GetInt32(reader.GetOrdinal("TreatmentId")),
                    StageId = reader.GetInt32(reader.GetOrdinal("StageId"))
                });
            }

            return medications;
        }

        public Medication? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Medications WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Medication
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IntervalHours = reader.IsDBNull(reader.GetOrdinal("IntervalHours")) ? null : reader.GetInt32(reader.GetOrdinal("IntervalHours")),
                    TotalDays = reader.IsDBNull(reader.GetOrdinal("TotalDays")) ? null : reader.GetInt32(reader.GetOrdinal("TotalDays")),
                    QuantityMg = reader.IsDBNull(reader.GetOrdinal("QuantityMg")) ? null : reader.GetInt32(reader.GetOrdinal("QuantityMg")),
                    TreatmentId = reader.IsDBNull(reader.GetOrdinal("TreatmentId")) ? null : reader.GetInt32(reader.GetOrdinal("TreatmentId")),
                    StageId = reader.GetInt32(reader.GetOrdinal("StageId"))
                };
            }

            return null;
        }

        public int Add(Medication medication)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Medications (Name, IntervalHours, TotalDays, QuantityMg, TreatmentId, StageId)
                VALUES (@name, @intervalHours, @totalDays, @quantityMg, @treatmentId, @stageId);
                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("@name", medication.Name);
            command.Parameters.AddWithValue("@intervalHours", (object?)medication.IntervalHours ?? DBNull.Value);
            command.Parameters.AddWithValue("@totalDays", (object?)medication.TotalDays ?? DBNull.Value);
            command.Parameters.AddWithValue("@quantityMg", (object?)medication.QuantityMg ?? DBNull.Value);
            command.Parameters.AddWithValue("@treatmentId", (object?)medication.TreatmentId ?? DBNull.Value);
            command.Parameters.AddWithValue("@stageId", medication.StageId);

            var insertedId = (long)command.ExecuteScalar()!;
            return (int)insertedId;
        }

        public bool Update(Medication medication)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Medications
                SET Name = @name,
                    IntervalHours = @intervalHours,
                    TotalDays = @totalDays,
                    QuantityMg = @quantityMg,
                    TreatmentId = @treatmentId,
                    StageId = @stageId
                WHERE Id = @id
            ";

            command.Parameters.AddWithValue("@id", medication.Id);
            command.Parameters.AddWithValue("@name", medication.Name);
            command.Parameters.AddWithValue("@intervalHours", (object?)medication.IntervalHours ?? DBNull.Value);
            command.Parameters.AddWithValue("@totalDays", (object?)medication.TotalDays ?? DBNull.Value);
            command.Parameters.AddWithValue("@quantityMg", (object?)medication.QuantityMg ?? DBNull.Value);
            command.Parameters.AddWithValue("@treatmentId", (object?)medication.TreatmentId ?? DBNull.Value);
            command.Parameters.AddWithValue("@stageId", medication.StageId);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Medications WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
}
