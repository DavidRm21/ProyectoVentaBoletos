
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalM.DataAccess;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales{ get; set; }
    public DbSet<Movie> Movies{ get; set; }
    public DbSet<Ticket> Tickets{ get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Cinema");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Sale>()
        .HasKey(s => s.Id);
        modelBuilder.Entity<Sale>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Sale>()
            .HasMany(s => s.Tickets)
            .WithOne(t => t.Sale)
            .HasForeignKey(t => t.SaleId);

        modelBuilder.Entity<User>().HasData(
            new User (1, "admin", "password"),
            new User (2, "luis", "a2dt"),
            new User (3, "cris", "123")
            );

        modelBuilder.Entity<Movie>().HasData(
            new Movie(1, "Inception", "Ciencia Ficción", 148, "PG-13", "C:\\Users\\Cristian\\source\\repos\\ProyectoFinalM\\Resources\\Images\\inception.jpg"),
            new Movie(2, "The Dark Knight", "Acción", 152, "PG-13", "C:\\Users\\Cristian\\source\\repos\\ProyectoFinalM\\Resources\\Images\\caballeto.jpg"),
            new Movie(3, "Parasite", "Drama", 132, "R", "C:\\Users\\Cristian\\source\\repos\\ProyectoFinalM\\Resources\\Images\\parasite.jpg"),
            new Movie(4, "Toy Story", "Animación", 81, "G", "C:\\Users\\Cristian\\source\\repos\\ProyectoFinalM\\Resources\\Images\\toystory.jpeg")
            );

        modelBuilder.Entity<Ticket>().HasData(
            );

    }

}

public record User (int Id, string Username, string Password);
public record Movie(int Id, string Titulo, string Genero, int Duracion, string Clasificacion, string ImageUrl);


public record Ticket
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int SaleId { get; set; }
    public string SeatNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Sale Sale { get; set; }

    public Ticket(int movieId, string seatNumber, DateTime purchaseDate)
    {
        MovieId = movieId;
        SeatNumber = seatNumber;
        PurchaseDate = purchaseDate;
    }
}

public record Sale
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public DateTime PurchaseDate { get; set; }
    public List<Ticket> Tickets { get; set; } = new();

    public Sale(int movieId, decimal subtotal, decimal tax, decimal total, DateTime purchaseDate)
    {
        MovieId = movieId;
        Subtotal = subtotal;
        Tax = tax;
        Total = total;
        PurchaseDate = purchaseDate;
    }
    
}


