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
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AdminsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemeService _memeService;


        public AdminsController(IUserService userService, IMemeService memeService)
        {
            _userService = userService;
            _memeService = memeService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAdmins([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var admins = await _userService.GetAllAdminsAsync(pageNumber, pageSize);
            return Ok(admins);
        }


        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
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
        public async Task<ActionResult> GetUsersByRole(string role, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role, pageNumber, pageSize);
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
            try
            {
                var user = await _userService.DeleteUserAsync(id);
                if (user == null) return NotFound("User not found");

                var totalPages = (await _memeService.GetMemesByUserIdAsync(id, 1, 1)).TotalRecords;
                var memes = (await _memeService.GetMemesByUserIdAsync(id, 1, totalPages)).Items;

                foreach (var meme in memes)
                {
                    await _memeService.DeleteMemeAsync(meme.Id);
                }

                return NoContent();
            }
            catch
            {
                return BadRequest("Unable to delete user");
            }
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

        [HttpDelete("{id}/remove-role")]
        public async Task<ActionResult> RemoveRole(Guid id, [FromBody] string role)
        {
            var result = await _userService.RemoveRoleAsync(id, role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpGet("users-count")]
        public async Task<ActionResult<int>> GetUsersCount()
        {
            var count = await _userService.GetUsersCountAsync();
            return Ok(count);
        }

        [HttpGet("admins-count")]
        public async Task<ActionResult<int>> GetAdminsCount()
        {
            var count = await _userService.GetAdminsCountAsync();
            return Ok(count);
        }
    }
}