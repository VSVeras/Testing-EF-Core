using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCore
{
    public class Program
    {
        //https://docs.microsoft.com/en-us/ef/core/
        //https://docs.microsoft.com/pt-br/ef/core/managing-schemas/migrations/index#create-a-migration
        //https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-1-the-basics/

        public static void Main()
        {
            //using (var context = new BloggingContext())
            //{
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();

            //    var tags = new[]
            //    {
            //        new Tag { Text = "Golden" },
            //        new Tag { Text = "Pineapple" },
            //        new Tag { Text = "Girlscout" },
            //        new Tag { Text = "Cookies" }
            //    };

            //    var posts = new[]
            //    {
            //        new Post { Title = "Best Boutiques on the Eastside" },
            //        new Post { Title = "Avoiding over-priced Hipster joints" },
            //        new Post { Title = "Where to buy Mars Bars" }
            //    };

            //    context.AddRange(
            //        new PostTag { Post = posts[0], Tag = tags[0] },
            //        new PostTag { Post = posts[0], Tag = tags[1] },
            //        new PostTag { Post = posts[1], Tag = tags[2] },
            //        new PostTag { Post = posts[1], Tag = tags[3] },
            //        new PostTag { Post = posts[2], Tag = tags[0] },
            //        new PostTag { Post = posts[2], Tag = tags[1] },
            //        new PostTag { Post = posts[2], Tag = tags[2] },
            //        new PostTag { Post = posts[2], Tag = tags[3] });

            //    context.SaveChanges();

            //}

            //using (var context = new BloggingContext())
            //{
            //    var posts = LoadAndDisplayPosts(context, "as added");

            //    posts.Add(context.Add(new Post { Title = "Going to Red Robin" }).Entity);

            //    var newTag1 = new Tag { Text = "Sweet" };
            //    var newTag2 = new Tag { Text = "Buzz" };

            //    foreach (var post in posts)
            //    {
            //        var oldPostTag = post.PostTags.FirstOrDefault(e => e.Tag.Text == "Pineapple");
            //        if (oldPostTag != null)
            //        {
            //            post.PostTags.Remove(oldPostTag);
            //            post.PostTags.Add(new PostTag { Post = post, Tag = newTag1 });
            //        }
            //        post.PostTags.Add(new PostTag { Post = post, Tag = newTag2 });
            //    }

            //    context.SaveChanges();
            //}

            //using (var context = new BloggingContext())
            //{
            //    LoadAndDisplayPosts(context, "after manipulation");
            //}

            using (var context = new BloggingContext())
            {
                //Não chame EnsureCreated() antes de Migrate(). 
                //O EnsureCreated() ignora as Migrações para criar o esquema e causa falha no Migrate().

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var tag = new Tag { Text = "Primeira tag" };
                context.Add(tag);

                var post = new Post { Title = "Primeiro post" };
                var postTag = new PostTag { Post = post, Tag = tag };
                context.Add(postTag);

                post = new Post { Title = "Segundo post" };
                postTag = new PostTag { Post = post, Tag = tag };
                context.Add(postTag);

                context.SaveChanges();

                Console.WriteLine("Consultar no banco - pressione ENTER");
                Console.ReadKey();

                var posts = context.Posts.Include(e => e.PostTags).ThenInclude(e => e.Tag).FirstOrDefault(e => e.PostId == 1);
                foreach (var onePost in posts.PostTags)
                {
                    Console.WriteLine($"    {onePost.Tag.Text}");
                }

                var tags = context.Tags.Include(e => e.PostTags).ThenInclude(e => e.Post).Where(x => x.TagId == 1).ToList();
                foreach (var oneTag in tags)
                {
                    var oldPostTag = oneTag.PostTags.Where(e => e.Tag.TagId == oneTag.TagId).ToList();
                    if (oldPostTag != null)
                    {
                        context.RemoveRange(oldPostTag);
                    }
                }

                context.SaveChanges();
            }

            Console.WriteLine("Finalizado");
        }

        private static List<Post> LoadAndDisplayPosts(BloggingContext context, string message)
        {
            Console.WriteLine($"Dumping posts {message}:");

            var posts = context.Posts
                .Include(e => e.PostTags)
                .ThenInclude(e => e.Tag)
                .ToList();

            foreach (var post in posts)
            {
                Console.WriteLine($"  Post {post.Title}");
                foreach (var tag in post.PostTags.Select(e => e.Tag))
                {
                    Console.WriteLine($"    Tag {tag.Text}");
                }
            }

            return posts;
        }
    }
}
