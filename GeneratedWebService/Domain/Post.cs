using System;

namespace Domain.Posts
{
    public partial class Post
    {
        public static CreatePostEvent Create(string title, string body)
        {
            var newGuid = Guid.NewGuid();
            return new CreatePostEvent(new Post(newGuid, title, body), newGuid);
        }
    }
}