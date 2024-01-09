using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBGList.Constants;
using MyBGList.Models;

namespace MyBGList.gRPC
{
    public class GrpcService : Grpc.GrpcBase
    {
        private readonly ApplicationDBContext _context;

        public GrpcService(ApplicationDBContext context)
        {
            _context = context;
        }

        public override async Task<BoardGameResponse> GetBoardGame(
            BoardGameRequest request,
            ServerCallContext scc)
        {
            var bg = await _context.BoardGames
                .Where(bg => bg.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new BoardGameResponse();
            if (bg != null)
            {
                response.Id = bg.Id;
                response.Name = bg.Name;
                response.Year = bg.Year;
            }
            return response;
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public override async Task<BoardGameResponse> UpdateBoardGame(
            UpdateBoardGameRequest request,
            ServerCallContext scc)
        {
            var bg = await _context.BoardGames
                .Where(bg => bg.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new BoardGameResponse();
            if (bg != null)
            {
                bg.Name = request.Name;
                _context.BoardGames.Update(bg);
                await _context.SaveChangesAsync();
                response.Id = bg.Id;
                response.Name = bg.Name;
                response.Year = bg.Year;
            }
            return response;
        }

        public override async Task<DomainResponse> GetDomain(
            DomainRequest request,
            ServerCallContext scc)
        {
            var d = await _context.Domains
                .Where(d => d.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new DomainResponse();
            if (d != null)
            {
                response.Id = d.Id;
                response.Name = d.Name;
            }
            return response;
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public override async Task<DomainResponse> UpdateDomain(
            UpdateDomainRequest request,
            ServerCallContext scc)
        {
            var d = await _context.Domains
                .Where(d => d.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new DomainResponse();
            if (d != null)
            {
                d.Name = request.Name;
                _context.Domains.Update(d);
                await _context.SaveChangesAsync();
                response.Id = d.Id;
                response.Name = d.Name;
            }
            return response;
        }

        public override async Task<MechanicResponse> GetMechanic(
            MechanicRequest request,
            ServerCallContext scc)
        {
            var m = await _context.Mechanics
                .Where(m => m.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new MechanicResponse();
            if (m != null)
            {
                response.Id = m.Id;
                response.Name = m.Name;
            }
            return response;
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public override async Task<MechanicResponse> UpdateMechanic(
            UpdateMechanicRequest request,
            ServerCallContext scc)
        {
            var m = await _context.Mechanics
                .Where(m => m.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new MechanicResponse();
            if (m != null)
            {
                m.Name = request.Name;
                _context.Mechanics.Update(m);
                await _context.SaveChangesAsync();
                response.Id = m.Id;
                response.Name = m.Name;
            }
            return response;
        }
    }
}
