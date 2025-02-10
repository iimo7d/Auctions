using Auctions.Data;
using Auctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Services
{
    public class ListingServices : IListingServices
    {


        private readonly ApplicationDbContext context;

        public ListingServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Listing listing)
        {
            context.Listings.Add(listing);
            await context.SaveChangesAsync();
        }

        public IQueryable<Listing> GetAll()
        {
            var ApplicationDbContext = context.Listings.Include(l => l.User);
            return ApplicationDbContext;
        }

        public async Task<Listing> GetById(int? id)
        {
            var listing = await context.Listings
                .Include(x => x.User)
                .Include(x => x.Comments)
                .Include(x => x.Bids)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync();
            return listing;


        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
