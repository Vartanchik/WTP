using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.WebAPI.ViewModels
{
    public class UserFilterViewModel
    {
        public UserFilterViewModel(List<AppUserDto> users, string name)
        {
            // Setup start element, which allows to select all
            users.Insert(0, new AppUserDto { UserName = "All", Id = 0 });
            Users = new SelectList(users, "Id", "Name");
            SelectedName = name;
        }
        public SelectList Users { get; private set; } //List of users
        public string SelectedName { get; private set; } // Input name
    }
}
