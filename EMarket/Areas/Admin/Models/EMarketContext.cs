using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EMarket.Areas.Admin.Models
{
    public partial class EMarketContext : DbContext
    {
        public EMarketContext()
        {
        }

        public EMarketContext(DbContextOptions<EMarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public virtual DbSet<HangHoa> HangHoa { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<KhoHang> KhoHang { get; set; }
        public virtual DbSet<Loai> Loai { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<ThongTinTaiKhoan> ThongTinTaiKhoan { get; set; }
        public virtual DbSet<TopSelling> TopSelling { get; set; }
        public virtual DbSet<DanhGia> DanhGia { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=EMarket;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhGia>(entity => 
            {
                entity.HasIndex(e => e.Id);
                
            });
            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasIndex(e => e.HangHoaId);

                entity.HasIndex(e => e.HoaDonId);

                entity.HasOne(d => d.HangHoa)
                    .WithMany(p => p.ChiTietHoaDon)
                    .HasForeignKey(d => d.HangHoaId);

                entity.HasOne(d => d.HoaDon)
                    .WithMany(p => p.ChiTietHoaDon)
                    .HasForeignKey(d => d.HoaDonId);
            });

            modelBuilder.Entity<HangHoa>(entity =>
            {
                entity.HasIndex(e => e.LoaiId);

                entity.HasIndex(e => e.NhaCungCapId);

                entity.Property(e => e.HangHoaId).HasColumnName("HangHoaID");

                entity.Property(e => e.Hinh)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LoaiId).HasColumnName("LoaiID");

                entity.Property(e => e.NhaCungCapId).HasColumnName("NhaCungCapID");

                entity.Property(e => e.TenHangHoa)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Loai)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.LoaiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HangHoa_LoaiID");

                entity.HasOne(d => d.NhaCungCap)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.NhaCungCapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HangHoa_NhaCungCap");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.Property(e => e.DiaChi).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasColumnName("SDT");

                entity.Property(e => e.TenKhachHang).IsRequired();
            });

            modelBuilder.Entity<KhoHang>(entity =>
            {
                entity.HasIndex(e => e.HangHoaId);

                entity.Property(e => e.KhoHangId).HasColumnName("KhoHangID");

                entity.Property(e => e.HangHoaId).HasColumnName("HangHoaID");

                entity.HasOne(d => d.HangHoa)
                    .WithMany(p => p.KhoHang)
                    .HasForeignKey(d => d.HangHoaId);
            });

            modelBuilder.Entity<Loai>(entity =>
            {
                entity.Property(e => e.LoaiId).HasColumnName("LoaiID");

                entity.Property(e => e.TenLoai)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.Property(e => e.NhaCungCapId).HasColumnName("NhaCungCapID");

                entity.Property(e => e.MoTa).HasMaxLength(200);

                entity.Property(e => e.TenNhaCungCap)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
            entity.Property(e => e.TaiKhoanId).HasColumnName("TaiKhoanID");

            entity.Property(e => e.Email).IsRequired();


            entity.Property(e => e.NgayDk)
                .HasColumnName("NgayDK")
                .HasColumnType("datetime");

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<ThongTinTaiKhoan>(entity =>
            {
                entity.HasIndex(e => e.TaiKhoanId);

                entity.Property(e => e.ThongTinTaiKhoanId).HasColumnName("ThongTinTaiKhoanID");

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.HoVaTen)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasColumnName("SDT")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.TaiKhoan)
                    .WithOne(p=>p.ThongTinTaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThongTinTaiKhoan_TaiKhoan");
            });

            modelBuilder.Entity<TopSelling>(entity =>
            {
                entity.HasIndex(e => e.HangHoaId);

                entity.Property(e => e.TopSellingId).HasColumnName("TopSellingID");

                entity.Property(e => e.HangHoaId).HasColumnName("HangHoaID");

                entity.Property(e => e.SoLan).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.HangHoa)
                    .WithMany(p => p.TopSelling)
                    .HasForeignKey(d => d.HangHoaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopSelling_HangHoa");
            });
        }
    }
}
