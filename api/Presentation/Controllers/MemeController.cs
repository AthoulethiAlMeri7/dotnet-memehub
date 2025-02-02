using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeController : ControllerBase
    {
        private readonly IMemeService _memeService;
        public MemeController(IMemeService memeService)
        {
            _memeService = memeService;
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

        [HttpPost]
        public async Task<IActionResult> CreateMeme([FromBody] CreateMemeDto memeDto)
        {
            try
            {
                var createdMeme = await _memeService.CreateMemeAsync(memeDto);
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