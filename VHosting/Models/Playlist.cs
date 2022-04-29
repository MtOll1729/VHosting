using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class Playlist
    {
        public Playlist()
        {
            Users = new HashSet<User>();
            Videos = new HashSet<Video>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? UserId { get; set; }
        public byte[]? CreatedAt { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}
