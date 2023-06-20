using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp.Models
{
    public class Notice
    {
        [Key]
        public int NID { get; set; }

        public DateTime UpdateTime { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(500,ErrorMessage = "Max Length 500 charecters")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
