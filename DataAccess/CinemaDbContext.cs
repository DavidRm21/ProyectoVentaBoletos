
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalM.DataAccess;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales{ get; set; }
    public DbSet<Movie> Movies{ get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Cinema");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User (1, "admin", "password"),
            new User (2, "luis", "a2dt"),
            new User (3, "cris", "123")
            );

        modelBuilder.Entity<Movie>().HasData(
            new Movie(1, "Inception", "Ciencia Ficción", 148, "PG-13"),
            new Movie(2, "The Dark Knight", "Acción", 152, "PG-13"),
            new Movie(3, "Parasite", "Drama", 132, "R"),
            new Movie(4, "Toy Story", "Animación", 81, "G")
            );

    }


}

public record User (int Id, string Username, string Password);
public record Sale (int Id, string Name, double Price);
public record Movie(int Id, string Titulo, string Genero, int Duracion, string Clasificacion);


