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
            // Will hold all the errors related to registration
            List<string> errorList = new List<string>();

            var user = new AppUserDto
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _appUserService.CreateAsync(user, formdata.Password);

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

            //[HttpGet]
            //[Route("history")]
            ////[Authorize(Policy = "RequireAdministratorRole")]
            //public async Task<IActionResult> GetHistory()
            //{
            //    var history = await _historyService.GetAllAsync();
            //    return Ok(history);
            //}

        //Get List of all Users
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

            var result = await _appUserService.UpdateAsync(user);

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
            bool success = await _appUserService.DeleteAsync(id);

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
            bool success = await _appUserService.LockAsync(id, formDate.Days);

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
            bool success = await _appUserService.UnLockAsync(id);
            
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
        public async Task<PageResult<AppUserDto>> Pagination(int? page, int pagesize = 10)
        {
            var users = await _appUserService.GetAllUsersAsync();
            var countDetails = users.Count();
            if (countDetails == 0)
                return null;

            List<AppUserDto> resultList = new List<AppUserDto>();
            foreach (var user in users)
            {
                if (user.DeletedStatus != true)
                    resultList.Add(user);
            }

            var result = new PageResult<AppUserDto>
            {
                Count = countDetails,
                PageIndex = page ?? 1,
                PageSize = 10,
                Items = resultList.Skip((page - 1 ?? 0) * pagesize).Take(pagesize).ToList()
            };
            return result;
        }


        [HttpGet]
        //[Authorize(Policy = "RequireAdministratorRole")]
        [Route("users/newpagination")]
        public async Task<IActionResult> UserIndex(string name, int page = 1,
            SortState sortOrder = SortState.NameAsc, bool enableDeleted = true, bool enableLocked=true)
        {
            int pageSize = 3;

            //Filtration
            List<AppUserDto> users = new List<AppUserDto>(await _appUserService.GetAllUsersAsync());
            //users = null;
            if (users == null)
                return NoContent();
                //return null;
           
            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(p => p.UserName.Contains(name)).ToList();
            }

            // Sorting
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case SortState.EmailAsc:
                    users = users.OrderBy(s => s.Email).ToList();
                    break;
                case SortState.EmailDesc:
                    users = users.OrderByDescending(s => s.Email).ToList();
                    break;
                case SortState.IdAsc:
                    users = users.OrderBy(s => s.Id).ToList();
                    break;
                case SortState.IdDesc:
                    users = users.OrderByDescending(s => s.Id).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }

            if (!enableDeleted)
                users = users.Where(s => s.DeletedStatus == false).ToList();

            if (!enableLocked)
                users = users.Where(s => s.LockoutEnd == null).ToList();

            // Pagination
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            UserIndexViewModel viewModel = new UserIndexViewModel
            {
                PageViewModel = new UserPageViewModel(count, page, pageSize),
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

            //Filtration
            List<HistoryDto> histories = new List<HistoryDto>(await _historyService.GetAllAsync());
            //users = null;
            if (histories == null)
                return NoContent();
            //return null;

            if (!String.IsNullOrEmpty(name))
            {
                histories = histories.Where(p => p.NewUserName.Contains(name)).ToList();
            }

            // Sorting
            switch (sortOrder)
            {
                case HistorySortState.NameDesc:
                    histories = histories.OrderByDescending(s => s.NewUserName).ToList();
                    break;
                case HistorySortState.EmailAsc:
                    histories = histories.OrderBy(s => s.NewUserEmail).ToList();
                    break;
                case HistorySortState.EmailDesc:
                    histories = histories.OrderByDescending(s => s.NewUserEmail).ToList();
                    break;
                case HistorySortState.IdAsc:
                    histories = histories.OrderBy(s => s.Id).ToList();
                    break;
                case HistorySortState.IdDesc:
                    histories = histories.OrderByDescending(s => s.Id).ToList();
                    break;
                case HistorySortState.UserIdAsc:
                    histories = histories.OrderBy(s => s.AppUserId).ToList();
                    break;
                case HistorySortState.UserIdDesc:
                    histories = histories.OrderByDescending(s => s.AppUserId).ToList();
                    break;
                case HistorySortState.AdminIdAsc:
                    histories = histories.OrderBy(s => s.AdminId).ToList();
                    break;
                case HistorySortState.AdminIdDesc:
                    histories = histories.OrderByDescending(s => s.AdminId).ToList();
                    break;
                case HistorySortState.DateAsc:
                    histories = histories.OrderBy(s => s.DateOfOperation).ToList();
                    break;
                case HistorySortState.NameAsc:
                    histories = histories.OrderBy(s => s.NewUserName).ToList();
                    break;  
                default:
                    histories = histories.OrderByDescending(s => s.DateOfOperation).ToList();
                    break;
            }


            // Pagination
            var count = histories.Count();
            var items = histories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // representation model
            HistoryIndexViewModel viewModel = new HistoryIndexViewModel
            {
                PageViewModel = new UserPageViewModel(count, page, pageSize),
                SortViewModel = new HistorySortViewModel(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Histories = items
            };
            return Ok(viewModel);
        }

    }
}
