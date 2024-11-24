using System;
using pravra_api.Models;
using System.Threading.Tasks;

namespace pravra_api.Interfaces
{
    public interface IGiftService
    {
        Task<ServiceResponse<Gift>> AddGift(Gift gift, IFormFile? image);
        Task<ServiceResponse<Gift>> UpdateGift(string giftId, Gift gift, IFormFile? image);
        Task<ServiceResponse<bool>> DeleteGift(string giftId);
        Task<ServiceResponse<IEnumerable<Gift>>> GetAllGifts();
        Task<ServiceResponse<Gift>> GetGiftById(string giftId);
        Task<ServiceResponse<IEnumerable<Gift>>> GetFilteredGifts(string? category, string? subcategory, bool? availability, decimal? minPrice, decimal? maxPrice);
    }
}