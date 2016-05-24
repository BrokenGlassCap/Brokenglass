using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private BROKEN_GLASSEntities m_context = new BROKEN_GLASSEntities();

        IRepository<Adress> m_adress;
        IRepository<Claim> m_claim;
        IRepository<ClaimState> m_claimState;
        IRepository<Photo> m_photo;
        IRepository<Service> m_service;
        IRepository<User> m_user;

        public IRepository<Adress> AdressRepository
        {
            get
            {
                if (m_adress == null)
                {
                    m_adress = new GenericRepository<Adress>(m_context);
                }
                return m_adress;
            }
        }

        public IRepository<Claim> ClaimRepository
        {
            get
            {
                if (m_claim == null)
                {
                    m_claim = new GenericRepository<Claim>(m_context);
                }
                return m_claim;
            }
        }

        public IRepository<ClaimState> ClaimStateRepository
        {
            get
            {
                if (m_claimState == null)
                {
                    m_claimState = new GenericRepository<ClaimState>(m_context);
                }
                return m_claimState;
            }
        }

        public IRepository<Photo> PhotoRepository
        {
            get
            {
                if (m_photo == null)
                {
                    m_photo = new GenericRepository<Photo>(m_context);
                }
                return m_photo;
            }
        }

        public IRepository<Service> ServiceRepository
        {
            get
            {
                if (m_service == null)
                {
                    m_service = new GenericRepository<Service>(m_context);
                }
                return m_service;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (m_user == null)
                {
                    m_user = new GenericRepository<User>(m_context);
                }
                return m_user;
            }
        }

        public void Save()
        {
            m_context.SaveChanges();
        }

        public void Dispose()
        {
            m_context.Dispose();
        }

        public async Task SaveAsync()
        {
            await m_context.SaveChangesAsync();
        }
    }
}
