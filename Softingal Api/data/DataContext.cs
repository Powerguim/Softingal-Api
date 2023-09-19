using Microsoft.EntityFrameworkCore;
using Softingal_Api;

namespace Softingal_Api.data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options){ }

        public DbSet<Address> Addresses => Set<Address>();

        public DbSet<User> Users => Set<User>();


    }
}
