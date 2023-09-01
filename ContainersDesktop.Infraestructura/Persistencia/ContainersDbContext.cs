using System.Reflection;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using CoreDesktop.Dominio.Models;
using CoreDesktop.Dominio.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Infraestructura.Persistencia;
public partial class ContainersDbContext : DbContext
{   
    public ContainersDbContext(DbContextOptions<ContainersDbContext> options) : base(options) 
    {
    }

    public virtual DbSet<UI_Default> UI_Default { get; set; }

    public virtual DbSet<UI_Config> UI_Config { get; set; }

    public virtual DbSet<Mensaje> Mensajes
    {
        get; set;
    }

    public virtual DbSet<Movim> Movim
    {
        get; set;
    }

    public virtual DbSet<ClaList> ClaList
    {
        get; set;
    }

    public virtual DbSet<Lista> Listas
    {
        get; set;
    }

    public virtual DbSet<TareaProgramada> TareasProgramadas
    {
        get; set;
    }

    public virtual DbSet<Objeto> Containers
    {
        get; set;
    }

    public virtual DbSet<Sincronizacion> Sincronizaciones
    {
        get; set;
    }

    public virtual DbSet<Dispositivo> Dispositivos
    {
        get; set;
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.FechaActualizacion = FormatoFecha.FechaEstandar(DateTime.Now);
                    break;
                case EntityState.Modified:
                    entry.Entity.FechaActualizacion = FormatoFecha.FechaEstandar(DateTime.Now);
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
