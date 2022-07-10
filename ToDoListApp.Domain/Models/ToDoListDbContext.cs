using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoListApp.Domain.Enums;

namespace ToDoListApp.Domain.Models
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options):base(options) {}

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<ToDo>()
            //    .Property(e => e.Status)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => Enum.Parse<ToDoStatus>(v));
        }
    }
}
