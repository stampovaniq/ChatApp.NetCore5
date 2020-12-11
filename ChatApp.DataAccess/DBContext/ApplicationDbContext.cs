using System;
using System.Collections.Generic;
using System.Text;
using ChatApp.DataAccess.Models.Authentication;
using ChatApp.DataAccess.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DataAccess.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region Register Entities

        public DbSet<Message> Message { get; set; }
        public DbSet<StatusType> StatusType { get; set; }
        public DbSet<MessageReceivers> MessageReceivers { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<StatusType>().ToTable("StatusType");
            modelBuilder.Entity<MessageReceivers>().ToTable("MessageReceivers");

            modelBuilder.Entity<Message>().HasIndex(x => x.CreatedBy).IsUnique(false);
            modelBuilder.Entity<Message>().HasIndex(x => x.StatusTypeId).IsUnique(false);
            modelBuilder.Entity<MessageReceivers>().HasIndex(x => x.MessageId).IsUnique(false);
            modelBuilder.Entity<MessageReceivers>().HasIndex(x => x.ReceiverId).IsUnique(false);
            modelBuilder.Entity<MessageReceivers>().HasIndex(x => x.StatusTypeId).IsUnique(false);

            modelBuilder.Entity<ApplicationUser>().HasMany(x => x.MessageCreatedBy).WithOne(x => x.CreatedByUser).HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ApplicationUser>().HasMany(x => x.MessageReceiversReceiverId).WithOne(x => x.ReceiverIdUser).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StatusType>().HasOne(x => x.Message).WithOne(x => x.StatusType).HasForeignKey<Message>(x => x.StatusTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StatusType>().HasOne(x => x.MessageReceivers).WithOne(x => x.StatusType).HasForeignKey<MessageReceivers>(x => x.StatusTypeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageReceivers>().HasKey(bc => new {bc.MessageId, bc.ReceiverId});
            modelBuilder.Entity<MessageReceivers>().HasOne(bc => bc.ReceiverIdUser).WithMany(b => b.MessageReceiversReceiverId).HasForeignKey(bc => bc.ReceiverId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MessageReceivers>().HasOne(bc => bc.MessageIdUser).WithMany(b => b.MessageReceiversMessageId).HasForeignKey(bc => bc.MessageId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StatusType>().HasData(
                new StatusType() {StatusTypeId = 1, StatusTypeName = "Sent", StatusTypeDescription = "Message Sent"},
                new StatusType() {StatusTypeId = 2, StatusTypeName = "Received", StatusTypeDescription = "Message Received"});

            base.OnModelCreating(modelBuilder);
        }
    }
}
