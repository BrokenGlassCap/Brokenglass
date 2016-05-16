using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public interface IUnitOfWork
    {
        GenericRepository<Adress> AdressRepository { get;}
        GenericRepository<Claim> ClaimRepository { get;}
        GenericRepository<ClaimState> ClaimStateRepository { get;}
        GenericRepository<Photo> PhotoRepository { get;}
        GenericRepository<Service> ServiceRepository { get;}
        GenericRepository<User> UserRepository { get;}
        void Save();
    }
}
