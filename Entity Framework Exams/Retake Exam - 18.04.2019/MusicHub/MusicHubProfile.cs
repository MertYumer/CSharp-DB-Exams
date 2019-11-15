namespace MusicHub
{
    using System;
    using System.Globalization;

    using AutoMapper;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.DTO.ImportDtos;
    using DataProcessor.DTO.ExportDtos;
    using System.Linq;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            this.CreateMap<ImportWriterDto, Writer>();

            this.CreateMap<ImportAlbumDto, Album>();

            this.CreateMap<ImportProducerDto, Producer>();

            this.CreateMap<ImportSongDto, Song>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.Parse(src.Duration)))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom
                (src => DateTime.ParseExact(src.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => (Genre)Enum.Parse(typeof(Genre), src.Genre)));

            this.CreateMap<Song, ExportSongWithDuration>()
                .ForMember(dest => dest.SongName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Writer, opt => opt.MapFrom(src => src.Writer.Name))
                .ForMember(dest => dest.Performer, opt => opt.MapFrom
                (src => src.SongPerformers
                .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                .FirstOrDefault()))
                .ForMember(dest => dest.AlbumProducer, opt => opt.MapFrom(src => src.Album.Producer.Name))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString()));
        }
    }
}
