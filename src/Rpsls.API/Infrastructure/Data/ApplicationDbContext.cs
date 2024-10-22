using Microsoft.EntityFrameworkCore;
using Rpsls.API.Entities;
using System;

namespace Rpsls.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<GameRule> GameRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Choice>(entity =>
            {
                entity.Property(p => p.Name).HasMaxLength(50);

                entity.HasData(
                    new Choice { Id = 1, Name = "Rock" },
                    new Choice { Id = 2, Name = "Paper" },
                    new Choice { Id = 3, Name = "Scissors" },
                    new Choice { Id = 4, Name = "Lizard" },
                    new Choice { Id = 5, Name = "Spock" }
                );
            });

            modelBuilder.Entity<GameRule>(entity =>
            {
                entity.HasOne(e => e.Winner)
                    .WithMany(c => c.WinningOutcomes)
                    .HasForeignKey(e => e.WinnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Loser)
                    .WithMany(c => c.LosingOutcomes)
                    .HasForeignKey(e => e.LoserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Description).HasMaxLength(255);

                entity.HasData(
                    new GameRule { Id = 1, WinnerId = 1, LoserId = 3, Description = "Rock crushes scissors." },
                    new GameRule { Id = 2, WinnerId = 1, LoserId = 4, Description = "Rock crushes lizard." },
                    new GameRule { Id = 3, WinnerId = 2, LoserId = 1, Description = "Paper covers rock." },
                    new GameRule { Id = 4, WinnerId = 2, LoserId = 5, Description = "Paper disproves Spock." },
                    new GameRule { Id = 5, WinnerId = 3, LoserId = 2, Description = "Scissors cuts paper." },
                    new GameRule { Id = 6, WinnerId = 3, LoserId = 4, Description = "Scissors decapitates lizard." },
                    new GameRule { Id = 7, WinnerId = 4, LoserId = 5, Description = "Lizard poisons Spock." },
                    new GameRule { Id = 8, WinnerId = 4, LoserId = 2, Description = "Lizard eats paper." },
                    new GameRule { Id = 9, WinnerId = 5, LoserId = 3, Description = "Spock smashes scissors." },
                    new GameRule { Id = 10, WinnerId = 5, LoserId = 1, Description = "Spock vaporizes rock." }
                );
            });
        }
    }
}
