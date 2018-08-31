using Service.Pattern;
using Repository.Pattern.Repositories;
using Pal.Entities.Models;

namespace Pal.Services
{
	public interface IAuditLogService : IService<AuditLog>
	{
	}
	// Task Comment
	public class AuditLogService : Service<AuditLog>, IAuditLogService
	{
		public AuditLogService(IRepositoryAsync<AuditLog> respository)
			: base(respository)
		{
		}
	}
}
