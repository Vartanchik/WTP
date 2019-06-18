using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerManageFilterDto
    {
        public PlayerManageFilterDto(List<PlayerShortDto> players, string name)
        {
            // Setup start element, which allows to select all
            players.Insert(0, new PlayerShortDto { Name = "All", Id = 0 });
            Players = new SelectList(players, "Id", "Name");
            SelectedName = name;
        }
        public SelectList Players { get; private set; } //List of users
        public string SelectedName { get; private set; } // Input name
    }
}
