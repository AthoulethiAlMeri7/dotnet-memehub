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
    [Authorize(Roles = "ROLE_USER,ROLE_ADMIN")]
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
            try
            {

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> SearchUsers(string search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(search, pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedUserDto>> UpdateUser([FromForm] UpdateUserDto updateUserDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null) return Unauthorized();
            var result = await _userService.UpdateUserAsync(user.Id, updateUserDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var updatedUser = await _userService.GetUserByIdAsync(user.Id);
            return Ok(updatedUser);
        }


        [HttpPost("upload-profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadProfilePicture(UploadProfilePictureDto uploadProfilePictureDto)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null) return Unauthorized();

                var profilePicUrl = await _userService.UploadProfilePictureAsync(user.Id, uploadProfilePictureDto);
                return Ok(profilePicUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [Authorize(Roles = "ROLE_GUEST")]
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(Guid userId)
        {
            try
            {
                var result = await _userService.VerifyEmailAsync(userId);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return Ok("Email verified successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}