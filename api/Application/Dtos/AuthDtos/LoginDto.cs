using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Dtos.AuthDtos
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

    }
}