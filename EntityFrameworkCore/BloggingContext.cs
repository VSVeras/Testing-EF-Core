using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore
{
    public class BloggingContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=localhost,1433;Initial Catalog=Blogging;Persist Security Info=True;User ID=sa;Pwd=abre7eses@mo;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(p => new { p.PostId, p.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(p => p.PostTags)
                .HasForeignKey(p => p.TagId);
        }
    }

}
