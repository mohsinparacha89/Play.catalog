using Microsoft.AspNetCore.Mvc;
using Play.catalog.Models;
using Play.catalog.Service;
using Play.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemEntity = Play.catalog.Data.Entities;

namespace Play.catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<ItemEntity.Item> _repository;
        public ItemsController(IRepository<ItemEntity.Item> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAsync()
        {
            var items = (await _repository.GetAllAsync()).Select(i => i.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetByIdAsync(Guid id)
        {
            var item = (await _repository.GetAsync(id)).AsDto();
            return item != null ? item : NotFound("the item does not exist");
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateAsync(CreatedItem createdItem)
        {
            var item = new Data.Entities.Item()
            {
                Name = createdItem.Name,
                Price = createdItem.Price,
                Description = createdItem.Description,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdatedItem item)
        {
            var existingitem = await _repository.GetAsync(id);
            if (existingitem == null)
            {
                return NotFound();
            }

            existingitem.Name = item.Name;
            existingitem.Price = item.Price;
            existingitem.Description = item.Description;

            await _repository.UpdateAsync(existingitem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingitem = await _repository.GetAsync(id);
            if (existingitem == null)
            {
                return NotFound();
            }
            await _repository.RemoveAsync(existingitem.Id);
            return NoContent();
        }
    }

}
