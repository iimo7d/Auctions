using Auctions.Models;

namespace Auctions.Services
{
    public interface IBidService
    {
        Task Add(Bid bid);
        IQueryable<Bid> GetAll();
    }
}
