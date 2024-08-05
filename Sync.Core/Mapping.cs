using AutoMapper;
using Sync.DAL.Models;
using Sync.Services.DTOs;

namespace Sync.Core
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<FieldDto, Field>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Polygons, opt => opt.MapFrom(src => src.Polygon))
              .AfterMap((src, dest) =>
              {
                  dest.Active = true;
                  var count = 0;
                  foreach (var polygon in dest.Polygons)
                  {
                      polygon.FieldId = dest.Id;
                      polygon.Field = dest;
                      polygon.Id = Guid.NewGuid();
                      polygon.PointOrder = ++count;
                  }
              });

            CreateMap<ImageDto, Image>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.FieldId, opt => opt.MapFrom((src, dest, destMember, context) => (context.Items["field"])))
                .AfterMap((src, dest) =>
                {
                    dest.Id = Guid.NewGuid();
                });
            CreateMap<Image, ImageDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Link));

            CreateMap<Polygon, PointDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

            CreateMap<PointDto, Polygon>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}
