using MedReminder.Library.Models; 
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace MedReminder.API.Database
{
    public class SymptomRepository
    {
        private readonly string _connectionString;

        public SymptomRepository(string connectionString)
        {
            _connectionString = connectionString;
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

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Symptoms WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Symptom
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                };
            }
            return null;
        }

        public Symptom AddOrUpdate(Symptom symptom)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            if (symptom.Id <= 0)
            {
                command.CommandText = @"
                    INSERT INTO Symptoms (Name, IsActive)
                    VALUES (@name, @isActive);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@name", symptom.Name);
                command.Parameters.AddWithValue("@isActive", symptom.IsActive);

                var result = command.ExecuteScalar();
                symptom.Id = Convert.ToInt32(result);
            }
            else
            {
                command.CommandText = @"
                    UPDATE Symptoms
                    SET Name = @name,
                        IsActive = @isActive
                    WHERE Id = @id";

                command.Parameters.AddWithValue("@name", symptom.Name);
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
    }
}
