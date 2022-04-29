using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class WatchedVideo
    {
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public TimeSpan WatchedTime { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;
    }
}
