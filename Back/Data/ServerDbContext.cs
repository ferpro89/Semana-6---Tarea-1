using Microsoft.EntityFrameworkCore;
using Semana5.Model;
namespace Semana5.Data
{
    public class ServerDbContext: DbContext
    {
        public ServerDbContext(DbContextOptions db) : base(db)
        {
        }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<Semana5.Model.UsuariosModel> UsuariosModel { get; set; } = default!;

    }
}
