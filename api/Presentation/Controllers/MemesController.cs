using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemeController : ControllerBase
    {
        private readonly IMemeRepository _memeRepository;

        public MemeController(IMemeRepository memeRepository)
        {
            _memeRepository = memeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meme>>> GetAllMemes()
        {
            var memes = await _memeRepository.GetAllAsync();
            return Ok(memes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Meme>> GetMemeById(Guid id)
        {
            var meme = await _memeRepository.GetByIdAsync(id);
            if (meme == null)
            {
                return NotFound();
            }
            return Ok(meme);
        }

        [HttpPost]
        public async Task<ActionResult<Meme>> AddMeme([FromBody] Meme meme)
        {
            var createdMeme = await _memeRepository.AddAsync(meme);
            return CreatedAtAction(nameof(GetMemeById), new { id = createdMeme.Id }, createdMeme);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Meme>> UpdateMeme(Guid id, [FromBody] Meme meme)
        {
            if (id != meme.Id)
            {
                return BadRequest();
            }

            var updatedMeme = await _memeRepository.UpdateAsync(meme);
            if (updatedMeme == null)
            {
                return NotFound();
            }

            return Ok(updatedMeme);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeme(Guid id)
        {
            var meme = await _memeRepository.GetByIdAsync(id);
            if (meme == null)
            {
                return NotFound();
            }

            await _memeRepository.DeleteAsync(meme);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Meme>>> GetMemesByUser(Guid userId)
        {
            var memes = await _memeRepository.GetByUserAsync(userId);
            return Ok(memes);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Meme>>> GetMemesByDate(DateTime date)
        {
            var memes = await _memeRepository.GetByDateAsync(date);
            return Ok(memes);
        }
    }
}