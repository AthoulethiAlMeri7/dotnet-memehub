using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Dtos.UserDtos
{
    public class UploadProfilePictureDto
    {
        public required IFormFile ProfilePicture { get; set; }

    }
}