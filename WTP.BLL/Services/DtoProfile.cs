﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.BLL.Services.Concrete
{
    public class DtoProfile : Profile
    {
        private readonly string _defaultPhoto;

        public DtoProfile(string defaultPhoto)
        {
            this._defaultPhoto = defaultPhoto;

            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src)))
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetLanguagesDto(src)))
                .ForMember(dest => dest.DateOfBirth,
                    config => config.MapFrom(
                        src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString(new CultureInfo("de-DE")) : ""));

            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToSave(src)))
                .ForMember(dest => dest.AppUserLanguages,
                           config => config.MapFrom(src => GetUserToLanguages(src)))
                .ForMember(dest => dest.DateOfBirth,
                    config => config.MapFrom(src => String.IsNullOrEmpty(src.DateOfBirth) ? (DateTime?) null : DateTime.Parse(src.DateOfBirth, new CultureInfo("de-DE"))));

            CreateMap<LanguageDto, Language>();
            CreateMap<Language, LanguageDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();
            CreateMap<GenderDto, Gender>();
            CreateMap<Gender, GenderDto>();
            CreateMap<TeamDto, Team>();
            CreateMap<Team, TeamDto>();
            CreateMap<PlayerDto, Player>();
            CreateMap<Player, PlayerDto>();
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
            CreateMap<CreateUpdatePlayerDto, Player>();
            CreateMap<Player, PlayerListItemDto>()
                .ForMember(dest => dest.Game,
                           config => config.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.Rank,
                           config => config.MapFrom(src => src.Rank.Name))
                .ForMember(dest => dest.Server,
                           config => config.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.Goal,
                           config => config.MapFrom(src => src.Goal.Name));

            CreateMap<Player, PlayerShortDto>()
                .ForMember(dest => dest.GameName,
                           config => config.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.RankName,
                           config => config.MapFrom(src => src.Rank.Name))
                .ForMember(dest => dest.ServerName,
                           config => config.MapFrom(src => src.Server.Name))
                .ForMember(dest => dest.TeamName,
                           config => config.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.AppUserName,
                           config => config.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.AppUserEmail,
                           config => config.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.GoalName,
                           config => config.MapFrom(src => src.Goal.Name));            

            CreateMap<History, HistoryDto>();
            CreateMap<HistoryDto, History>();
            CreateMap<PlayerJoinedDto, PlayerDto>();
            CreateMap<PlayerDto,PlayerJoinedDto>();
            CreateMap<AppUserDto, ShortUserFormDto>();
            CreateMap<ShortUserFormDto, AppUserDto>();
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
    }
}
