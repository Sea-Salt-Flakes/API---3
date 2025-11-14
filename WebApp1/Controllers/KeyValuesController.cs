using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp1.Data;
using WebApp1.Models;
using WebApp1.DTOs;

namespace WebApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeyValuesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<KeyValuesController> _logger;

        public KeyValuesController(AppDbContext db, ILogger<KeyValuesController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/KeyValues
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.KeyValues.AsNoTracking().ToListAsync();
            var dtos = list.Select(k => new KeyValueDto(k.Id, k.Key, k.Value, k.Description));
            return Ok(dtos);
        }

        // GET: api/KeyValues/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.KeyValues.FindAsync(id);
            if (item == null) return NotFound(new { message = "Item not found" });
            return Ok(new KeyValueDto(item.Id, item.Key, item.Value, item.Description));
        }

        // POST: api/KeyValues
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KeyValueCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var exists = await _db.KeyValues.AnyAsync(x => x.Key == dto.Key);
                if (exists) return Conflict(new { message = "Key already exists" });

                var item = new KeyValueItem
                {
                    Key = dto.Key,
                    Value = dto.Value,
                    Description = dto.Description
                };
                _db.KeyValues.Add(item);
                await _db.SaveChangesAsync();

                var result = new KeyValueDto(item.Id, item.Key, item.Value, item.Description);
                return CreatedAtAction(nameof(Get), new { id = item.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating KeyValue");
                return Problem(detail: ex.Message, title: "Create failed", statusCode: 500);
            }
        }

        // PUT: api/KeyValues/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] KeyValueUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest(new { message = "Id mismatch" });

            var item = await _db.KeyValues.FindAsync(id);
            if (item == null) return NotFound(new { message = "Item not found" });

            try
            {
                item.Key = dto.Key;
                item.Value = dto.Value;
                item.Description = dto.Description;

                await _db.SaveChangesAsync();

                return Ok(new KeyValueDto(item.Id, item.Key, item.Value, item.Description));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency issue updating id {Id}", id);
                return Conflict(new { message = "Concurrency error" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating KeyValue");
                return Problem(detail: ex.Message, title: "Update failed", statusCode: 500);
            }
        }

        // DELETE: api/KeyValues/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.KeyValues.FindAsync(id);
            if (item == null) return NotFound(new { message = "Item not found" });

            try
            {
                _db.KeyValues.Remove(item);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting KeyValue id {Id}", id);
                return Problem(detail: ex.Message, title: "Delete failed", statusCode: 500);
            }
        }
    }
}
