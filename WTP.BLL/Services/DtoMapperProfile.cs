using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Entities.TeamEntities;

namespace WTP.BLL.Services.Concrete
{
    public class DtoMapperProfile : Profile
    {
        private readonly string _defaultPhoto;

        public DtoMapperProfile(string defaultPhoto)
        {
            this._defaultPhoto = defaultPhoto;

            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src)))
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetLanguagesDto(src)));

            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToSave(src)))
                .ForMember(dest => dest.AppUserLanguages,
                           config => config.MapFrom(src => GetUserToLanguages(src)));

            CreateMap<LanguageDto, Language>();
            CreateMap<Language, LanguageDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();
            CreateMap<GenderDto, Gender>();
            CreateMap<Gender, GenderDto>();
            CreateMap<TeamDto, Team>();
            CreateMap<Team, TeamDto>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<GameDto, Game>();
            CreateMap<Game, GameDto>();
            CreateMap<GoalDto, Goal>();
            CreateMap<Goal, GoalDto>();
            CreateMap<ServerDto, Server>();
            CreateMap<Server, ServerDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();
            CreateMap<MatchDto, Match>();
            CreateMap<Match, MatchDto>();
            CreateMap<Rank, RankDto>();
            CreateMap<RankDto, Rank>();
            CreateMap<CreatePlayerDto, Player>();
            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src.AppUser)))
                .ForMember(dest => dest.Age,
                           config => config.MapFrom(src => AgeCalculation(src.AppUser)))
                .ForMember(dest => dest.Rank,
                           config => config.MapFrom(src => src.Rank.Name))
                .ForMember(dest => dest.Server,
                           config => config.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.Goal,
                           config => config.MapFrom(src => src.Goal.Name))
                .ForMember(dest => dest.Country,
                           config => config.MapFrom(src => src.AppUser.Country.Name))
                .ForMember(dest => dest.TeamId,
                           config => config.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.TeamName,
                           config => config.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.TeamPhoto,
                           config => config.MapFrom(src => TeamPhotoToView(src.Team)))
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetPlayerLanguages(src.AppUser)));
            CreateMap<Player, PlayerListItemDto>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src.AppUser)))
                .ForMember(dest => dest.Game,
                           config => config.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.Rank,
                           config => config.MapFrom(src => src.Rank.Name))
                .ForMember(dest => dest.Server,
                           config => config.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.Goal,
                           config => config.MapFrom(src => src.Goal.Name))
                .ForMember(dest => dest.Invitations,
                           config => config.MapFrom(src => GetListOfInvitations(src.Invitations)));
            CreateMap<CreateTeamDto, Team>();
            CreateMap<Team, TeamListItemDto>()
                .ForMember(dest => dest.Game,
                           config => config.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.Server,
                           config => config.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.Goal,
                           config => config.MapFrom(src => src.Goal.Name))
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => TeamPhotoToView(src)))
                .ForMember(dest => dest.Invitations,
                           config => config.MapFrom(src => GetListOfInvitations(src.Invitations)));
            CreateMap<Invitation, InvitationListItemDto>()
                .ForMember(dest => dest.PlayerName,
                           config => config.MapFrom(src => src.Player.Name))
                .ForMember(dest => dest.TeamName,
                           config => config.MapFrom(src => src.Team.Name));
        }

        private string PhotoToView(AppUser user)
        {
            return string.IsNullOrEmpty(user.Photo)
                ? _defaultPhoto
                : user.Photo;
        }

        private string PhotoToSave(AppUserDto user)
        {
            return user.Photo == _defaultPhoto
                ? null
                : user.Photo;
        }

        private List<AppUserLanguage> GetUserToLanguages(AppUserDto user)
        {
            return new List<AppUserLanguage>(user.Languages.Select(x =>
                new AppUserLanguage
                {
                    AppUserId = user.Id,
                    LanguageId = x.Id
                }));

        }

        private List<LanguageDto> GetLanguagesDto(AppUser user)
        {
            return new List<LanguageDto>(user.AppUserLanguages.Select(x =>
                new LanguageDto
                {
                    Id = x.LanguageId,
                    Name = x.Language.Name
                }));

        }

        private string TeamPhotoToView(Team team)
        {
            return string.IsNullOrEmpty(team.Photo)
                ? _defaultPhoto
                : team.Photo;
        }

        private List<PlayerListItemDto> GetListOfPlayers(List<Player> players)
        {
            return new List<PlayerListItemDto>(players.Select(x =>
                new PlayerListItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Game = x.Game.Name,
                    Rank = x.Rank.Name,
                    Server = x.Server.Name,
                    Goal = x.Goal.Name,
                    About = x.About,
                    Decency = (int)x.Decency
                }));
        }

        private List<InvitationListItemDto> GetListOfInvitations(ICollection<Invitation> invitations)
        {
            return new List<InvitationListItemDto>(invitations.Select(i =>
                new InvitationListItemDto
                {
                    Id = i.Id,
                    PlayerName = i.Player.Name,
                    TeamName = i.Team.Name,
                    Author = i.Author.ToString()
                }));
        }

        private IList<string> GetPlayerLanguages(AppUser user)
        {
            return new List<string>(user.AppUserLanguages.Select(x => x.Language.Name));

        }

        private int AgeCalculation(AppUser user)
        {
            if (user.DateOfBirth == null) return 0;

            var today = DateTime.Today;

            var birthday = user.DateOfBirth.Value;

            var age = today.Year - birthday.Year;

            if (birthday.Date > today.AddYears(-age)) return age--;

            return age;
        }
    }
}
