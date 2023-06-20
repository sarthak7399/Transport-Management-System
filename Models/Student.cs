using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp.Models
{
    public class Student
    {
        [Key]
        public int SID { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z .]{3,50}", ErrorMessage = "Invalid UserName Entered")]
        public string StudentName { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required]
        [RegularExpression(@"[6789]{1}[0-9]{9}", ErrorMessage = "Invalid Mobile Number Entered")]
        public string Mobile { get; set; }

        public string PWD { get; set; }

        public string Address { get; set; }
        public int BID { get; set; } = 1 ;

        [NotMapped]
        public Bus Bus { get; set; }

    }
}
