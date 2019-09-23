using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using Repository.Pattern.Repositories;
using Pal.Entities.Models;

namespace Pal.Services
{
    public interface IJobworkService : IService<Jobwork>
    {
    }
    // Task Comment
    public class JobworkService : Service<Jobwork>, IJobworkService
    {
        public JobworkService(IRepositoryAsync<Jobwork> respository)
            : base(respository)
        {
        }
    }
}
