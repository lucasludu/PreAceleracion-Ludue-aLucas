using AutoMapper;
using Disney.Auth.Request;
using Disney.Auth.Response;
using Disney.Dtos.Character;
using Disney.Dtos.Genre;
using Disney.Dtos.Movies;
using Disney.Entities;

namespace Disney.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //--------------------- USER ---------------------
            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();

            //--------------------- CHARACTER ---------------------
            CreateMap<Character, CharacterDto>().ReverseMap();
            CreateMap<Character, CharacterGetDto>().ReverseMap();
            CreateMap<Character, CharacterListItemDto>().ReverseMap();
            CreateMap<CharacterCreateDto, Character>()
                .ForMember(ch => ch.Image, option => option.Ignore())
                .ReverseMap();

            CreateMap<CharacterDto, CharacterCreateDto>().ReverseMap();

            //--------------------- CHARACTER ---------------------
            CreateMap<Movie, MovieDetailsDto>().ReverseMap();
            CreateMap<Movie, MovieListDto>().ReverseMap();
            CreateMap<MovieCreateDto, Movie>()
                .ForMember(m => m.Image, option => option.Ignore())
                .ReverseMap();

            //--------------------- CHARACTER ---------------------
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenreCreateDto, Genre>()
                .ForMember(g => g.Image, option => option.Ignore())
                .ReverseMap();
        }
    }
}
