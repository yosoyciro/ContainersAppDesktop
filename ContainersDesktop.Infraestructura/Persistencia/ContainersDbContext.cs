using System.Reflection;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Infraestructura.Persistencia;
public partial class ContainersDbContext : DbContext
{   
    public ContainersDbContext(DbContextOptions<ContainersDbContext> options) : base(options) 
    {
    }

    public virtual DbSet<UI_Default> UI_DEFAULT
    {
        get;
        set;
    }

    public virtual DbSet<UI_Config> UI_CONFIG
    {
        get;
        set;
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)        
    //    => optionsBuilder.UseSqlite();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //No dejar borrar registros padres con hijos
        //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //{
        //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        //}
    }
}
