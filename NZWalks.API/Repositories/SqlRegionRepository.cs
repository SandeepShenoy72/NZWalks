using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAll()
        {
           return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionById(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> CreateRegion(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateRegionAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null) 
            { 
                return null;  
            }

            existingRegion.Code=region.Code;
            existingRegion.Name=region.Name;
            existingRegion.RegionImageUrl=region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;

        }

        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            var regionToBeDeleted = await dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);
            if(regionToBeDeleted == null)
            {
                return null;
            }
            dbContext.Regions.Remove(regionToBeDeleted);
            await dbContext.SaveChangesAsync();

            return regionToBeDeleted;

        }
    }
}
