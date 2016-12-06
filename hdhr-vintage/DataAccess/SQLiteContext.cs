using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.DataAccess
{
    public class SQLiteContext : DbContext
    {
        public SQLiteContext() : base("SQLiteContext")
        {
            Database.SetInitializer<SQLiteContext>(null);
            this.Database.Connection.ConnectionString = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "hdhr-vintage.db";
        }

        public DbSet<Models.Channel> Channel { get; set; }
        public DbSet<Models.Device> Device { get; set; }
        public DbSet<Models.Program> Program { get; set; }
        public DbSet<Models.Tuner> Tuner { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //modelBuilder
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
