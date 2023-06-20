using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp.Models
{
    public class Bus
    {
        [Key]
        public int BID { get; set; }

        public string BusNumber { get; set; } = "XXXXXXXXXX";
        public string VisitingAreas { get; set; } = "";
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }


        [NotMapped]
        public string MapLink { get; set; }
    }
}
