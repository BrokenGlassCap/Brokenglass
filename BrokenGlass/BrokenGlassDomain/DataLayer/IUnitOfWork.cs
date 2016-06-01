using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Adress> AdressRepository { get;}
        IRepository<Claim> ClaimRepository { get;}
        IRepository<ClaimState> ClaimStateRepository { get;}
        IRepository<Photo> PhotoRepository { get;}
        IRepository<Service> ServiceRepository { get;}
        IRepository<User> UserRepository { get;}
        IRepository<GlobalParameters> GlobalParametersRepository { get; }
        IRepository<MetaDataDictionary> MetaDataDictionaryRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
