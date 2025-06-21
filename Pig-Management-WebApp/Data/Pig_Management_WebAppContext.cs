using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pig_Management_WebApp.Models;

namespace Pig_Management_WebApp.Data
{
    public class Pig_Management_WebAppContext : DbContext
    {
        public Pig_Management_WebAppContext (DbContextOptions<Pig_Management_WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<Pig_Management_WebApp.Models.Races_Porcs> Races_Porcs { get; set; } = default!;
    }
}
