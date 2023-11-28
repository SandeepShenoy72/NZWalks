using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var regions =  await dbContext.Regions.ToListAsync();

            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto(){
                    Id=region.Id,
                    Code=region.Code,
                    Name=region.Name,
                    RegionImageUrl=region.RegionImageUrl
                });
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id) 
        {
            
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id==id);
            if(regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            
            return Ok(regionDto);  
        }

        [HttpPost]

        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto AddregionDto)
        {
            var regionDomain = new Region
            {
                Name = AddregionDto.Name,
                Code = AddregionDto.Code,
                RegionImageUrl = AddregionDto.RegionImageUrl
            };
           await dbContext.Regions.AddAsync(regionDomain);
           await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegionById), new {id=regionDomain.Id}, AddregionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,UpdateRegionDto UpdateRegion)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null) return NotFound();

            regionDomainModel.Code=UpdateRegion.Code;
            regionDomainModel.Name=UpdateRegion.Name;
            regionDomainModel.RegionImageUrl=UpdateRegion.RegionImageUrl;
           
            await dbContext.SaveChangesAsync();

            return Ok(UpdateRegion);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);
            if(regionDomain == null) return NotFound();

            dbContext.Regions.Remove(regionDomain);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
