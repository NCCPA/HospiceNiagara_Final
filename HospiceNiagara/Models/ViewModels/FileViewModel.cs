using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models.ViewModels
{
    public class FileViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        public string MimeType { get; set; }

        [Required(ErrorMessage = "You cannot leave the name of the file blank.")]
        [StringLength(100, ErrorMessage = "The name of the file cannot be more than 100 characters.")]
        public string FileName { get; set; }

        [StringLength(100, ErrorMessage = "The file description cannot be more than 100 characters.")]
        public string FileDescription { get; set; }
    }
}