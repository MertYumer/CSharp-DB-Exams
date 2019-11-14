namespace MusicHub
{
    using System;
    using System.Globalization;

    using AutoMapper;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.DTO.ImportDtos;

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
        }
    }
}
