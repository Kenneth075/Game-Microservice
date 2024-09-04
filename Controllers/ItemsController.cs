using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using static Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAynce()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto()).ToList();
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> post(CreateItemDto createItem)
        {
            var item = new Items
            {
                Name = createItem.Name,
                Description = createItem.Description,
                Price = createItem.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemDto updateItem)
        {
            var existingItem = await itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItem.Name;
            existingItem.Description = updateItem.Description;
            existingItem.Price = updateItem.Price;

            await itemsRepository.UpdateAsync(existingItem);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(Guid id)
        {
            var existingItem = await itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            await itemsRepository.DeleteAsync(existingItem.Id);
            return NoContent();
        }
    }
}
