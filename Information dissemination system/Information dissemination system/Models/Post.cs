using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information_dissemination_system.Models
{
    public class Post
    {
        private string _title;
        private string _content;

        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title不能为空");
                _title = value;
            }
        }

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

        public string CoverImage { get; set; }
        public int Views { get; set; }
        public bool IsTop { get; set; }
        public bool IsPublished { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}