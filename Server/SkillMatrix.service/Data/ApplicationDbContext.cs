using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillMatrix.Domain.Models;

namespace SkillMatrix.service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        // Seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var skills = new SkillsMaster[]
            {
                new SkillsMaster()
                {
                    Id= 1,
                    SkillName = "ANGULAR",
                    SkillStatus= 1,
                    SkillDescription = "Angular is a frond end language"
                },
                 new SkillsMaster()
                {
                    Id= 2,
                    SkillName = "DOTNET",
                    SkillStatus= 1,
                    SkillDescription = "DotNet is a back end language"
                },
                  new SkillsMaster()
                {
                    Id= 3,
                    SkillName = "REACT",
                    SkillStatus= 1,
                    SkillDescription = "React is a frond end language"
                },
                 new SkillsMaster()
                {
                    Id= 4,
                    SkillName = "NODE",
                    SkillStatus= 1,
                    SkillDescription = "Node is a back end language"
                }
            };

            var users = new Users[]
            {
                new Users()
                {
                    Id= 1,
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin@12345"))
                },
                new Users()
                {
                    Id= 2,
                    Name = "employe1",
                    Email = "employe1@gmail.com",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("12345678"))
                },
                 new Users()
                {
                    Id= 3,
                    Name = "employe2",
                    Email = "employe2@gmail.com",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("12345678")) 
                }
            };
            modelBuilder.Entity<Users>().HasData(users);
            modelBuilder.Entity<SkillsMaster>().HasData(skills);
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<SkillsMaster> SkillsMaster { get; set; }

        public DbSet<EmployeSkills> EmployeSkills { get; set; }
    }
}
