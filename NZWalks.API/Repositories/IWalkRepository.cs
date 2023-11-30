using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        public Task<List<Walk>> GetAllWalksAsync(string? filterOn=null,string? filterQuery=null,string? sortOn=null,bool isAscending=true,int pageNumber=1,int pageSize=1000);
        public Task<Walk> CreateWalkAsync(Walk walk);
        public Task<Walk> GetWalksById(Guid id);
        public Task<Walk> UpdateWalkAsync(Guid id,Walk walk);
        public Task<Walk> DeleteWalkAsync(Guid id);
    }
}
