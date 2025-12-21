using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class StationDBContext : DbContext
    {
        public StationDBContext (DbContextOptions<StationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Api.Models.Station> Station { get; set; } = default!;
    }
}
