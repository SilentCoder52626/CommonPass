using DomainModule.Entity;
using DomainModule.Entity.Pass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureModule.Mapping
{
    public class AccountDetailsEntityMapping : IEntityTypeConfiguration<AccountDetails>
    {
        public void Configure(EntityTypeBuilder<AccountDetails> builder)
        {
            builder
              .HasKey(a => a.Id);

            builder
                  .Property(a => a.Id)
                  .HasColumnName("id");
            builder
                  .Property(a => a.Account)
                  .HasColumnName("account")
                  .IsRequired();
            builder
                .Property(a => a.Pass)
                .HasColumnName("password")
                .IsRequired();
            builder
                .Property(a => a.Name)
                .HasColumnName("username")
                .IsRequired();

            builder
                    .Property(a => a.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();
            builder
                .HasOne(a => a.User)
                .WithMany(a => a.AccountDetails)
                .HasForeignKey(a => a.UserId);

        }
    }
}
