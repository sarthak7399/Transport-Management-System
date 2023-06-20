using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp.Models
{
    public class Location
    {
        [Key]
        public int LID { get; set; }

        public string Longitude { get; set; } = "0";
        public string Latitude { get; set; } = "0";
        public int BID { get; set; } = 0;
        public DateTime? LastUpdateTime { get; set; } = DateTime.Now;

    }
}
