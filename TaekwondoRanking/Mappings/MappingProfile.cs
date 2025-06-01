using AutoMapper;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Athlete, AthletePointsViewModel>().ReverseMap();
            CreateMap<Athlete, AthleteSearchResultViewModel>().ReverseMap();
            CreateMap<Result, AthleteSearchResultViewModel>().ReverseMap();
            CreateMap<Athlete, AthleteTournamentHistoryViewModel>().ReverseMap();
            CreateMap<Competition, CompetitionViewModel>().ReverseMap();
            CreateMap<CompetitionDbContext, CompetitionViewModel>().ReverseMap(); // Optional
            CreateMap<SubCompetition1, CompetitionViewModel>().ReverseMap();
            CreateMap<SubCompetition2, CompetitionViewModel>().ReverseMap();
            CreateMap<Competition, SubCompetition2ViewModel>().ReverseMap();
            CreateMap<SubCompetition2, SubCompetition2ViewModel>().ReverseMap();

            CreateMap<SubCompetition2, SubCompetition2ViewModel>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.IdSubCompetition2))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.IdCategoryNavigation.NameCategory));

            CreateMap<SubCompetition1, SubCompetition1ViewModel>()
                .ForMember(dest => dest.AgeClassName, opt => opt.MapFrom(src => src.AgeClassNavigation.NameAgeClass))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.SubCompetition2s));




        }
    }
}
