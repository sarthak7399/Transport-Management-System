using Microsoft.EntityFrameworkCore;
using Bus_Pass_Mgt_Asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Student>  Students { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Bus_Pass_Mgt_Asp.Models.Location> Locations { get; set; }
        public DbSet<Bus_Pass_Mgt_Asp.Models.Notice> Notice { get; set; }

    }
}
