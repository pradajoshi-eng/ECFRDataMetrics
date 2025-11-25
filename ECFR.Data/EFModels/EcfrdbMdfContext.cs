using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace ECFR.Data.EFModels;

public partial class EcfrdbMdfContext : DbContext
{
    private readonly IHostingEnvironment _env;
    public EcfrdbMdfContext()
    {
    }

    public EcfrdbMdfContext(DbContextOptions<EcfrdbMdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agency> Agencies { get; set; }

    public virtual DbSet<AgencyCfrReferenceRel> AgencyCfrReferenceRels { get; set; }

    public virtual DbSet<AgencySnapshot> AgencySnapshots { get; set; }

    public virtual DbSet<CfrReference> CfrReferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Data\\ecfrdb.mdf;Integrated Security=True;MultipleActiveResultSets=true";
        connectionString = connectionString.Replace("|DataDirectory|", Directory.GetCurrentDirectory());
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0747EB0550");

            entity.ToTable("Agency");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .HasColumnName("Display_name");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ShortName)
                .HasMaxLength(255)
                .HasColumnName("Short_name");
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.SortableName)
                .HasMaxLength(255)
                .HasColumnName("Sortable_name");

            entity.HasOne(d => d.AgencyNavigation).WithMany(p => p.InverseAgencyNavigation)
                .HasForeignKey(d => d.AgencyId)
                .HasConstraintName("FK_Agency_RelatedAgency");
        });

        modelBuilder.Entity<AgencyCfrReferenceRel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07AB812C40");

            entity.ToTable("Agency_CfrReference_Rel");

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyCfrReferenceRels)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agency_CfrReference_Rel_Agency");

            entity.HasOne(d => d.Title).WithMany(p => p.AgencyCfrReferenceRels)
                .HasForeignKey(d => d.TitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agency_CfrReference_Rel_CfrReference");
        });

        modelBuilder.Entity<AgencySnapshot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC075F9E1EA3");

            entity.ToTable("AgencySnapshot");

            entity.Property(e => e.RetrievedAt).HasColumnType("datetime");
            entity.Property(e => e.SourceUrl).HasMaxLength(255);

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencySnapshots)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencySnapshot_Agency");
        });

        modelBuilder.Entity<CfrReference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07354AEE93");

            entity.ToTable("CfrReference");

            entity.Property(e => e.Chapter).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
