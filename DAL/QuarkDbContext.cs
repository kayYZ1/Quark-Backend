using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.Entities;

namespace Quark_Backend.DAL;

public partial class QuarkDbContext : DbContext
{
    public QuarkDbContext()
    {
    }

    public QuarkDbContext(DbContextOptions<QuarkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements {get; set;}

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<JobPosition> JobPositions { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersConversation> UsersConversations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=quark_db;Username=user;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("announcements_pkey");

            entity.ToTable("announcements");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(30);
            entity.Property(e => e.Content).HasColumnName("content").HasMaxLength(150);
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("announcements_user_id_fkey");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conversations_pkey");

            entity.ToTable("conversations");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.HasMany(e => e.Users).WithMany(e => e.Conversations)
                .UsingEntity<UsersConversation>();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_positions_pkey");

            entity.ToTable("job_positions");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.HasOne(d => d.Department).WithMany(p => p.JobPositions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_positions_department_id_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("messages_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.SentDate).HasColumnName("sent_date");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("messages_conversation_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("messages_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");
            entity.HasIndex(e => e.Username)
                .IsUnique(true);
            entity.HasIndex(e => e.Email)
                .IsUnique(true);

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasColumnName("password");
            entity.Property(e => e.PermissionLevel).HasColumnName("permission_level");
            entity.Property(e => e.SelfDescription)
                .HasMaxLength(300)
                .HasColumnName("self_description");
            entity.Property(e => e.PictureUrl)
                .HasMaxLength(100)
                .HasColumnName("picture_url");
            entity.HasOne(d => d.JobPosition).WithMany(p => p.Users)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("users_job_positions_id_fkey");
        });

        modelBuilder.Entity<UsersConversation>(entity =>
        {
            entity.HasKey(e => new {e.UsersId, e.ConversationsId});
            entity.ToTable("users_conversations");
            entity.Property(e => e.ConversationsId)
                .HasColumnName("conversations_id");
            entity.Property(e => e.UsersId)
                .HasColumnName("users_id");

            entity.HasOne(d => d.Conversations).WithMany()
                .HasForeignKey(d => d.ConversationsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_conversations_conversations_id_fkey");

            entity.HasOne(d => d.Users).WithMany()
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_conversations_users_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
