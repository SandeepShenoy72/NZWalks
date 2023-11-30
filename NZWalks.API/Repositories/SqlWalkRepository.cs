using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Migrations;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null,string? filterQuery = null,string? sortOn=null,bool isAscending=true,int pageNumber=1,int pageSize=1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if(!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if(!string.IsNullOrWhiteSpace(sortOn))
            {     
                    if (sortOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending?walks.OrderBy(x => x.Name):walks.OrderByDescending(x => x.Name);
                    }  
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
           // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
           await dbContext.Walks.AddAsync(walk);
           await dbContext.SaveChangesAsync();
           return walk;
        }

        public async Task<Walk> GetWalksById(Guid id)
        {
            var walkItem = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if(walkItem == null) 
            { 
                return null; 
            }
            return walkItem;
        }

        public async Task<Walk> UpdateWalkAsync(Guid id,Walk walk)
        {
            var regionToBeUpdated = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id==id);
            if(regionToBeUpdated == null) 
            {
                return null;
            }
            regionToBeUpdated.Description = walk.Description;
            regionToBeUpdated.Name = walk.Name;
            regionToBeUpdated.LengthInKm = walk.LengthInKm ;
            regionToBeUpdated.WalkImageUrl=walk.WalkImageUrl;
            regionToBeUpdated.DifficultyId = walk.DifficultyId;
            regionToBeUpdated.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return regionToBeUpdated;
        }

        public async Task<Walk> DeleteWalkAsync(Guid id)
        {
            var walkToBeDeleted = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkToBeDeleted == null) return null;
            dbContext.Walks.Remove(walkToBeDeleted);
            await dbContext.SaveChangesAsync();
            return walkToBeDeleted;
        }
    }
}
