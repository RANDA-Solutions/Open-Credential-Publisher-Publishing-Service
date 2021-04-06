using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Stores
{
    public class ClientStore: IClientStore
    {
        private readonly ConfigurationDbContext _context;
        private readonly IMapper _mapper;
        public ClientStore(ConfigurationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var dbClient = await _context.Clients
                                    .Include(cl => cl.Claims)
                                    .FirstOrDefaultAsync(x => x.ClientId == clientId);
            var client = _mapper.Map<Client>(dbClient);
            return client;
        }
    }
}
