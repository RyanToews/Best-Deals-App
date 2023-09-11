using BestDealLib.Models;
using Microsoft.AspNetCore.Mvc;
using CoreApi2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using CoreApi2.Services;
using System.Linq;

namespace CoreApi2.Controllers
{
    /// <summary>
    /// StoresController.
    /// This class contains the methods available to the API that enable
    /// accessing the data from the Stores table in the database.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("Policy")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        // Database context object
        private readonly ApplicationDbContext _dbContext;

        // Constructor for the class
        public StoresController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // GET: api/Stores
        /// <summary>
        /// GetStores(). This method returns all stores in the Stores table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
          if (_dbContext.Stores == null)
          {
              return NotFound();
          }
            return await _dbContext.Stores.ToListAsync();
        }

        
        // GET api/Stores/{id}
        /// <summary>
        /// GetStore(). This method returns a specific store from the Stores
        /// table, given a specific store id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var storesResult = await GetStores();
            var stores = storesResult.Value;

            foreach (Store store in stores!)
            {
                if (id == store.Id)
                {
                    return store;
                }
            }

            return NotFound();
        }
        
        /// <summary>
        /// GetStores(). This method gets all stores within location data
        /// parameters.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius">In kilometers</param>
        /// <returns></returns>
        // GET /{latitude}+{longitude}
        [HttpGet("/{latitude},{longitude};{radius}")]
        public async Task<ActionResult<List<StoreDistance>>> GetStores(double latitude, double longitude, int radius)
        {
            GoogleMapsService googleMapsService = new GoogleMapsService(KeyService.GetMapApiKey()!);
            List<StoreDistance> storeDistances = await googleMapsService.GetDistancesAsync(latitude, longitude, _dbContext, radius*1000);
            return storeDistances;
        }


        // GET: /stores/{storeId}/{ids}
        /// <summary>
        /// GetItemsByIds(). This method returns a list of stores with particular
        /// ids.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("/stores/{ids}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByIds(string ids)
        {
            string[] idArray = ids.Split(";");

            // Convert idArray to List<int> assuming Id is int.
            List<int> intIds = idArray.Select(int.Parse).ToList();

            // Query the database once for all matching IDs.
            List<Store> stores = await _dbContext.Stores
                .Where(store => intIds.Contains(store.Id))
                .ToListAsync();

            return stores;
        }


    }
}
