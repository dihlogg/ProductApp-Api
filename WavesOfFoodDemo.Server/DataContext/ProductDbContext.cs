using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.AppSettings;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.DataContext;

public class ProductDbContext : DbContext
{
    private readonly PostgreSetting _postgreSetting;

    public ProductDbContext(PostgreSetting postgreSetting)
    {
        _postgreSetting = postgreSetting;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_postgreSetting.ConnectionString ?? "");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }

    public virtual DbSet<ProductInfo> ProductInfos { get; set; }
    public virtual DbSet<UserInfo> UserInfos { get; set; }
    public virtual DbSet<CartInfo> CartInfos { get; set; }
    public virtual DbSet<CartDetails> CartDetails { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
    public virtual DbSet<ProductInfoHistory> ProductInfoHistorys { get; set; }
    public virtual DbSet<Conversations> Conversations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().ToTable("Categories").HasKey(x => x.Id);
        modelBuilder.Entity<ProductImage>().ToTable("ProductImages").HasKey(x => x.Id);
        //conversations
        modelBuilder.Entity<Conversations>().ToTable("Conversations").HasKey(x => x.Id);
        modelBuilder.Entity<Conversations>()
        .Property(c => c.ProductsJson)
        .HasColumnType("jsonb");

        modelBuilder.Entity<Conversations>()
            .HasOne<UserInfo>(s => s.UserInfos)
            .WithMany(g => g.Conversations)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<ProductInfo>().ToTable("ProductInfos").HasKey(x => x.Id);
        modelBuilder.Entity<ProductInfo>()
        .HasOne<Category>(s => s.Categories)
        .WithMany(g => g.ProductInfos)
        .HasForeignKey(s => s.CategoryId);

        modelBuilder.Entity<ProductInfo>()
        .HasMany(p => p.ProductImages)
        .WithOne(i => i.ProductInfos)
        .HasForeignKey(i => i.ProductInfoId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductInfoHistory>()
       .HasMany(p => p.ProductImages)
       .WithOne(i => i.ProductInfoHistories)
       .HasForeignKey(i => i.ProductInfoHistoryId)
       .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserInfo>().ToTable("UserInfos").HasKey(x => x.Id);

        modelBuilder.Entity<CartInfo>().ToTable("CartInfos")
        .HasOne<UserInfo>(s => s.UserInfos)
        .WithMany(g => g.CartInfos)
        .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<CartDetails>().HasKey(x => x.Id);
        modelBuilder.Entity<CartDetails>()
        .HasOne<ProductInfo>(s => s.ProductInfo)
        .WithMany(g => g.CartDetails)
        .HasForeignKey(s => s.ProductId);
        modelBuilder.Entity<CartDetails>()
        .HasOne<CartInfo>(s => s.CartInfo)
        .WithMany(g => g.CartDetails)
        .HasForeignKey(s => s.CartId);
    }

    public override int SaveChanges()
    {
        var dateNow = DateTime.UtcNow;
        var errorList = new List<ValidationResult>();

        var entries = ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Added ||
                        p.State == EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            if (entry.State == EntityState.Added)
            {
                if (entity is BaseEntities itemBase)
                {
                    itemBase.CreateDate = itemBase.UpdateDate = dateNow;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entity is BaseEntities itemBase)
                {
                    itemBase.UpdateDate = dateNow;
                }
            }

            Validator.TryValidateObject(entity, new ValidationContext(entity), errorList);
        }

        if (errorList.Count != 0)
        {
            throw new Exception(string.Join(", ", errorList.Select(p => p.ErrorMessage)).Trim());
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var errorList = new List<ValidationResult>();

        var entries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified).ToList();

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            if (entry.State == EntityState.Added)
            {
                if (entity is BaseEntities itemBase)
                {
                    itemBase.CreateDate = itemBase.UpdateDate = DateTime.UtcNow;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entity is BaseEntities itemBase)
                {
                    itemBase.UpdateDate = DateTime.UtcNow;
                }
            }

            Validator.TryValidateObject(entity, new ValidationContext(entity), errorList);
        }

        if (errorList.Count != 0)
        {
            throw new Exception(string.Join(", ", errorList.Select(p => p.ErrorMessage)).Trim());
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}