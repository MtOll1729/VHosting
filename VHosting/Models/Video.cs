using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class Video
    {
        public Video()
        {
            Comments = new HashSet<Comment>();
            WatchedVideos = new HashSet<WatchedVideo>();
            Playlists = new HashSet<Playlist>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Link { get; set; } = null!;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool IsMonetized { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<WatchedVideo> WatchedVideos { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
