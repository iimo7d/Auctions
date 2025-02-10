using Auctions.Data;
using Auctions.Models;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Services
{
    public class BidsService : IBidService
    {
        private readonly ApplicationDbContext context;

        public BidsService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task Add(Bid bid)
        {
            context.Bids.Add(bid);
            await context.SaveChangesAsync();
        }

        public IQueryable<Bid> GetAll()
        {
            var AppDbContext = from a in context.Bids.Include(l => l.listing).ThenInclude(l => l.User)
                select a;
            return AppDbContext;
        }
    }
}
