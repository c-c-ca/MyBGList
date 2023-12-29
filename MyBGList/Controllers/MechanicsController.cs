﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MyBGList.Attributes;
using MyBGList.DTO;
using MyBGList.Extensions;
using MyBGList.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MechanicsController : ControllerBase
    {
        private readonly ILogger<MechanicsController> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IDistributedCache _distributedCache;

        public MechanicsController(
            ILogger<MechanicsController> logger,
            ApplicationDBContext context,
            IDistributedCache distributedCache)
        {
            _logger = logger;
            _context = context;
            _distributedCache = distributedCache;
        }

        [HttpGet(Name = "GetMechanics")]
        [ResponseCache(CacheProfileName = "Any-60")]
        async public Task<RestDTO<Mechanic[]>> Get(
            [FromQuery] RequestDTO<MechanicDTO> input)
        {
            var query = _context.Mechanics.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
            {
                query = query.Where(m => m.Name.Contains(input.FilterQuery));
            }

            var recordCount = await query.CountAsync();

            Mechanic[]? result = null;
            var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";
            if (!_distributedCache.TryGetValue<Mechanic[]>(cacheKey, out result))
            {
                query = query
                    .OrderBy($"{input.SortColumn} {input.SortOrder}")
                    .Skip(input.PageIndex * input.PageSize)
                    .Take(input.PageSize);
                result = await query.ToArrayAsync();
                _distributedCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
            }

            return new RestDTO<Mechanic[]>()
            {
                Data = result,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Mechanics",
                            new {
                                input.PageIndex,
                                input.PageSize,
                                input.SortColumn,
                                input.SortOrder,
                                input.FilterQuery
                            },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateMechanic")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<RestDTO<Mechanic?>> Post(MechanicDTO model)
        {
            var mechanic = await _context.Mechanics
                .Where(m => m.Id == model.Id)
                .FirstOrDefaultAsync();

            if (mechanic != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    mechanic.Name = model.Name;
                }
                mechanic.LastModifiedDate = DateTime.Now;
                _context.Mechanics.Update(mechanic);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Mechanic?>()
            {
                Data = mechanic,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Mechanics",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST"
                    ),
                }
            };
        }

        [HttpDelete(Name = "DeleteMechanic")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<RestDTO<Mechanic?>> Delete(int id)
        {
            var mechanic = await _context.Mechanics
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (mechanic != null)
            {
                _context.Mechanics.Remove(mechanic);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Mechanic?>()
            {
                Data = mechanic,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Mechanics",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE"
                    ),
                }
            };
        }
    }
}
