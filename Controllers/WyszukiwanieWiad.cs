using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quark_Backend.DAL;
using Quark_Backend.Entities;
using Quark_Backend.Models;
using Quark_Backend.Services;

namespace Quark_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WyszukiwanieWiad : Controller
    {
        private readonly QuarkDbContext _dbContext;
        public WyszukiwanieWiad(QuarkDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> WyszukajWiadomosc([FromBody] WyszukiwanieWiadomosciRequest szukanaWiadomosc) 
        {
            try
            {
                var query = _dbContext.Messages.AsQueryable();
                query = query.Where(m => m.ConversationId == szukanaWiadomosc.ConversationId);


                if (!string.IsNullOrEmpty(szukanaWiadomosc.FragmentWiadomosci))
                {
                    query = query.Where(m => m.Text.ToLower().Contains(szukanaWiadomosc.FragmentWiadomosci.ToLower()));
                }

                if (szukanaWiadomosc.UserId.HasValue)
                {
                    query = query.Where(m => m.UserId == szukanaWiadomosc.UserId);
                }

                if (szukanaWiadomosc.SentDate.HasValue)
                {
                    query = query.Where(m => m.SentDate == szukanaWiadomosc.SentDate);
                }

                var wyszukaneWiadomosci = await query.ToListAsync();
                // Moze ograniczyc ilosc wiadomosci, np. w miare mozliwosci przesylac mniejsze listy co klikniecie
                if (wyszukaneWiadomosci.Any())
                {
                    var wyniki = wyszukaneWiadomosci.Select(m => new
                    {
                        Id = m.Id,
                        Wiadomosc = m
                    });

                    return Ok(wyniki);
                }
                else
                {
                    return NotFound("Nie znaleziono pasujących wiadomości.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}

