using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            PaymentReceipts = new HashSet<PaymentReceipt>();
            Playlists = new HashSet<Playlist>();
            Videos = new HashSet<Video>();
            WatchedVideos = new HashSet<WatchedVideo>();
            PlaylistsNavigation = new HashSet<Playlist>();
            Subsribers = new HashSet<User>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? PaymentCard { get; set; }
        public int AccountType { get; set; }
        public bool IsVerified { get; set; }

        public virtual Account AccountTypeNavigation { get; set; } = null!;
        public virtual UserSetting UserSetting { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PaymentReceipt> PaymentReceipts { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<WatchedVideo> WatchedVideos { get; set; }

        public virtual ICollection<Playlist> PlaylistsNavigation { get; set; }
        public virtual ICollection<User> Subsribers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
