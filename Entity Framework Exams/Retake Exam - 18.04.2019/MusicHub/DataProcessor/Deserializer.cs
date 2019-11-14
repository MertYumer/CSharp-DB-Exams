namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using AutoMapper;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DTO.ImportDtos;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writerDtos = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString);
            var writers = new List<Writer>();

            var sb = new StringBuilder();

            foreach (var writerDto in writerDtos)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = Mapper.Map<Writer>(writerDto);
                writers.Add(writer);
                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }

            context.Writers.AddRange(writers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producerDtos = JsonConvert.DeserializeObject<ImportProducerDto[]>(jsonString);
            var producers = new List<Producer>();

            var sb = new StringBuilder();

            foreach (var producerDto in producerDtos)
            {
                var isValidProducer = IsValid(producerDto);
                var areValidAlbums = producerDto.Albums.All(s => IsValid(s));

                if (!isValidProducer || !areValidAlbums)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = new Producer
                {
                    Name = producerDto.Name,
                    PhoneNumber = producerDto.PhoneNumber,
                    Pseudonym = producerDto.Pseudonym
                };

                foreach (var albumDto in producerDto.Albums)
                {
                    var album = new Album
                    {
                        Name = albumDto.Name,
                        ReleaseDate = DateTime.ParseExact(albumDto.ReleaseDate,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture)
                    };

                    producer.Albums.Add(album);
                }

                producers.Add(producer);

                if (producer.PhoneNumber == null)
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, 
                        producer.Name,
                        producer.Albums.Count));
                }

                else
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone,
                       producer.Name,
                       producer.PhoneNumber,
                       producer.Albums.Count));
                }
            }

            context.Producers.AddRange(producers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSongDto[]),
                    new XmlRootAttribute("Songs"));

            var songDtos = (ImportSongDto[])(xmlSerializer.Deserialize(new StringReader(xmlString)));
            var songs = new List<Song>();

            StringBuilder sb = new StringBuilder();

            foreach (var songDto in songDtos)
            {
                var isValidSong = IsValid(songDto);
                var isValidGenre = Enum.IsDefined(typeof(Genre), songDto.Genre);
                var isValidAlbum = context.Albums.Any(a => a.Id == songDto.AlbumId);
                var isValidWriter = context.Writers.Any(w => w.Id == songDto.WriterId);

                if (!isValidSong || !isValidGenre || !isValidAlbum || !isValidWriter)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = Mapper.Map<Song>(songDto);
                songs.Add(song);

                sb.AppendLine(string.Format(
                    SuccessfullyImportedSong, 
                    song.Name, 
                    song.Genre.ToString(), 
                    song.Duration.ToString()));
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPerformerDto[]),
                    new XmlRootAttribute("Performers"));

            var performerDtos = (ImportPerformerDto[])(xmlSerializer.Deserialize(new StringReader(xmlString)));
            var performers = new List<Performer>();

            StringBuilder sb = new StringBuilder();

            foreach (var performerDto in performerDtos)
            {
                var isValidPerformer = IsValid(performerDto);

                if (!isValidPerformer)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var idExists = true;

                foreach (var songDto in performerDto.PerformerSongs)
                {
                    idExists = context.Songs.Any(s => s.Id == songDto.SongId);

                    if (!idExists)
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }
                }

                if (idExists)
                {
                    var performer = new Performer
                    {
                        FirstName = performerDto.FirstName,
                        LastName = performerDto.LastName,
                        Age = performerDto.Age,
                        NetWorth = performerDto.NetWorth
                    };

                    foreach (var songDto in performerDto.PerformerSongs)
                    {
                        var song = context.Songs.FirstOrDefault(s => s.Id == songDto.SongId);

                        var songPerformer = new SongPerformer
                        {
                            PerformerId = performer.Id,
                            Performer = performer,
                            SongId = song.Id,
                            Song = song
                        };

                        performer.PerformerSongs.Add(songPerformer);
                    }

                    performers.Add(performer);

                    sb.AppendLine(string.Format(
                        SuccessfullyImportedPerformer,
                        performer.FirstName,
                        performer.PerformerSongs.Count));
                }
            }

            context.Performers.AddRange(performers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}