﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        [Required]
        [ForeignKey("Song")]
        public int SongId { get; set; }
        public Song Song { get; set; }

        [Required]
        [ForeignKey("Performer")]
        public int PerformerId { get; set; }
        public Performer Performer { get; set; }
    }
}
