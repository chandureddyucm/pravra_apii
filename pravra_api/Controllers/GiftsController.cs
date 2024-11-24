using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pravra_api.Extensions;
using pravra_api.Interfaces;
using pravra_api.Models;

namespace pravra_api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/gifts")]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftsController(IGiftService giftService)
        {
            _giftService = giftService;
        }

        [HttpPost("addGift")]
        public async Task<IActionResult> AddGift(IFormCollection form)
        {
            var file = form.Files.GetFile("image"); 
            Gift gift = new Gift
            {
                Name = form["name"].ToString(), 
                Description = form["description"].ToString(),
                Category = form["category"].ToString(),
                Subcategory = form["subCategory"].ToString(),
                Price = decimal.TryParse(form["price"], out decimal price) ? price : 0, 
                ImageSrc = "" 
            };
            var newGift = await _giftService.AddGift(gift, file);
            return newGift.ToActionResult();
        }

        [HttpPut("updateGift/{giftId}")]
        public async Task<IActionResult> UpdateGift(string giftId, [FromBody] Gift gift)
        {
            var response = await _giftService.UpdateGift(giftId, gift);
            return response.ToActionResult();
        }

        [HttpDelete("deleteGift/{giftId}")]
        public async Task<IActionResult> DeleteGift(string giftId)
        {
            var response = await _giftService.DeleteGift(giftId);
            return response.ToActionResult();
        }

        [HttpGet("getAllGifts")]
        public async Task<IActionResult> GetAllGifts()
        {
            var gifts = await _giftService.GetAllGifts();
            return gifts.ToActionResult();
        }

        [HttpGet("getGiftById/{giftId}")]
        public async Task<IActionResult> GetGiftById(string giftId)
        {
            var gift = await _giftService.GetGiftById(giftId);
            if (gift == null) return NotFound();
            return gift.ToActionResult();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredGifts(
            [FromQuery] string? category,
            [FromQuery] string? subcategory,
            [FromQuery] bool? availability,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var gifts = await _giftService.GetFilteredGifts(category, subcategory, availability, minPrice, maxPrice);
            return gifts.ToActionResult();
        }
    }
}