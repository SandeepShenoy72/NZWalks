using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionController(NZWalksDbContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var regions =  await regionRepository.GetAll();

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
            var regionDomain = await regionRepository.GetRegionById(id);
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
            regionDomain = await regionRepository.CreateRegion(regionDomain);
            return CreatedAtAction(nameof(GetRegionById), new {id=regionDomain.Id}, AddregionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,UpdateRegionDto UpdateRegion)
        {
            var UpdateRegionModel = new Region
            {
                Code = UpdateRegion.Code,
                Name = UpdateRegion.Name,
                RegionImageUrl = UpdateRegion.RegionImageUrl,
            };
            var regionDomainModel = await regionRepository.UpdateRegionAsync(id, UpdateRegionModel);

            if (regionDomainModel == null) return NotFound();

            return Ok(regionDomainModel);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.DeleteRegionAsync(id);

            if(regionDomain == null) return NotFound();
          
            return Ok(regionDomain);
        }

    }
}
