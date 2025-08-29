using Versus.Data;

namespace Versus.Services
{
    public class UserServices
    {
        private readonly VersusContext _context;

        public UserServices(VersusContext context) { 
            _context = context;
        }


    }
}
