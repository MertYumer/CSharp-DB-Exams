namespace MusicHub.DataProcessor
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using AutoMapper;
    using Data;
    using DTO.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Producers
                .FirstOrDefault(p => p.Id == producerId)
                .Albums
                .OrderByDescending(a => a.Price)
                .ToArray();

            var albumDtos = albums
                .Select(a => new ExportAlbumDto
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,

                    Songs = a.Songs
                        .OrderByDescending(s => s.Name)
                        .ThenBy(s => s.Writer)
                        .Select(s => new ExportSongDto
                        { 
                            SongName = s.Name,
                            Price = $"{s.Price:f2}",
                            Writer = s.Writer.Name
                        })
                        .ToArray(),

                    AlbumPrice = $"{a.Price:f2}"
                })
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(albumDtos, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer.Name)
                .ThenBy(s => s.SongPerformers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}"))
                .ToArray();

            var songDtos = Mapper.Map<ExportSongWithDuration[]>(songs);

            var xmlSerializer = new XmlSerializer(typeof(ExportSongWithDuration[]),
                            new XmlRootAttribute("Songs"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), songDtos, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}