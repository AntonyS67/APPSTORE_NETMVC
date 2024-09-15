using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.Domain
{
    public class LoadDatabase
    {
        public static async Task InsertarData(DatabaseContext context, UserManager<ApplicationUser> usuarioManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }

            if (!usuarioManager.Users.Any())
            {
                var usuario = new ApplicationUser
                {
                    Nombre = "Antoni silva",
                    Email = "ansir.manuel@gmail.com",
                    UserName = "ansir"
                };
                await usuarioManager.CreateAsync(usuario, "antonysilva123#R");
                await usuarioManager.AddToRoleAsync(usuario, "ADMIN");
            }

            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria { Nombre = "Drama" },
                    new Categoria { Nombre = "Comedia" },
                    new Categoria { Nombre = "Accion" },
                    new Categoria { Nombre = "Terror" },
                    new Categoria { Nombre = "Aventura" }
                );
            }

            if (!context.Libros.Any())
            {
                context.Libros.AddRange(
                    new Libro { Titulo = "El quijote de la mancha", CreateDate = "06/06/2020", Imagen = "quijote.jpg", Autor = "Miguel de Cervantes" },
                    new Libro { Titulo = "Harry Potter", CreateDate = "06/06/2021", Imagen = "harry.jpg", Autor = "Juan de la vega" }
                );
            }

            if (!context.LibroCategoria.Any())
            {
                context.LibroCategoria.AddRange(
                    new LibroCategoria { CategoriaId = 1, LibroId = 1 },
                    new LibroCategoria { CategoriaId = 1, LibroId = 2 }
                );
            }

            context.SaveChanges();
        }
    }
}
