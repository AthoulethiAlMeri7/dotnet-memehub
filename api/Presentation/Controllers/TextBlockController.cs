using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextBlockController : ControllerBase
    {
        private readonly ITextBlockRepository _textBlockRepository;

        public TextBlockController(ITextBlockRepository textBlockRepository)
        {
            _textBlockRepository = textBlockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextBlock>>> GetAllTextBlocks()
        {
            var textBlocks = await _textBlockRepository.GetAllAsync();
            return Ok(textBlocks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextBlock>> GetTextBlockById(Guid id)
        {
            var textBlock = await _textBlockRepository.GetByIdAsync(id);
            if (textBlock == null)
            {
                return NotFound();
            }
            return Ok(textBlock);
        }

        [HttpPost]
        public async Task<ActionResult<TextBlock>> AddTextBlock([FromBody] TextBlock textBlock)
        {
            var createdTextBlock = await _textBlockRepository.AddAsync(textBlock);
            return CreatedAtAction(nameof(GetTextBlockById), new { id = createdTextBlock.Id }, createdTextBlock);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TextBlock>> UpdateTextBlock(Guid id, [FromBody] TextBlock textBlock)
        {
            if (id != textBlock.Id)
            {
                return BadRequest();
            }

            var updatedTextBlock = await _textBlockRepository.UpdateAsync(textBlock);
            if (updatedTextBlock == null)
            {
                return NotFound();
            }

            return Ok(updatedTextBlock);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTextBlock(Guid id)
        {
            var textBlock = await _textBlockRepository.GetByIdAsync(id);
            if (textBlock == null)
            {
                return NotFound();
            }

            await _textBlockRepository.DeleteAsync(textBlock);
            return NoContent();
        }
    }
}