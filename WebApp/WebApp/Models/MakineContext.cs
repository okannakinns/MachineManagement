using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Models;

public partial class MakineContext : DbContext
{
    public MakineContext()
    {
    }

    public MakineContext(DbContextOptions<MakineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fiyat> Fiyats { get; set; }

    public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }

    public virtual DbSet<Makine> Makines { get; set; }

    public virtual DbSet<MakineDurum> MakineDurums { get; set; }
    public virtual DbSet<Musteri> Musteris { get; set; }
    public virtual DbSet<Marka> Markas { get; set; }
    public virtual DbSet<Model> Models { get; set; }
    public virtual DbSet<Fotograf> Fotografs { get; set; }

    public virtual DbSet<MakineTip> MakineTips { get; set; }

    public virtual DbSet<FisIslemleri> FisIslemleris { get; set; }

    public virtual DbSet<Is> Jobs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=OKAN-LAPTOP\\OKANAKIN;Initial Catalog=Makine;User ID=okannakinn;Password=123456789;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fiyat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fiyat__3214EC0731A561BF");

            entity.ToTable("Fiyat");

            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.Gunluk).HasColumnType("money");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
            entity.Property(e => e.Saatlik).HasColumnType("money");

            entity.HasOne(d => d.MakineTip).WithMany(p => p.Fiyats)
                .HasForeignKey(d => d.MakineTipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fiyat_MakineTip");
        });

        modelBuilder.Entity<Kullanicilar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kullanic__3214EC077C08C4EB");

            entity.ToTable("Kullanicilar");

            entity.Property(e => e.Adi).HasMaxLength(20);
            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
            entity.Property(e => e.Sifre).HasMaxLength(20);
        });

        modelBuilder.Entity<Makine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Makinele__3214EC07D4AE9756");

            entity.ToTable("Makine");

            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.Adi).HasMaxLength(50);
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
            entity.Property(e => e.Plaka).HasMaxLength(20);

            entity.HasOne(d => d.MakineDurum).WithMany(p => p.Makines)
                .HasForeignKey(d => d.MakineDurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Makine_MakineDurum");

            entity.HasOne(d => d.MakineTip).WithMany(p => p.Makines)
                .HasForeignKey(d => d.MakineTipId)
                .HasConstraintName("FK_Makine_MakineTip");
        });
        modelBuilder.Entity<Marka>(entity =>
        {
            entity.ToTable("Marka");
        });
        modelBuilder.Entity<Model>(entity =>
        {
            entity.ToTable("Model");
        });
        modelBuilder.Entity<Fotograf>(entity =>
        {
            entity.ToTable("Fotograf");
        });
        modelBuilder.Entity<Musteri>(entity =>
        {
            entity.ToTable("Musteri");
        });
        modelBuilder.Entity<MakineDurum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MakineDu__3214EC07ACAFA93D");

            entity.ToTable("MakineDurum");

            entity.Property(e => e.Adi).HasMaxLength(20);
            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
        });

        modelBuilder.Entity<MakineTip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MakineTi__3214EC07D1915E42");

            entity.ToTable("MakineTip");

            entity.Property(e => e.Adi).HasMaxLength(50);
            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
        });

        modelBuilder.Entity<FisIslemleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FisIslemleri__3214EC07903EC177");

            entity.ToTable("FisIslemleri");

            entity.Property(e => e.BaslangicTarihi).HasColumnType("datetime");
            entity.Property(e => e.BitisTarihi).HasColumnType("datetime");
            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
            entity.Property(e => e.Ucret).HasColumnType("money");

            //entity.HasOne(d => d.Fiyat).WithMany(p => p.FisIslemleris)
            //    .HasForeignKey(d => d.FiyatId)
            //    .HasConstraintName("FK_FisIslemleri_Fiyat");

            //entity.HasOne(d => d.Makine).WithMany(p => p.FisIslemleris)
            //    .HasForeignKey(d => d.MakineId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_FisIslemleri_Makine");
        });

        modelBuilder.Entity<Is>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Is__3214EC0754DCE3E1");

            entity.ToTable("Is");

            entity.Property(e => e.Adi).HasMaxLength(30);
            entity.Property(e => e.GuncellenmeTarihi).HasColumnType("datetime");
            entity.Property(e => e.OlusturulmaTarihi).HasColumnType("datetime");
           

            
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
