using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;
    }
}
