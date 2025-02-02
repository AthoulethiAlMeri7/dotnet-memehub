using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeController : ControllerBase
    {
        private readonly IMemeService _memeService;
        private readonly IUserService _userService;
        private readonly ITextBlockService _textBlockService;
        public MemeController(IMemeService memeService, IUserService userService, ITextBlockService textBlockService)
        {
            _memeService = memeService;
            _textBlockService = textBlockService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMemes()
        {
            try
            {
                var memes = await _memeService.GetAllMemesAsync();
                return Ok(memes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemeById(Guid id)
        {
            try
            {
                var meme = await _memeService.GetMemeByIdAsync(id);
                return Ok(meme);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        public async Task<UserDto> getCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeme([FromBody] CreateMemeRequestDto memeRequestDto)
        {
            try
            {
                var user = await this.getCurrentUser();
                if (user == null)
                {
                    return Unauthorized();
                }
                var createdMeme = await _memeService.CreateMemeAsync(user, memeRequestDto.Meme);
                //lzm lenna kol textblock ncreatih wbaad liste teehom lkol l created nzidhom ll meme created
                var texBlocks = await _textBlockService.CreateTextBlockAsync(memeRequestDto.TextBlock);
                return Ok(createdMeme);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeme(Guid id, [FromBody] UpdateMemeDto memeDto)
        {
            try
            {
                var updatedMeme = await _memeService.UpdateMemeAsync(id, memeDto);
                return Ok(updatedMeme);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeme(Guid id)
        {
            try
            {
                await _memeService.DeleteMemeAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}