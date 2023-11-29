﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walksRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walksRepository,IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync() 
        {
            var allWalks = await walksRepository.GetAllWalksAsync();
            return Ok(mapper.Map<List<WalkDto>>(allWalks));
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetWalksById([FromRoute] Guid id)
        {
            var walkDomain = await walksRepository.GetWalksById(id);
            if(walkDomain == null) 
            { 
                return NotFound(); 
            }
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalkAsync([FromBody] AddWalkDto addDto)
        {
            var walkDomain = await walksRepository.CreateWalkAsync(mapper.Map<Walk>(addDto));
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkDto updateDto)
        {
            var updateWalk = await walksRepository.UpdateWalkAsync(id, mapper.Map<Walk>(updateDto));
            if(updateWalk == null)return NotFound();
            return Ok(mapper.Map<WalkDto>(updateWalk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            var deletedWalk = await walksRepository.DeleteWalkAsync(id);
            if(deletedWalk == null) return NotFound();
            return Ok(mapper.Map<WalkDto>(deletedWalk));
        }


    }
}
