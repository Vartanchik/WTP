using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.History;

namespace WTP.WebAPI.ViewModels
{
    public class HistoryFilterViewModel
    {
        public HistoryFilterViewModel(List<HistoryDto> histories, string name)
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
