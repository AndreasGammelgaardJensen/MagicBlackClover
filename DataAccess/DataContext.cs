using MagicScannerLib.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

			/*
             * dotnet ef migrations add InitialCreate
             * dotnet ef database update

             */
			//used when running EF updates
			//optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=TestDB;User ID=SA;Password=And12345;TrustServerCertificate=True;");
			//optionsBuilder.UseSqlServer(@"Server=localhost.,1433;Database=InstitutionTestDb;Trusted_Connection=True;");
			//optionsBuilder.UseSqlServer(@"Data Source=InstitutionDB,1433;Initial Catalog=InstitutionDB;User ID=SA;Password=And12345;TrustServerCertificate=True;", options => options.EnableRetryOnFailure());
			//optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=Test2;User ID=SA;Password=And12345;TrustServerCertificate=True;");
			//optionsBuilder.UseSqlite(@"Data Source=store.db");
			optionsBuilder.UseSqlServer(@"Server=localhost;Database=MagicScannerDB;Trusted_Connection=True;TrustServerCertificate=True;");
			//optionsBuilder.UseSqlServer(@"Data Source=InstitutionDB,1433;Initial Catalog=InstitutionDB;User ID=SA;Password=And12345;TrustServerCertificate=True;", options => options.EnableRetryOnFailure());


		}

		public IQueryable<TDatabaseModel> Get<TDatabaseModel>() where TDatabaseModel : class
		{
			return base.Set<TDatabaseModel>().AsNoTracking().AsQueryable();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CollectionDatabaseModel>()
				.HasKey(a => a.Id);

			modelBuilder.Entity<ScanDatabaseModel>()
				.HasKey(a => a.Id);

			modelBuilder.Entity<ScanDatabaseModel>()
				.HasOne(s => s.Collection)  // One ScanDatabaseModel has one Collection
				.WithMany()                 // No navigation from Collection back to ScanDatabaseModel
				.HasForeignKey(s => s.CollectionId) // Foreign key in ScanDatabaseModel
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ScanItemsDatabaseModel>()
				.HasKey(a => a.Id);

			modelBuilder.Entity<CardDatabaseModel>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<CardDatabaseModel>()
				.HasMany(c => c.ForeignNames)
				.WithOne(f => f.Card)
				.HasForeignKey(f => f.CardId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CardDatabaseModel>()
				.HasMany(c => c.Legalities)
				.WithOne(l => l.Card)
				.HasForeignKey(l => l.CardId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CardDatabaseModel>()
				.HasMany(c => c.Printings)
				.WithOne(p => p.Card)
				.HasForeignKey(p => p.CardId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ForeignNameDatabaseModel>()
				.HasKey(f => f.Id);

			modelBuilder.Entity<LegalityDatabaseModel>()
				.HasKey(l => l.Id);

			modelBuilder.Entity<PrintingDatabaseModel>()
				.HasKey(p => p.Id);

			modelBuilder.Entity<CollectionCardsDatabaseModel>()
				.HasKey(cc => new { cc.CollectionId, cc.CardId });

			modelBuilder.Entity<CollectionCardsDatabaseModel>()
				.HasOne(cc => cc.Collection)
				.WithMany()
				.HasForeignKey(cc => cc.CollectionId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CollectionCardsDatabaseModel>()
				.HasOne(cc => cc.Card)
				.WithMany()
				.HasForeignKey(cc => cc.CardId)
				.OnDelete(DeleteBehavior.Cascade);
		}

		public DbSet<CollectionDatabaseModel> Collections => Set<CollectionDatabaseModel>();
		public DbSet<ScanDatabaseModel> Scans => Set<ScanDatabaseModel>();
		public DbSet<ScanItemsDatabaseModel> ScanItems => Set<ScanItemsDatabaseModel>();
		public DbSet<CardDatabaseModel> Cards => Set<CardDatabaseModel>();
		public DbSet<ForeignNameDatabaseModel> ForeignNames => Set<ForeignNameDatabaseModel>();
		public DbSet<LegalityDatabaseModel> Legalities => Set<LegalityDatabaseModel>();
		public DbSet<PrintingDatabaseModel> Printings => Set<PrintingDatabaseModel>();
		public DbSet<CollectionCardsDatabaseModel> CollectionCards => Set<CollectionCardsDatabaseModel>();


	}
}
