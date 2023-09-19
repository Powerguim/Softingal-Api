using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softingal_Api.data;
using System.Net;

namespace Softingal_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly DataContext _context;

        public AdressesController(DataContext context)
        {
            _context = context;
        }


        //Get addresses
        [HttpGet]
        public async Task<ActionResult<List<Address>>> GetAddresses()
        {
            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Address>>> CreateAddress(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Address>>> UpdateAddress(Address address)
        {
            var dbAddress = await _context.Addresses.FindAsync(address.Id);

            if (dbAddress == null)
                return BadRequest("Address was not found!");

            dbAddress.Name = address.Name;

            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Address>>> DeleteAddress(int id)
        {
            var dbAddress = await _context.Addresses.FindAsync(id);

            if (dbAddress == null)
                return BadRequest("Address was not found!");

            _context.Addresses.Remove(dbAddress);
            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

    }
}
