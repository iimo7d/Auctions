using Auctions.Models;

namespace Auctions.Services
{
    public interface ICommentService
    {
        Task Add(Comment comment);
    }
}
