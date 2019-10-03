using System;
using Microsoft.EntityFrameworkCore;
using static ExampleService.PersonStore;
using static ExampleService.TeamStore;
using static ExampleService.TeamMemberStore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GuidIds = StronglyTyped.GuidIds;
using IntIds = StronglyTyped.IntIds;
using LongIds = StronglyTyped.LongIds;

namespace ExampleService
{
	internal class EntityFrameworkContext : DbContext
	{
		public DbSet<PersonRecord> Person { get; set; }
		public DbSet<TeamRecord> Team { get; set; }
		public DbSet<TeamMemberRecord> TeamMember { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(ExampleService.Database.CreateConnectionString());
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PersonRecord>().ToTable("person");
			modelBuilder.Entity<PersonRecord>().HasKey(record => record.PersonId);
			modelBuilder.Entity<PersonRecord>().Property(record => record.PersonId).HasConversion(new ValueConverter<GuidIds.Id<Person>, Guid>(x => x.Value, x => new GuidIds.Id<Person>(x)));

			modelBuilder.Entity<TeamRecord>().ToTable("team");
			modelBuilder.Entity<TeamRecord>().HasKey(record => record.TeamId);
			modelBuilder.Entity<TeamRecord>().Property(record => record.TeamId).HasConversion(new ValueConverter<IntIds.Id<Team>, int>(x => x.Value, x => new IntIds.Id<Team>(x)));

			modelBuilder.Entity<TeamMemberRecord>().ToTable("team_member");
			modelBuilder.Entity<TeamMemberRecord>().HasKey(record => record.TeamMemberId);
			modelBuilder.Entity<TeamMemberRecord>().Property(record => record.TeamMemberId).HasConversion(new ValueConverter<LongIds.Id<TeamMember>, long>(x => x.Value, x => new LongIds.Id<TeamMember>(x)));
			modelBuilder.Entity<TeamMemberRecord>().Property(record => record.TeamId).HasConversion(new ValueConverter<IntIds.Id<Team>, int>(x => x.Value, x => new IntIds.Id<Team>(x)));
			modelBuilder.Entity<TeamMemberRecord>().Property(record => record.PersonId).HasConversion(new ValueConverter<GuidIds.Id<Person>, Guid>(x => x.Value, x => new GuidIds.Id<Person>(x)));
		}
	}
}
