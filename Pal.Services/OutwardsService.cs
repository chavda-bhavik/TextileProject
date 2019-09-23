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
    public interface IOutwardsService : IService<Outwards>
    {
    }
    // Task Comment
    public class OutwardsService : Service<Outwards>, IOutwardsService
    {
        public OutwardsService(IRepositoryAsync<Outwards> respository)
            : base(respository)
        {
        }
    }
}
