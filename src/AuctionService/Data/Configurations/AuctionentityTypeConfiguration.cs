using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionService.Data.Configurations;

public class AuctionentityTypeConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.Property(a => a.Status)
        .HasConversion(
            a => a.ToString(),
            a => (Status)Enum.Parse(typeof(Status), a)
        );
    }
}
