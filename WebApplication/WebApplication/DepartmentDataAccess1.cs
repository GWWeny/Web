using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class DepartmentDataAccess1
    {
        //用于封装对department表的操作
        private string connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

        public void Insert(Department department)
        {
            string query = "INSERT INTO department (departname, description) VALUES (@departname, @description)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@departname", department.Departname);
                command.Parameters.AddWithValue("@description", department.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Department department)
        {
            string query = "UPDATE department SET departname = @departname, description = @description WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", department.Id);
                command.Parameters.AddWithValue("@departname", department.Departname);
                command.Parameters.AddWithValue("@description", department.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM department WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Department Get(int id)
        {
            string query = "SELECT * FROM department WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Department
                        {
                            Id = reader.GetInt32(0),
                            Departname = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                    }
                    return null;
                }
            }
        }

        public List<Department> GetAll()
        {
            List<Department> departments = new List<Department>();
            string query = "SELECT * FROM department";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32(0),
                            Departname = reader.GetString(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }
            return departments;
        }

        // DepartmentDataAccess1.cs 新增方法
        public List<Department> GetActiveDepartments()
        {
            const string query = @"SELECT Id 
                         FROM department 
                         WHERE IsActive = 1 
                         ORDER BY Id";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var departments = new List<Department>();
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32(0)
                        });
                    }
                    return departments;
                }
            }
        }
    }
}