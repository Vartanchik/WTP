using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class UserFilterDto
    {
        public UserFilterDto(List<AppUserDto> users, string name)
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
