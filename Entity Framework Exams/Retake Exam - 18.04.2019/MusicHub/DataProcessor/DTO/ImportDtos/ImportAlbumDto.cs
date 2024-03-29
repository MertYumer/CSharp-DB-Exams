﻿namespace MusicHub.DataProcessor.DTO.ImportDtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ImportAlbumDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string ReleaseDate { get; set; }
    }
}
