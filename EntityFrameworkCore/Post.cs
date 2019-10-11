using System.Collections.Generic;

namespace EntityFrameworkCore
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        public ICollection<PostTag> PostTags { get; } = new List<PostTag>();
    }
}
