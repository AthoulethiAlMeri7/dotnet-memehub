using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTemplates([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var templates = await _templateService.GetAllTemplatesAsync(pageNumber, pageSize);
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemplateById(Guid id)
        {
            try
            {
                var template = await _templateService.GetTemplateByIdAsync(id);
                return Ok(template);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles ="ROLE_ADMIN")]

        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateDto templateDto)
        {
            try
            {
                var createdTemplate = await _templateService.CreateTemplateAsync(templateDto);
                return Ok(createdTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="ROLE_ADMIN")]

        public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateTemplateDto templateDto)
        {
            try
            {
                var updatedTemplate = await _templateService.UpdateTemplateAsync(id, templateDto);
                return Ok(updatedTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="ROLE_ADMIN")]

        public async Task<IActionResult> DeleteTemplate(Guid id)
        {
            try
            {
                await _templateService.DeleteTemplateAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}