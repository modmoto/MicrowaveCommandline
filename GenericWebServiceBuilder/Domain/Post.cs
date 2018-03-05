using System;

namespace Domain
{
    public partial class Post
    {
        public Post(String title)
        {
            if (title.Length <= 5)
            {

            }

            throw new Exception();
        }
    }
}