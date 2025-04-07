using System.Text.Json.Serialization;
using FinBeatTech.Database;
using FinBeatTech.Dto;
using FinBeatTech.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SomeDataController : ControllerBase
    {
        private readonly ILogger<SomeDataController> _logger;
        private readonly AppDbContext _context;

        public SomeDataController(ILogger<SomeDataController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IResult> Save([FromBody] List<DataItemDto> items)
        {
            await _context.Database.ExecuteSqlRawAsync("delete from dataitems");
            _context.DataItems.AddRange(items.ToData());
            await _context.SaveChangesAsync();
            return Results.Ok();
        }

        [HttpGet]
        public async Task<IResult> Get(int? id, int? minCode, int? maxCode, string? value)
        {
            var query = _context.DataItems.AsNoTracking().AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }

            if (minCode.HasValue)
            {
                query = query.Where(item => item.Code >= minCode);
            }
            if (maxCode.HasValue)
            {
                query = query.Where(item => item.Code <= maxCode);
            }
            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(item => EF.Functions.ILike(item.Value,$"%{value}%"));
            }

            var result = await query.ToListAsync();
            return Results.Json(result.ToDto());
        }
    }
}
