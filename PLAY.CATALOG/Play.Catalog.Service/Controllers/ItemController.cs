using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController: ControllerBase
    {
        //private static readonly List<ItemDto> items = new()
        //{
        //    new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //    new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //    new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow)
        //};


        private readonly IRepository<Item> itemsRepository;

        //Dependency injection constructor
        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());

            return items;
        }

        //Get /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id) //ActionResult allows the return of multiple types not just an itemDTO
        {
            var item = await itemsRepository.GetAsync(id);

            if(item == null)
            {
                return NotFound();
            }
            else
            {
                return item.AsDto();
            }

        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> AsyncPost(CreateItemDto createItemDto)
        {
            //var item = new Item(
            //  Guid.NewGuid(),
            //  createItemDto.Name,
            //  createItemDto.Description, createItemDto.Price,
            //  DateTimeOffset.UtcNow
            //  );

            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {

            var existingItem = await itemsRepository.GetAsync(id);

            if(existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;


            await itemsRepository.UpdateAsync(existingItem);


            return NoContent();


        }

        //DELETE /items/{id}
        [HttpDelete("/{id})")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {


            await itemsRepository.RemoveAsync(id);
            return NoContent();

        }





    }
}
