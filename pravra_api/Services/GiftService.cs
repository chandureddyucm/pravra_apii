using System;
using MongoDB.Driver;
using pravra_api.Interfaces;
using pravra_api.Models;
using Microsoft.Extensions.Options;
using pravra_api.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;
using pravra_api.Extensions;

namespace pravra_api.Services
{

    public class GiftService : IGiftService
    {
        private readonly IMongoCollection<Gift> _giftsCollection;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public GiftService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _giftsCollection = database.GetCollection<Gift>("gifts");

            _configuration = configuration;
            _jwtHelper = new JwtHelper(_configuration);
        }

        public async Task<ServiceResponse<Gift>> AddGift(Gift gift)
        {
            var response = new ServiceResponse<Gift>();
            try
            {
                gift.GiftId = Guid.NewGuid();
                await _giftsCollection.InsertOneAsync(gift);
                return response.SetResponse(true, "Gift created successfully", gift);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<Gift>> UpdateGift(string giftId, Gift gift)
        {
            var response = new ServiceResponse<Gift>();
            try
            {
                var update = Builders<Gift>.Update.Set(u => u.Name, gift.Name).Set(u => u.Description, gift.Description).Set(u => u.Category, gift.Category).Set(u => u.Subcategory, gift.Subcategory).Set(u => u.Price, gift.Price).Set(u => u.Availability, gift.Availability);
                var updateResult = await _giftsCollection.UpdateOneAsync(u => u.GiftId.ToString() == giftId, update);
                if (updateResult.ModifiedCount > 0)
                    return response.SetResponse(true, "Updated Gift details successfully");
                else
                    return response.SetResponse(false, "Gift not found or no changes made.");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }

        }

        public async Task<ServiceResponse<bool>> DeleteGift(string giftId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var delete = Builders<Gift>.Update.Set(u => u.IsDeleted, true);
                var deleteResult = await _giftsCollection.UpdateOneAsync(u => u.GiftId.ToString() == giftId, delete);
                if (deleteResult.ModifiedCount > 0)
                    return response.SetResponse(true, "Gift deleted successfully");
                else
                    return response.SetResponse(false, "Gift not found");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<Gift>>> GetAllGifts()
        {
            var response = new ServiceResponse<IEnumerable<Gift>>();
            try
            {
                List<Gift> gifts = await _giftsCollection.Find(_ => true).ToListAsync();
                return response.SetResponse(true, gifts);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<Gift>> GetGiftById(string giftId)
        {
            var response = new ServiceResponse<Gift>();
            try
            {
                Gift gift = await _giftsCollection.Find(u => u.GiftId.ToString() == giftId).FirstOrDefaultAsync();
                if (gift == null)
                    return response.SetResponse(false, $"Gift not found with giftId:{giftId}");
                else
                    return response.SetResponse(true, gift);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<Gift>>> GetFilteredGifts(string? category, string? subcategory, bool? availability, decimal? minPrice, decimal? maxPrice)
        {
            var response = new ServiceResponse<IEnumerable<Gift>>();
            try
            {
                var filter = Builders<Gift>.Filter.Empty;

                if (!string.IsNullOrEmpty(category))
                    filter &= Builders<Gift>.Filter.Eq(i => i.Category, category);

                if (!string.IsNullOrEmpty(subcategory))
                    filter &= Builders<Gift>.Filter.Eq(i => i.Subcategory, subcategory);

                if (availability.HasValue)
                    filter &= Builders<Gift>.Filter.Eq(i => i.Availability, availability);

                if (minPrice.HasValue)
                    filter &= Builders<Gift>.Filter.Gte(i => i.Price, minPrice);

                if (maxPrice.HasValue)
                    filter &= Builders<Gift>.Filter.Lte(i => i.Price, maxPrice);

                IEnumerable<Gift> gifts = await _giftsCollection.Find(filter).ToListAsync();
                if (gifts == null)
                    return response.SetResponse(false, "No Gifts Found");
                else
                    return response.SetResponse(true, gifts);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }
    }
}