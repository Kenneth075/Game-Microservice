using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new List<ItemDto>{
            new ItemDto(Guid.NewGuid(), "Potion","Restore a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote","Cure poison",10,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword","Deals a small amount of demage",20,DateTimeOffset.UtcNow),

        };

        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItemById(Guid id)
        {
            var item = items.Where(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> post(CreateItemDto createItem)
        {
            var item = new ItemDto(Guid.NewGuid(), createItem.Name, createItem.Description, createItem.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult update(Guid id, UpdateItemDto updateItem)
        {
            var existingItem = items.Where(x => x.Id == id).FirstOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }

            var newItem = existingItem with
            {
                Name = updateItem.Name,
                Description = updateItem.Description,
                Price = updateItem.Price,
            };
            var index = items.FindIndex(x => x.Id == id);
            items[index] = newItem;
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult delete(Guid id)
        {
            var index = items.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent();
        }
    }
}
