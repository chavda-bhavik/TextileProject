using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Emailing
{
    public interface IEmailing
    {
        void SendEmail(EmailingMessage mail);
    }
}
