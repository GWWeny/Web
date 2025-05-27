using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information_dissemination_system.Models
{
    public class Category
    {
        private string _name;
        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name不能为空");
                _name = value;
            }
        }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}