using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auctions.Data;
using Auctions.Models;
using Auctions.ViewModels;
using Auctions.Services;
using System.Security.Claims;

namespace Auctions.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingServices _listingService;
        private readonly IBidService _bidService;
        private readonly ICommentService _commentService;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ListingsController(IListingServices listingService, IWebHostEnvironment webHostEnvironment, IBidService bidService, ICommentService commentService)
        {
            _listingService = listingService;
            _webHostEnviroment = webHostEnvironment;
            _bidService = bidService;
            _commentService = commentService;
        }


        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            var list = _listingService.GetAll();
            int pageSize = 3;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(a => a.Title.Contains(searchString));
                return View(await PaginatedList<Listing>.CreateAsync(list.Where(l => l.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));

            }

            return View(await PaginatedList<Listing>.CreateAsync(list.Where(l => l.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        public async Task<IActionResult> MyListings(int? pageNumber)
        {
            var myListing = _listingService.GetAll();
            int pageSize = 3;

            return View("Index",await PaginatedList<Listing>.CreateAsync(myListing.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MyBids(int? pageNumber)
        {
            var myBids = _bidService.GetAll();
            int pageSize = 3;

            return View(await PaginatedList<Bid>.CreateAsync(myBids.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ListingVM listing)
        {
            if (listing.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "Images");
                string fileName = listing.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    listing.Image.CopyTo(fileStream);
                }

                var listObj = new Listing
                {
                    Title = listing.Title,
                    Description = listing.Description,
                    Price = listing.Price,
                    IdentityUserId = listing.IdentityUserId,
                    ImagePath = fileName,
                };
                await _listingService.Add(listObj);
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingService.GetById(id);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        [HttpPost]
        public async Task<ActionResult> AddBid([Bind("Id,Price,ListingId,IdentityUserId")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                await _bidService.Add(bid);
            }
            var listing = await _listingService.GetById(bid.ListingId);
            listing.Price = bid.Price;
            await _listingService.SaveChanges();

            return View("Details", listing);
        }

        public async Task<ActionResult> CloseBidding(int id)
        {
            var listing = await _listingService.GetById(id);
            listing.IsSold = true;
            await _listingService.SaveChanges();

            return View("Details", listing);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment([Bind("Id, Content, ListingId, IdentityUserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentService.Add(comment);
            }
            var listing = await _listingService.GetById(comment.ListingId);
            return View("Details", listing);
        }
    }
}
