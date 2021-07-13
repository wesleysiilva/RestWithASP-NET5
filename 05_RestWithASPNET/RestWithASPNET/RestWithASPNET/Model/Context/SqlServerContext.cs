using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET.Model.Context {
  public class SqlServerContext : DbContext {
    public SqlServerContext() {}
    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base (options) {}
    public DbSet<Person> Persons { get; set; }
    public DbSet<User> User { get; set; }
  }
}
