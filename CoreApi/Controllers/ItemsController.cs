using Microsoft.AspNetCore.Mvc;
using BestDealLib.Models;
using Microsoft.AspNetCore.Cors;
using CoreApi2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CoreApi2.Services;

namespace CoreApi2.Controllers
{
    /// <summary>
    /// ItemsController
    /// This class contains the methods available to the API that enable
    /// accessing data from the Items table in the database.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("Policy")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        // Database context object
        private readonly ApplicationDbContext _dbContext;

        // Constructor for the class
        public ItemsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        
        // GET: api/Items
        /// <summary>
        /// GetItems(). This method returns a list of all items in the Items
        /// table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
          if (_dbContext.Items == null)
          {
              return NotFound();
          }
            return await _dbContext.Items.ToListAsync();
        }

        // GET: /searchitem/{partialName}
        /// <summary>
        /// GetItemsByPartialString(). This method takes a string and returns
        /// all items having partial string matches.
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        [HttpGet("/searchitem/{partialName}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByPartialString(string partialName)
        {
            var lowerPartialName = partialName.ToLower();

            var items = await _dbContext.Items!
                .Where(item => item.Name!.ToLower().Contains(partialName))
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound();
            }

            return items;
        }

        // GET: /items/{storeId}/{ids}
        /// <summary>
        /// GetItemsByIds(). This method returns a list of items with particular
        /// ids, for one particular store.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("/items/{storeId}/{ids}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByIds(int storeId, string ids)
        {
            string[] idArray = ids.Split(";");

            List<Item> items = new List<Item>();

            for (int i = 0; i < idArray.Length; i++ )
            {
                Item? item = await _dbContext.Items!.FirstOrDefaultAsync(item => item.Id == idArray[i] && item.StoreId == storeId);
                if (item != null)
                {
                    items.Add(item);
                }
                
            }
            return items;
        }

        // GET: /{storeId}/{partialName}
        /// <summary>
        /// GetItemsByNameStoreId(). This method takes a string and compares it
        /// to all properties of the Items in the database and returns a list of
        /// all partial matches.
        /// </summary>
        /// <param name="partialName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("/{storeId}/{partialName}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByNameStoreId(string partialName, int storeId)
        {
            var lowerPartialName = partialName.ToLower();

            var items = await _dbContext.Items!
                .Where(item => item.Name!.ToLower().Contains(partialName) && item.StoreId == storeId)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound();
            }

            return items;
        }

        // GET: /itemsfromstoreid/{id}
        /// <summary>
        /// GetItemsByStoreId(). This method returns all items for a store,
        /// given the store id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/itemsfromstoreid/{id}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByStoreId(int id)
        {
            var items = await _dbContext.Items!
                .Where(item => item.StoreId! == id)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound();
            }

            return items;
        }

        // GET: /{latitude}/{longitude}/{radius}/{partialName}
        /// <summary>
        /// GetItemsByLocationAndName(). This method take in map coordinates, a 
        /// search radius, and a string. It then returns a list of items that
        /// match a partial string search, only from stores within the
        /// geographic search area. It also lists the items by the product, then
        /// decending cost of the item.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <param name="partialName"></param>
        /// <returns></returns>
        [HttpGet("/{latitude}/{longitude}/{radius}/{partialName}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByLocationAndName(double latitude, double longitude, int radius, string partialName)
        {
            GoogleMapsService googleMapsService = new GoogleMapsService(KeyService.GetMapApiKey());
            // Get the closest stores based on latitude and longitude
            List<StoreDistance> storeDistances = await googleMapsService.GetDistancesAsync(latitude, longitude, _dbContext, radius*1000);
            var closestStoreIds = storeDistances.Select(sd => sd.StoreId);

            // Get the item IDs of the items with the specified partial name
            var itemIds = await _dbContext.Items!
                .Where(item => item.Name!.Contains(partialName))
                .Select(item => item.Id)
                .ToListAsync();

            // Get all items with the specified item IDs, including variants in different stores
            var items = await _dbContext.Items!
                .Where(item => itemIds.Contains(item.Id))
                .ToListAsync();

            // Group the items by Item Id and sort by sale price in ascending order
            var groupedItems = items
                .GroupBy(item => item.Id)
                .SelectMany(group =>
                {
                    // If there are multiple variants of the same item, add all of them to the result
                    if (group.Count() > 1)
                        return group;
                    // If there is only one variant, check if it is at the closest store
                    else if (closestStoreIds.Contains(group.First().StoreId))
                        return group;
                    else
                        return Enumerable.Empty<Item>();
                })
                .OrderBy(item => item.SalePrice)
                .ToList();

            return groupedItems;
        }
    }
}
