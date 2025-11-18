using FutbotecaApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FutbotecaApi.Context
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<Reportes> Reportes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // No olvides llamar a la base para que Identity funcione correctamente
            // Definimos que la relación entre Seguidor y Seguido sea única
            modelBuilder.Entity<Seguimiento>()
                .HasIndex(s => new { s.SeguidorId, s.SeguidoId })
                .IsUnique();

            // Relación de Seguidor -> Seguidos (Usuario sigue a otros usuarios)
            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // 🔥 Esto es importante
            });
            modelBuilder.Entity<Seguimiento>()
                .HasOne(s => s.Seguidor)
                .WithMany(u => u.Siguiendo)  // Usuario tiene muchos "Siguiendo"
                .HasForeignKey(s => s.SeguidorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Relación de Seguido -> Seguidores (Usuarios que siguen a otro usuario)
            modelBuilder.Entity<Seguimiento>()
                .HasOne(s => s.Seguido)
                .WithMany(u => u.Seguidores)  // Usuario tiene muchos "Seguidores"
                .HasForeignKey(s => s.SeguidoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UsuarioId, l.VideoId })
                .IsUnique();  // Asegura que un usuario no pueda dar "like" al mismo video más de una vez
        }



    }
}
