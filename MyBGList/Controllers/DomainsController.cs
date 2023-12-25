﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Attributes;
using MyBGList.DTO;
using MyBGList.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DomainsController : ControllerBase
    {
        private readonly ILogger<DomainsController> _logger;
        private readonly ApplicationDBContext _context;

        public DomainsController(
            ILogger<DomainsController> logger,
            ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetDomains")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        async public Task<RestDTO<Domain[]>> Get(
            [FromQuery] RequestDTO<DomainDTO> input)
        {
            var query = _context.Domains.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
            {
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            }
            var recordCount = await query.CountAsync();
            query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

            return new RestDTO<Domain[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Domains",
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

        [HttpPost(Name = "UpdateDomain")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<Domain?>> Post(DomainDTO model)
        {
            var domain = await _context.Domains
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();

            if (domain != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    domain.Name = model.Name;
                }
                domain.LastModifiedDate = DateTime.Now;
                _context.Domains.Update(domain);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Domain?>()
            {
                Data = domain,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Domains",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST"
                    ),
                }
            };
        }

        [HttpDelete(Name = "DeleteDomain")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<Domain?>> Delete(int id)
        {
            var domain = await _context.Domains
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (domain != null)
            {
                _context.Domains.Remove(domain);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Domain?>()
            {
                Data = domain,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Domains",
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
