using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Integrations.Demo
{
    public interface IDemo_Integration
    {
        Task FromNotionToDB(string cursor);
    }
}
