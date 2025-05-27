using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information_dissemination_system.Models
{
    public class Comment
    {
        private string _content;
        public int Id { get; set; }
        public int PostId { get; set; }
        public int SubscriberId { get; set; }

        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Content不能为空");
                _content = value;
            }
        }

        public int? ParentId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}