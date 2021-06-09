using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4_Cs.Models
{
    public class Assigment1DbContext : DbContext

    {
        public Assigment1DbContext (DbContextOptions<Assigment1DbContext> options) : base(options)
        {

        }

        public DbSet<Lab4_Cs.Models.Role> Roles { get; set; }
        public DbSet<Lab4_Cs.Models.BlogPost> BlogPosts { get; set; }
        public DbSet<Lab4_Cs.Models.Comment> Comments { get; set; }
        public DbSet<Lab4_Cs.Models.User> Users { get; set; }
        public DbSet<Lab4_Cs.Models.Photo> Photos { get; set; }
    }

}
