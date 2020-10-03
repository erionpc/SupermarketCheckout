using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Checkout.Identity.Models
{
    public class IdentityResponseDto
    {
        public string Status => Enum.GetName(typeof(AuthResult), StatusEnum);

        [JsonIgnore]
        public AuthResult StatusEnum { get; set; }

        public string Message { get; set; }

        public IdentityResponseDto()
        {

        }

        public IdentityResponseDto(AuthResult status, string message)
        {
            this.StatusEnum = status;
            this.Message = message;
        }
    }

    public enum AuthResult
    {
        Error,
        Success
    }
}
