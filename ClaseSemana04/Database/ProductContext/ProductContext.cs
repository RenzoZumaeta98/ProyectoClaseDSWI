using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ClaseSemana04.Database.ProductContext
{
    public class ProductContext : DbContext
    {
        //Seccion indico que entidades deben convertirse en tablas
        public DbSet<ProductEntity> Products { get; set; }


        //Constructor base disenado para que se guie de lo escrito en el Startup
        public ProductContext(DbContextOptions<ProductContext> option) : base(option) //
        {
           
        }

        //Metodo para hacer que se guarden los cambios
        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
