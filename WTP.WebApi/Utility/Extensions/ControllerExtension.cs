using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WTP.WebAPI.Utility.Extensions
{
    public static class ControllerExtension
    {
        public static int GetCurrentUserId(this ControllerBase controller)
        {
            var id = Convert.ToInt32(controller.User.Claims.Where(c => c.Type == "UserID").First().Value);

            return id;
        }
    }
}
