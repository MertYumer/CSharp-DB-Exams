namespace MusicHub.DataProcessor.DTO.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ExportSongWithDuration
    {
        public string SongName { get; set; }

        public string Writer { get; set; }

        public string Performer { get; set; }

        public string AlbumProducer { get; set; }

        public string Duration { get; set; }
    }
}
