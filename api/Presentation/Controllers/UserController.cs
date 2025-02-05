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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> SearchUsers(string search)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(search);
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
            try
            {
                var profilePicUrl = await _userService.UploadProfilePictureAsync(id, uploadProfilePictureDto);
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