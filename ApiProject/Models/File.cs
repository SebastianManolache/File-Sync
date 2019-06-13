﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
