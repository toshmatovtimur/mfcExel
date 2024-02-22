using System.IO;
using Microsoft.EntityFrameworkCore;

namespace exel_for_mfc.FilterDB;

public partial class FdbContext : DbContext
{
    public FdbContext()
    {
    }

    public FdbContext(DbContextOptions<FdbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AreaF> AreaFs { get; set; }

    public virtual DbSet<LocalF> Localves { get; set; }

    public virtual DbSet<PayF> PayFs { get; set; }

    public virtual DbSet<PrivF> PrivFs { get; set; }

    public virtual DbSet<SolF> Solves { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(PacHt()); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaF>(entity =>
        {
            entity.ToTable("AreaF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<LocalF>(entity =>
        {
            entity.ToTable("LocalF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PayF>(entity =>
        {
            entity.ToTable("PayF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PrivF>(entity =>
        {
            entity.ToTable("PrivF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<SolF>(entity =>
        {
            entity.ToTable("SolF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    //Относительный путь
    static private string PacHt()
    {
        var x = Directory.GetCurrentDirectory();
        var y = Directory.GetParent(x).FullName;
        var c = Directory.GetParent(y).FullName;
        var r = "Data Source=" + Directory.GetParent(c).FullName + @"\FilterDB\fdb.db";
        return r;
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}