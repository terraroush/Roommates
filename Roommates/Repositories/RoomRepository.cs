using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoomRepository : BaseRepository
    {
        public List<Room> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, MaxOccupancy FROM Room";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Room> rooms = new List<Room>();

                    while (reader.Read())
                    {
                        
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        int maxOccupancyColumnPosition = reader.GetOrdinal("MaxOccupancy");
                        int maxOccupancy = reader.GetInt32(maxOccupancyColumnPosition);

                    
                        Room room = new Room
                        {
                            Id = idValue,
                            Name = nameValue,
                            MaxOccupancy = maxOccupancy,
                        };

                       
                        rooms.Add(room);
                    }

                   
                    reader.Close();

               
                    return rooms;
                }
            }
        }
        public Room GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, MaxOccupancy FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Room room = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        room = new Room
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                        };
                    }

                    reader.Close();

                    return room;
                }
            }
        }
        public void Insert(Room room)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Room (Name, MaxOccupancy) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name, @maxOccupancy)";
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    int id = (int)cmd.ExecuteScalar();

                    room.Id = id;
                }
            }

            // when this method is finished we can look in the database and see the new room.
        }
      
        public RoomRepository(string connectionString) : base(connectionString) { }

        // ...We'll add some methods shortly...
    }
}                                         