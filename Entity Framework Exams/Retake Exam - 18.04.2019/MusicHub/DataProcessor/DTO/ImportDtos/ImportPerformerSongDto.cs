namespace MusicHub.DataProcessor.DTO.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ImportPerformerSongDto
    {
        [XmlAttribute("id")]
        [Required]
        public int SongId { get; set; }
    }
}
