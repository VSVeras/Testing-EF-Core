using System.Collections.Generic;

namespace EntityFrameworkCore
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Text { get; set; }

        public ICollection<PostTag> PostTags { get; } = new List<PostTag>();
    }
}
