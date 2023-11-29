using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        public Task<List<Walk>> GetAllWalksAsync();
        public Task<Walk> CreateWalkAsync(Walk walk);
        public Task<Walk> GetWalksById(Guid id);
        public Task<Walk> UpdateWalkAsync(Guid id,Walk walk);
        public Task<Walk> DeleteWalkAsync(Guid id);
    }
}
