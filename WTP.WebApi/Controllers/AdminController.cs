using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.HistoryService;
using WTP.BLL.Shared;

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
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateAdminAccount([FromBody] RegisterDto formdata)
        {
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

            string errorResult = "";
            foreach (var t in errorList)
                errorResult += t;

            return BadRequest(new ResponseDto(400, "Creating admin is failed.", errorResult));
        }

        //Create User account
        [HttpPost]
        [Route("users/profiles")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateUserProfile([FromBody] RegisterDto formdata)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formdata.Password, adminId);

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

            string errorResult = "";
            foreach (var t in errorList)
                errorResult += t;

            return BadRequest(new ResponseDto(400, "Creating user account is failed.", errorResult));
        }

        //Create User account
        [HttpPost]
        [Route("users/moderator")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateModeratorProfile([FromBody] RegisterDto formdata)
        {
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

            string errorResult = "";
            foreach (var t in errorList)
                errorResult += t;

            return BadRequest(new ResponseDto(400, "Creating moderator account is failed.", errorResult));
        }


        ////Get List of all Users
        [HttpGet]
        [Route("users")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<AppUserDto[]> GetUsersProfile()
        {
            List<AppUserDto> result = new List<AppUserDto>();
            var users = await _appUserService.GetUsersList();

            if (users.Count() == 0)
                return result.ToArray();
            //return Ok("List of Users are empty!");

            foreach (var user in users)
            {               
                var appUserDtoViewModel = new AppUserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Photo = user.Photo,
                    UserName = user.UserName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Country = user.Country,
                    Steam = user.Steam,
                    Players = user.Players,
                    Teams = user.Teams,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    IsDeleted = user.IsDeleted
                };

                //Check if account isn't deleted
                if (user.IsDeleted != true)
                    result.Add(appUserDtoViewModel);
            }
            return result.ToArray();
        }

        //Update user's account
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] AppUserDto formdata, [FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
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
                return NotFound(new ResponseDto
                {
                    StatusCode = 404,
                    Message = "User with id:" + id + " wasnt found."
                });
            }


            user.UserName = formdata.UserName;
            user.Email = formdata.Email;

            var result = await _appUserService.UpdateAsync(user, adminId);

            if (result.Succeeded)
            {
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
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

            string errorResult = "";
            foreach (var t in errorList)
                errorResult += t;

            return BadRequest(new ResponseDto
            {
                StatusCode = 404,
                Message = "User with id:" + id + " wasnt found.",
                Info = errorResult
            });
        }

        //Delete user's account by id
        [HttpDelete]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;
            bool success = await _appUserService.DeleteAsync(id, adminId);

            if (success)
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
                    Message = "User's profile with id " + id + " was deleted."
                });

            return NotFound(new ResponseDto
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

        }

        //Lock users account by id
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}/block")]
        public async Task<IActionResult> LockUser([FromBody]LockDto formDate, [FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            bool success = await _appUserService.LockAsync(id, formDate.Days, adminId);

            if (success)
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
                    Message = "User's profile with id " + id + " was locked."
                });

            return NotFound(new ResponseDto
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

        }

        //UnLock user's account by id
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/{id}/unblock")]
        public async Task<IActionResult> UnLockUser([FromRoute]int id)
        {
            //int userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
            int adminId = 1;

            bool success = await _appUserService.UnLockAsync(id, adminId);

            if (success)
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
                    Message = "User's profile with id " + id + " was unlocked."
                });

            return NotFound(new ResponseDto
            {
                StatusCode = 404,
                Message = "User's profile with id " + id + " wasn't found."
            });

            //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status406NotAcceptable);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/pagination")]
        public async Task<UserIndexDto> UserIndex(string name, int page = 1,
            SortState sortOrder = SortState.NameAsc, bool enableDeleted = true, bool enableLocked = true)
        {
            int pageSize = 3;

            List<AppUserDto> users = new List<AppUserDto>(await _appUserService.GetUsersList());
            if (users == null)
                return null;
                //return NoContent();

            //Filtration
            users = _appUserService.FilterByName(users, name);

            // Sorting
            users = _appUserService.SortByParam(users, sortOrder, enableDeleted, enableLocked);

            // Pagination
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            UserIndexDto viewModel = new UserIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new UserSortDto(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Users = items
            };
            return viewModel;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("history")]
        public async Task<HistoryIndexDto> HistoryIndex(string name, int page = 1,
            HistorySortState sortOrder = HistorySortState.DateDesc)
        {
            int pageSize = 3;
            List<HistoryDto> histories = new List<HistoryDto>(await _historyService.GetHistoryList());

            if (histories == null)
                return null;
                //return NoContent();

            //Filtration
            histories = _historyService.FilterByUserName(histories, name).ToList();

            // Sorting
            histories = _historyService.SortByParam(histories, sortOrder).ToList();


            // Pagination
            var count = histories.Count();
            var items = histories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            HistoryIndexDto viewModel = new HistoryIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new HistorySortDto(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Histories = items
            };
            return viewModel;
        }


    }
}