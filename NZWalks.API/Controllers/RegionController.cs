using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
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
        private readonly IMapper mapper;

        public RegionController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var regions =  await regionRepository.GetAll();
            return Ok(mapper.Map<List<RegionDto>>(regions));
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
            return Ok(mapper.Map<RegionDto>(regionDomain));  
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto AddregionDto)
        {
            
                var regionDomain = await regionRepository.CreateRegion(mapper.Map<Region>(AddregionDto));
                return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, mapper.Map<RegionDto>(regionDomain));
          
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,UpdateRegionDto UpdateRegion)
        {
            var regionDomainModel = await regionRepository.UpdateRegionAsync(id, mapper.Map<Region>(UpdateRegion));
            if (regionDomainModel == null) return NotFound();
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.DeleteRegionAsync(id);
            if(regionDomain == null) return NotFound();        
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

    }
}
