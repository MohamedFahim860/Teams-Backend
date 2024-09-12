using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;

namespace Teams.Persistence.EntityConfiguration
{
    internal class UserChannelEntityTypeConfiguration : IEntityTypeConfiguration<UserChannel>
    {
        public void Configure(EntityTypeBuilder<UserChannel> builder) 
        {
            builder.HasKey(uc => uc.Id);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserChannel)
                .HasForeignKey(uc => uc.UserId);

            builder.HasOne(uc => uc.Channel)
                .WithMany(c => c.UserChannel)
                .HasForeignKey(uc => uc.ChannelId);
        }
    }
}
