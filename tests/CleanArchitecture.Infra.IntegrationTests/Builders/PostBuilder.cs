using System;
using CleanArchitecture.Domain.Entities;
using LoremNET;

namespace CleanArchitecture.Infra.IntegrationTests.Builders
{
    public class PostBuilder
    {
        private Post _post;

        public PostBuilder()
        {
            _post = new Post();
        }
        public static PostBuilder Create()
        {
            return new PostBuilder();
        }

        public PostBuilder WithId(Guid guid)
        {
            _post.PostId = guid;
            return this;
        }

        public PostBuilder WithName(string name = "What's Unit Testing?'")
        {
            _post.PostName = name;
            return this;
        }

        public PostBuilder WithText(string text)
        {
            _post.Text = text;
            return this;
        }

        public PostBuilder WithBlogId(Guid guid)
        {
            _post.BlogId = guid;
            return this;
        }

        public Post Build()
        {
            return _post;
        }

        public static Post Default()
        {
            return new Post
            {
                PostId = Guid.NewGuid(),
                PostName = Lorem.Words(10),
                Text = Lorem.Sentence(100),
                BlogId = Guid.NewGuid()
            };
        }

        public PostBuilder WithPostId(Guid guid)
        {
            _post.PostId = guid;
            return this;
        }

        /// <summary>
        /// Needs to be called after setting PostId
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public PostBuilder WithComments(int n)
        {
            for (var i = 0; i < n; i++)
            {
                var newComment = new Comment
                {
                    CommentId = Guid.NewGuid(),
                    CommentName = Lorem.Words(10),
                    Email = Lorem.Email(),
                    PostId = _post.PostId
                };

                _post.Comment.Add(newComment);
            }

            return this;
        }
    }
}