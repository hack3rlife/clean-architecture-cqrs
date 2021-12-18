﻿using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace io.hack3rlife.data
{
    public static class BlogDbContextDataSeed
    {
        private static readonly List<Blog> Blogs = new List<Blog>
        {
            new Blog
            {
                BlogId = new Guid("c949db94-5195-498a-afbe-7a90031b3125"),
                BlogName = "Asp. Net Core Development & Testing"
            }
        };

        private static readonly List<Post> Posts = new List<Post>
        {
            new Post
            {
                BlogId = new Guid("c949db94-5195-498a-afbe-7a90031b3125"),
                PostName = "Asp. Net Core Clean Architecture Testing",
                PostId = new Guid("b7713508-4162-4f0b-a258-e46ae97ac40a")
            }
        };

        private static readonly List<Comment> Comments = new List<Comment>
        {
            new Comment
            {
                CommentId = new Guid("ffe18dc4-d4b9-4504-be02-886f11af7c02"),
                CommentName = "This is Awesome!!!",
                PostId = new Guid("b7713508-4162-4f0b-a258-e46ae97ac40a")
            }
        };

        public static async Task SeedSampleDataAsync(BlogDbContext context)
        {
            // Seed, if necessary
            if (!context.Blog.Any())
            {
                context.Blog.AddRange(Blogs);
                context.Post.AddRange(Posts);
                context.Comment.AddRange(Comments);
            }

            await context.SaveChangesAsync();
        }
    }
}
