using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Domain.Models
{
    public class RevokedToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Token { get; set; }
        public DateTime RevokedAt { get; set; }

    }
}