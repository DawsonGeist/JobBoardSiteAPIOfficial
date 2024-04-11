namespace JobBoardSiteAPIOfficial.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using JobBoardSiteAPIOfficial.Models;

    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        static readonly string connectionString = "Server=localhost; User ID=root; Password=password; Database=job_board_schema";

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public bool OpenConnection()
        {
            bool success = false;
            if (Database.CanConnect())
            {
                this.Database.OpenConnection();
                success = true;
            }
            return success;
        }

        public void CloseConnection()
        {
            Database.CloseConnectionAsync().Wait();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }


}
