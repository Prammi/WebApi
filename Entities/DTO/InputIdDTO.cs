using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTO
{
   public class InputIdDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }
    }
}
