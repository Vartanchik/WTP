using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class HistoryFilterDto
    {
        public HistoryFilterDto(List<HistoryDto> histories, string name)
        {
            // Setup start element, which allows to select all
            histories.Insert(0, new HistoryDto { NewUserName = "All", Id = 0 });
            Histories = new SelectList(histories, "Id", "Name");
            SelectedName = name;
        }
        public SelectList Histories { get; private set; } //List of users
        public string SelectedName { get; private set; } // Input name

    }
}
