using ForumDeDiscussion.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumDeDiscussion.Data.Context
{
    public class ForumDeDiscussionDbContext : DbContext
    {
        public ForumDeDiscussionDbContext(DbContextOptions<ForumDeDiscussionDbContext> options) : base(options)
        {
        }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Messages)
                .WithOne(m => m.Subject)
                .HasForeignKey(m => m.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Subject)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SubjectId);
        }
    }
}
