using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WTP.WebAPI.Utility.Extensions
{
    public static class ControllerExtension
    {
        public static int GetCurrentUserId(this Controller controller)
        {
            int userId = Convert.ToInt32(controller.User.Claims.First(c => c.Type == "UserID").Value);

            return userId;
        }
    }
}
