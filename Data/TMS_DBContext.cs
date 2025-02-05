using Microsoft.EntityFrameworkCore;
using API_TMS.ModMain.Entities;

namespace API_TMS.Data
{
	public class TMS_DBContext : DbContext
	{
		public TMS_DBContext(DbContextOptions dbContextOptions): base(dbContextOptions)
		{
            
        }

		public DbSet<MyTask> MyTasks { get; set; } = null!;
    }
}
