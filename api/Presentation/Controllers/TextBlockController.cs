using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ROLE_USER")]

    public class TextBlockController : ControllerBase
    {
        private readonly ITextBlockService _textBlockService;
        public TextBlockController(ITextBlockService textBlockService)
        {
            _textBlockService = textBlockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTextBlocks()
        {
            try
            {
                var textBlocks = await _textBlockService.GetAllTextBlocksAsync();
                return Ok(textBlocks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTextBlockById(Guid id)
        {
            try
            {
                var textBlock = await _textBlockService.GetTextBlockByIdAsync(id);
                return Ok(textBlock);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTextBlock([FromBody] CreateTextBlockDto textBlockDto)
        {
            try
            {
                var createdTextBlock = await _textBlockService.CreateTextBlockAsync(textBlockDto);
                return Ok(createdTextBlock);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTextBlock(Guid id, [FromBody] UpdateTextBlockDto textBlockDto)
        {
            try
            {
                var updatedTextBlock = await _textBlockService.UpdateTextBlockAsync(id, textBlockDto);
                return Ok(updatedTextBlock);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTextBlock(Guid id)
        {
            try
            {
                await _textBlockService.DeleteTextBlockAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}