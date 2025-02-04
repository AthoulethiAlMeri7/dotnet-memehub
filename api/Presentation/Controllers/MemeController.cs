using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeController : ControllerBase
    {
        private readonly IValidator<CreateMemeDto> _validator;
        private readonly IMemeService _memeService;
        private readonly IUserService _userService;
        private readonly ITextBlockService _textBlockService;
        public MemeController(IValidator<CreateMemeDto> validator, IMemeService memeService, IUserService userService, ITextBlockService textBlockService)
        {
            _validator = validator;
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

        private async Task<UserDto> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            return user;
        }

        private async Task<List<TextBlockDto>> CreateTextBlocks(List<CreateTextBlockDto> textBlocks, Guid memeId)
        {
            var createdTextBlocks = new List<TextBlockDto>();

            foreach (var textBlock in textBlocks)
            {
                textBlock.MemeId = memeId;
                var createdTextBlock = await _textBlockService.CreateTextBlockAsync(textBlock);
                if (createdTextBlock != null)
                {
                    createdTextBlocks.Add(createdTextBlock);
                }
            }

            return createdTextBlocks;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeme([FromBody] CreateMemeRequestDto memeRequestDto)
        {
            try
            {
                var user = await this.GetCurrentUser();
                if (user == null) return Unauthorized();

                var validationResult = await _validator.ValidateAsync(memeRequestDto.Meme);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                var createdMeme = await _memeService.CreateMemeAsync(user, memeRequestDto.Meme);
                if (createdMeme == null || createdMeme.Id == Guid.Empty) return BadRequest(new { message = "Failed to add meme to the database" });

                createdMeme.TextBlocks = await CreateTextBlocks(memeRequestDto.TextBlocks, createdMeme.Id);

                return Ok(createdMeme);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeme(Guid id, [FromBody] UpdateMemeRequestDto memeDto)
        {
            try
            {
                var updatedMeme = await _memeService.UpdateMemeAsync(id, memeDto.Meme);
                var memeTextBlocks = await _textBlockService.GetTextBlocksByMemeIdAsync(id);
                foreach (var textBlock in memeTextBlocks)
                {
                    await _textBlockService.DeleteTextBlockAsync(textBlock.Id);
                }
                var newTextBlocks = memeDto.TextBlocks;
                var meme = await _memeService.GetMemeByIdAsync(id);
                var memeNewTextBlocks = await CreateTextBlocks(newTextBlocks, id);

                updatedMeme.TextBlocks = memeNewTextBlocks;

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
                var meme = await _memeService.GetMemeByIdAsync(id);
                if (meme == null) return NotFound(new { message = "Meme not found" });

                await _memeService.DeleteMemeAsync(id);
                return Ok(new { message = "Meme deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}