using APICore.Application.GymSpa.Connectors;
using Microsoft.Extensions.Configuration;

namespace APICore.Application.GymSpa.Services
{
    public class ConnectionStrings : IConnectionStrings
    {
        private readonly IConfiguration _conf;

        public ConnectionStrings(IConfiguration conf)
        {
            _conf = conf;
        }

        string IConnectionStrings.GetConnectionString()
        {
            string cons = _conf.GetConnectionString("DB_A57DC4_PNADb");
            return cons;
        }
    }
}