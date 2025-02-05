using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;
using api.Application.Services.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnedUserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> SearchUsers(string search)
        {
            var users = await _userService.SearchUsersAsync(search);
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedUserDto>> UpdateUser([FromForm] UpdateUserDto updateUserDto)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var result = await _userService.UpdateUserAsync(currentUser.Id, updateUserDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var updatedUser = await _userService.GetUserByIdAsync(currentUser.Id);
            return Ok(updatedUser);
        }


        [HttpPost("{id}/upload-profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadProfilePicture(Guid id, UploadProfilePictureDto uploadProfilePictureDto)
        {
            var profilePicUrl = await _userService.UploadProfilePictureAsync(id, uploadProfilePictureDto);
            return Ok(profilePicUrl);
        }

    }
}