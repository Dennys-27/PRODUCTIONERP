using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FERSOFT.ERP.Infrastructure.Seeding
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            // Verificar si la tabla Movies está vacía
            if (!context.Movies.Any())
            {
                Console.WriteLine("Insertando películas...");

                context.Movies.AddRange(new List<MovieEntity>
                {
                    new MovieEntity
                    {
                        Name = "Inception",
                        Genre = MovieGenreEnum.SCIENCE_FICTION,
                        AllowedAge = 13,
                        LengthMinutes = 148
                    },
                    new MovieEntity
                    {
                        Name = "The Dark Knight",
                        Genre = MovieGenreEnum.ACTION,
                        AllowedAge = 16,
                        LengthMinutes = 152
                    },
                    new MovieEntity
                    {
                        Name = "Interstellar",
                        Genre = MovieGenreEnum.SCIENCE_FICTION,
                        AllowedAge = 10,
                        LengthMinutes = 169
                    },
                    new MovieEntity
                    {
                        Name = "La La Land",
                        Genre = MovieGenreEnum.MUSICALS,
                        AllowedAge = 12,
                        LengthMinutes = 128
                    }
                });

                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Películas ya existen.");
            }

            // Verificar si la tabla Clients está vacía
            if (!context.Customers.Any())
            {
                Console.WriteLine("Insertando clientes...");
                context.Customers.AddRange(new List<CustomerEntity>
                {
                    new CustomerEntity
                    {
                        Name = "John Doe",
                        Email = "johndoe@example.com"
                    },
                    new CustomerEntity
                    {
                        Name = "Jane Smith",
                        Email = "janesmith@example.com"
                    },
                    new CustomerEntity
                    {
                        Name = "Alice Johnson",
                        Email = "alicejohnson@example.com"
                    }
                });

                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Clientes ya existen.");
            }
        }
    }
}
