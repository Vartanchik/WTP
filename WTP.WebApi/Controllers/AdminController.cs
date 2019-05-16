using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.History;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.HistoryService;
using WTP.BLL.Shared.HistoryState;
using WTP.BLL.Shared.SortState;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IAppUserService _appUserService;

        public AdminController(IAppUserService appUserService, IHistoryService historyService)
        {
            _appUserService = appUserService;
            _historyService = historyService;
        }

        //Create Admin account
        [HttpPost]
        [Route("profiles")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateAdminAccount([FromBody] RegisterViewModel formdata)
        {
            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAdminAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                //Confirmation Email
                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return Ok(new ErrorResponseModel { Message = errorList });
        }

        //Create User account
        [HttpPost]
        [Route("users/profiles")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateUserProfile([FromBody] RegisterViewModel formdata)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formdata.Password,adminId);

            if (result.Succeeded)
            {
                // Sending Confirmation Email

                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return BadRequest(new JsonResult(errorList));
        }

        //Create User account
        [HttpPost]
        [Route("users/moderator")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateModeratorProfile([FromBody] RegisterViewModel formdata)
        {
            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateModeratorAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                // Sending Confirmation Email

                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return BadRequest(new JsonResult(errorList));
        }


        ////Get List of all Users
        [HttpGet]
        [Route("users")]
        //[Authorize(Policy = "RequireAdministratorRole")]
        public async Task<UserDataForAdminViewModel[]> GetUsersProfile()
        {
            List<UserDataForAdminViewModel> result = new List<UserDataForAdminViewModel>();
            var users = await _appUserService.GetAllUsersAsync();

            if (users.Count() == 0)
                return result.ToArray();
            //return Ok("List of Users are empty!");

            foreach (var user in users)
            {
                var langs = new List<LanguageDto>();

                foreach (var item in user.AppUserLanguages)
                {
                    langs.Add(new LanguageDto
                    {
                        Id = item.LanguageId,
                        Name = item.Language.Name
                    });
                }
                // Convert userDto to UserDataForAdminViewModel
                var appUserDtoViewModel = new UserDataForAdminViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Photo = user.Photo,
                    UserName = user.UserName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Languages = langs,
                    Country = user.Country,
                    Steam = user.Steam,
                    Players = user.Players,
                    Teams = user.Teams,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    DeletedStatus = user.DeletedStatus
                };

                //Check if account isn't deleted
                if (user.DeletedStatus != true)
                    result.Add(appUserDtoViewModel);
            }
            return result.ToArray();
            //return Ok(new JsonResult(result.ToArray()));
            //return Ok();
        }

        //Update user's account
        [HttpPut]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserForAdminViewModel formdata, [FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel
                {
                    StatusCode = 400,
                    Message = "Model is not valid."
                });
            }

            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();
            

            var user = await _appUserService.GetAsync(id);

            if (user == null)
            {
                return NotFound(new ResponseViewModel
                {
                    StatusCode = 404,
                    Message = "User with id:"+id+" wasnt found."
                });
            }

           
            user.UserName = formdata.UserName;
            user.Email = formdata.Email;

            var result = await _appUserService.UpdateAsync(user, adminId);

            if (result.Succeeded)
            {
                return Ok(new ResponseViewModel
                {
                    StatusCode = 202,
                    Message = "User's profile with id " + id + " was updated."
                });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }
            }

            return BadRequest(new ErrorResponseModel { Message = errorList });
        }

        //Delete user's account by id
        [HttpDelete]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;
            bool success = await _appUserService.DeleteAsync(id, adminId);

            if (success)
                return Ok(new ResponseViewModel
                {
                    StatusCode = 202,
                    Message = "User's profile with id " + id + " was deleted."
                });

            return NotFound(new ResponseViewModel
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

        }

        //Lock users account by id
        [HttpPut]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}/block")]
        public async Task<IActionResult> LockUser([FromBody]LockViewModel formDate, [FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            bool success = await _appUserService.LockAsync(id, formDate.Days, adminId);

            if (success)
                return Ok(new ResponseViewModel
                {
                    StatusCode = 202,
                    Message = "User's profile with id " + id + " was locked."
                });

            return NotFound(new ResponseViewModel
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

        }

        //UnLock user's account by id
        [HttpPut]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}/unblock")]
        public async Task<IActionResult> UnLockUser([FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            bool success = await _appUserService.UnLockAsync(id, adminId);
            
            if (success)
                return Ok(new ResponseViewModel
                {
                    StatusCode = 202,
                    Message = "User's profile with id " + id + " was unlocked."
                });
            
            return NotFound(new ResponseViewModel
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

            //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status406NotAcceptable);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/pagination")]
        public async Task<IActionResult> UserIndex(string name, int page = 1,
            SortState sortOrder = SortState.NameAsc, bool enableDeleted = true, bool enableLocked=true)
        {
            int pageSize = 3;

            List<AppUserDto> users = new List<AppUserDto>(await _appUserService.GetAllUsersAsync());
            if (users == null)
                return NoContent();

            //Filtration
            users = _appUserService.Filter(users, name);

            // Sorting
            users = _appUserService.Sort(users, sortOrder, enableDeleted,enableLocked);

            // Pagination
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            UserIndexViewModel viewModel = new UserIndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new UserSortViewModel(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Users = items
            };
            return Ok(viewModel);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("history")]
        public async Task<IActionResult> HistoryIndex(string name, int page = 1,
            HistorySortState sortOrder = HistorySortState.DateDesc)
        {
            int pageSize = 3;
            List<HistoryDto> histories = new List<HistoryDto>(await _historyService.GetAllAsync());
            
            if (histories == null)
                return NoContent();

            //Filtration
            histories = _historyService.Filter(histories, name);

            // Sorting
            histories = _historyService.Sort(histories, sortOrder);


            // Pagination
            var count = histories.Count();
            var items = histories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            HistoryIndexViewModel viewModel = new HistoryIndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new HistorySortViewModel(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Histories = items
            };
            return Ok(viewModel);
        }

    }
}
