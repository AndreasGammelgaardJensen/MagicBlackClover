using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
	{


		public DataContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			var connectionstring = @"Server=localhost;Database=MagicScannerDB;Trusted_Connection=True;TrustServerCertificate=True;";
			optionsBuilder.UseSqlServer(connectionstring,b=> b.MigrationsAssembly("DataAccess"));
			optionsBuilder.EnableSensitiveDataLogging(false);


			return new DataContext(optionsBuilder.Options);
		}
		//Generate CreateContext Method where i can add logging filter to the optionsBuilder    
	}
}
