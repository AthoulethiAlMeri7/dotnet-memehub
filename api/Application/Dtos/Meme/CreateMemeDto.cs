using System.ComponentModel.DataAnnotations;

namespace api.Application.Dtos
{
    public class CreateMemeDto
    {
        public string? Title { get; set; }
        public Guid TemplateId { get; set; }
    }
}