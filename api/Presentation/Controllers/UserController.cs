using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Application.Services.ServiceContracts;
using api.Application.Dtos.UserDtos;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{id}/memes")]

        public async Task<ActionResult<ApplicationUser>> GetUserByIdWithMemes(Guid id)
        {
            var user = await _userService.GetUserByIdWithMemesAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(userDto);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || id != user.Id)
            {
                return BadRequest();
            }

            var result = await _userService.UpdateUserAsync(id,updateUserDto);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("{id}/roles")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult> AddRole(Guid id, [FromQuery] string role)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.AddRoleAsync(id, role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}