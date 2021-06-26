
using Play.catalog.Data.Entities;

namespace Play.catalog.Service
{
    public static class Extensions
    {
        public static Models.Item AsDto(this Item item)
        {
            return new Models.Item()
            { 
                Id = item.Id,
                CreatedDate = item.CreatedDate,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price
            };
        }
    }
}
