using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.Dto;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            //Get All Regions from Web Api
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7087/api/Region");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                //Log Exception
            }
            
            return View(response);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddRegionViewModel addModel)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7087/api/Region"),
                Content = new StringContent(JsonSerializer.Serialize(addModel), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
          
        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7087/api/Region/{id.ToString()}");
               if(response!=null)
                {
                    return View(response);
                }
            }
            catch(Exception e)
            {

            }
           
            return View(ViewBag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto regionDto)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7087/api/Region/{regionDto.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(regionDto), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7087/api/Region/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }catch(Exception ex)
            {
               //Log
            }
            return View("Edit");
        }

    }
}
