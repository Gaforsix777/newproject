using Microsoft.EntityFrameworkCore;
using practicamvc.Models;

namespace practicamvc.Data
{
    public class ArtesaniasDBContext : DbContext
    {
        public ArtesaniasDBContext(DbContextOptions<ArtesaniasDBContext> options) : base(options) { }

        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<DetallePedidoModel> DetallePedidos { get; set; }

        // NUEVO
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // NUEVO - Índices únicos
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ... resto de tu configuración existente
            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PedidoModel>()
                .Property(p => p.MontoTotal)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<DetallePedidoModel>()
                .Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PedidoModel>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PedidoModel>()
                .HasMany(p => p.DetallePedidos)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
