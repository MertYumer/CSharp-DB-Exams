﻿namespace VaporStore.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;

    using VaporStore.Data.Models;
    public class ImportUserDto

    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        [Required]
        public ImportCardDto[] Cards { get; set; }
    }
}
