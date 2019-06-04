using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaseDados.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    /// <summary>
    /// apresenta a craicao da base de dados
    ///     - dos utilizadores
    ///     - dos dados do negócio
    /// </summary>
    public class MultasDB : IdentityDbContext<ApplicationUser>
    {
        //construtor que identifica a localizaçao da DB
        public MultasDB()
            : base("MultasDBConnectionString", throwIfV1Schema: false)
        {
        }

        static MultasDB()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<MultasDB>(new ApplicationDbInitializer());
        }

        public static MultasDB Create()
        {
            return new MultasDB();
        }



        //adicionar aqui as tabelas da BD
        // vamos colocar, aqui, as instruções relativas às tabelas do 'negócio'
        // descrever os nomes das tabelas na Base de Dados
        public virtual DbSet<Multas> Multas { get; set; } // tabela Multas
        public virtual DbSet<Condutores> Condutores { get; set; } // tabela Condutores
        public virtual DbSet<Agentes> Agentes { get; set; } // tabela Agentes
        public virtual DbSet<Viaturas> Viaturas { get; set; } // tabela Viaturas

        //método a ser executado no inicio da criação do Modelo
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //eliminar a convenção de atribuir automáticamente 'on Delete Cascade' nas FKs
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            }
        }
}