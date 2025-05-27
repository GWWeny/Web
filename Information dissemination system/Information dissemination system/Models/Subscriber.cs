using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information_dissemination_system.Models
{
    public class Subscriber
    {
        //C# 8.0才能用
        /*public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }*/

        private string _username;
        private string _password;

        public int Id { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username 不能为空");
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password 不能为空");
                _password = value;
            }
        }

        public string Email { get; set; }  // 可为空，不强制检查
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}