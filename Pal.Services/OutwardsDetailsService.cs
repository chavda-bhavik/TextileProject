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
    public interface IOutwardsDetailsService : IService<OutwardsDetails>
    {
    }
    // Task Comment
    public class OutwardsDetailsService : Service<OutwardsDetails>, IOutwardsDetailsService
    {
        public OutwardsDetailsService(IRepositoryAsync<OutwardsDetails> respository)
            : base(respository)
        {
        }
    }
}
