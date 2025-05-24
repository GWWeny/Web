using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class CustomDataAccess1
    {
        //用于封装对custom表的操作
        private string connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

        public void Insert(Custom custom)
        {
            string query = "INSERT INTO custom (cname, departID, age, ename, password) VALUES (@cname, @departID, @age, @ename, @password)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cname", custom.Cname);
                command.Parameters.AddWithValue("@departID", custom.DepartID);
                command.Parameters.AddWithValue("@age", custom.Age);
                command.Parameters.AddWithValue("@ename", custom.Ename);
                command.Parameters.AddWithValue("@password", custom.Password);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Custom custom)
        {
            string query = "UPDATE custom SET cname = @cname, departID = @departID, age = @age, ename = @ename, password = @password WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", custom.Id);
                command.Parameters.AddWithValue("@cname", custom.Cname);
                command.Parameters.AddWithValue("@departID", custom.DepartID);
                command.Parameters.AddWithValue("@age", custom.Age);
                command.Parameters.AddWithValue("@ename", custom.Ename);
                command.Parameters.AddWithValue("@password", custom.Password);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM custom WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //查，没什么用
        public Custom Get(int id)
        {
            string query = "SELECT * FROM custom WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Custom
                        {
                            Id = reader.GetInt32(0),
                            Cname = reader.GetString(1),
                            DepartID = reader.GetInt32(2),
                            Age = reader.GetInt32(3),
                            Ename = reader.GetString(4),
                            Password = reader.GetString(5)
                        };
                    }
                    return null;
                }
            }
        }

        public List<Custom> GetAll()
        {
            List<Custom> customs = new List<Custom>();
            string query = "SELECT * FROM custom";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customs.Add(new Custom
                        {
                            Id = reader.GetInt32(0),
                            Cname = reader.GetString(1),
                            DepartID = reader.GetInt32(2),
                            Age = reader.GetInt32(3),
                            Ename = reader.GetString(4),
                            Password = reader.GetString(5)
                        });
                    }
                }
            }
            return customs;
        }

    }
}