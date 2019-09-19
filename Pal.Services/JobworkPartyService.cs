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
    public interface IJobworkPartyService : IService<JobworkParty>
    {
    }
    public class JobworkPartyService : Service<JobworkParty>, IJobworkPartyService
    {
        public JobworkPartyService(IRepositoryAsync<JobworkParty> respository)
            : base(respository)
        {
        }
    }
}
