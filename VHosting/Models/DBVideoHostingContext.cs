using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VHosting
{
    public partial class DBVideoHostingContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DBVideoHostingContext()
        {
        }

        public DBVideoHostingContext(DbContextOptions<DBVideoHostingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<PaymentReceipt> PaymentReceipts { get; set; } = null!;
        public virtual DbSet<Playlist> Playlists { get; set; } = null!;
        public virtual DbSet<SettingsOption> SettingsOptions { get; set; } = null!;
        public virtual DbSet<UserSetting> UserSettings { get; set; } = null!;
        public virtual DbSet<Video> Videos { get; set; } = null!;
        public virtual DbSet<WatchedVideo> WatchedVideos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-O1SJKGD; Database=DBVideoHosting; Trusted_Connection=True; MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Permissions).HasColumnName("permissions");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VideoId).HasColumnName("video_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_user");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_video");
            });

            modelBuilder.Entity<PaymentReceipt>(entity =>
            {
                entity.ToTable("payment_receipt");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ReceiptLink).HasColumnName("receipt_link");

                entity.Property(e => e.SubscriptionType).HasColumnName("subscription_type");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentReceipts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payment_receipt_user");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("playlist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_playlist_user");

                entity.HasMany(d => d.Videos)
                    .WithMany(p => p.Playlists)
                    .UsingEntity<Dictionary<string, object>>(
                        "PlaylistVideo",
                        l => l.HasOne<Video>().WithMany().HasForeignKey("VideoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_playlist_video_video"),
                        r => r.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_playlist_video_playlist"),
                        j =>
                        {
                            j.HasKey("PlaylistId", "VideoId");

                            j.ToTable("playlist_video");

                            j.IndexerProperty<int>("PlaylistId").HasColumnName("playlist_id");

                            j.IndexerProperty<int>("VideoId").HasColumnName("video_id");
                        });
            });

            modelBuilder.Entity<SettingsOption>(entity =>
            {
                entity.HasKey(e => new { e.SettingsId, e.OptionId });

                entity.ToTable("settings_option");

                entity.Property(e => e.SettingsId).HasColumnName("settings_id");

                entity.Property(e => e.OptionId).HasColumnName("option_id");

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .HasColumnName("value");

                entity.HasOne(d => d.Settings)
                    .WithMany(p => p.SettingsOptions)
                    .HasPrincipalKey(p => p.SettingsId)
                    .HasForeignKey(d => d.SettingsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_settings_option_user_settings");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "UK_email")
                    .IsUnique();

                entity.HasIndex(e => e.Nickname, "UK_nickname")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountType).HasColumnName("account_type");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.IsVerified).HasColumnName("is_verified");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.PaymentCard)
                    .HasMaxLength(50)
                    .HasColumnName("payment_card");

                entity.HasOne(d => d.AccountTypeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AccountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_account");

                entity.HasMany(d => d.PlaylistsNavigation)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserPlaylist",
                        l => l.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_user_playlist_playlist"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_user_playlist_user"),
                        j =>
                        {
                            j.HasKey("UserId", "PlaylistId");

                            j.ToTable("user_playlist");

                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");

                            j.IndexerProperty<int>("PlaylistId").HasColumnName("playlist_id");
                        });

                entity.HasMany(d => d.Subsribers)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "SubscriberUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("SubsriberId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_subscriber_user_user1"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_subscriber_user_user"),
                        j =>
                        {
                            j.HasKey("UserId", "SubsriberId").HasName("PK_subscriber_user_1");

                            j.ToTable("subscriber_user");

                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");

                            j.IndexerProperty<int>("SubsriberId").HasColumnName("subsriber_id");
                        });

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Subsribers)
                    .UsingEntity<Dictionary<string, object>>(
                        "SubscriberUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_subscriber_user_user"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("SubsriberId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_subscriber_user_user1"),
                        j =>
                        {
                            j.HasKey("UserId", "SubsriberId").HasName("PK_subscriber_user_1");

                            j.ToTable("subscriber_user");

                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");

                            j.IndexerProperty<int>("SubsriberId").HasColumnName("subsriber_id");
                        });
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("user_settings");

                entity.HasIndex(e => e.SettingsId, "UN_settings")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.SettingsId).HasColumnName("settings_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserSetting)
                    .HasForeignKey<UserSetting>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_settings_user");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("video");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Dislikes).HasColumnName("dislikes");

                entity.Property(e => e.IsMonetized).HasColumnName("is_monetized");

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.Link).HasColumnName("link");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Videos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_video_user");
            });

            modelBuilder.Entity<WatchedVideo>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.VideoId });

                entity.ToTable("watched_video");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VideoId).HasColumnName("video_id");

                entity.Property(e => e.IsDisliked).HasColumnName("is_disliked");

                entity.Property(e => e.IsLiked).HasColumnName("is_liked");

                entity.Property(e => e.WatchedTime).HasColumnName("watched_time");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WatchedVideos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_watched_video_user");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.WatchedVideos)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_watched_video_video");
            });

            OnModelCreatingPartial(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
