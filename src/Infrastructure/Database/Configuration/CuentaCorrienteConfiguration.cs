using Continental.API.Core.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Continental.API.Infrastructure.Database.Configuration;

public class CuentaCorrienteConfiguration : IEntityTypeConfiguration<CuentaCorrienteDto>
{
    public void Configure(EntityTypeBuilder<CuentaCorrienteDto> builder)
    {
        builder.ToTable("CCCCMA", "WILSON1");
        builder.HasKey(cc => cc.CuentaCompleta);
        builder.Property(cc => cc.Aplica).HasColumnName("CC_APLI").IsUnicode(false);
        builder.Property(cc => cc.Sucursal).HasColumnName("CC_SUC").IsUnicode(false);
        builder.Property(cc => cc.NumeroCuenta).HasColumnName("CC_CTA").IsUnicode(false)
            .HasConversion(v => v.Trim(), v => v.Trim());
        builder.Property(cc => cc.SubCuenta).HasColumnName("CC_CTA1").IsUnicode(false);
        builder.Property(cc => cc.CuentaCompleta).HasColumnName("CC_KEY").IsUnicode(false);
            // .HasConversion(v => v.Trim(), v => v.Trim());
        builder.Property(cc => cc.Cancel).HasColumnName("CC_CANCEL");
    }
}
