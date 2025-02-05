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
    // [Authorize]
    public class AdminsController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminsController(IUserService userService)
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

        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> GetUsersByEmail(string email)
        {
            try
            {
                var users = await _userService.GetUsersByEmailAsync(email);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<ReturnedUserDto>>> GetUsersByRole(string role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(users);
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
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("username/{userName}")]
        public async Task<ActionResult<ReturnedUserDto>> GetUserByUserName(string userName)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(userName);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ReturnedUserDto>> UpdateUser(Guid id, [FromForm] UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(id, updateUserDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var updatedUser = await _userService.GetUserByIdAsync(id);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
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
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id}/add-role")]
        public async Task<ActionResult> AddRole(Guid id, [FromBody] string role)
        {
            var result = await _userService.AddRoleAsync(id, role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
    }
}