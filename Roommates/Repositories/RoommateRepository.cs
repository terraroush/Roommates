using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, r.Name as RoomName FROM Roommate rmt JOIN Room r ON rmt.RoomId = r.id WHERE rmt.Id = @id" ;
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = new Room{
                            Name = reader.GetString(reader.GetOrdinal("Name")), }
                        };
                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        public RoommateRepository(string connectionString) : base(connectionString) { }

    }
}
