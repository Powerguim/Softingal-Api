using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using Softingal_Api.data;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace Softingal_Api.Controllers
{
    [Authorize]
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
            // Get the user's ID from the claims in the JWT token
            var subClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // "sub" claim
            var userId = subClaim?.Value;


            // Check if userId is null or empty
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Unauthorized (The dev at least protected the user addresses :D ).");
            }

            // Retrieve addresses for the authenticated user
            var userAddresses = await _context.Addresses
                .Where(address => address.UserId == int.Parse(userId))
                .ToListAsync();

            return Ok(userAddresses);
        }

        [HttpPost]
        public async Task<ActionResult<List<Address>>> CreateAddress(Address address)
        {
            // Get the user's ID from the claims in the JWT token
            var subClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // "sub" claim
            var userId = subClaim?.Value;


            // Check if userId is null or empty
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Unauthorized (The dev at least protected the user addresses :D ).");
            }

            // Set the UserId property of the new address
            address.UserId = int.Parse(userId);

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Address>>> UpdateAddress(Address address)
        {
            var dbAddress = await _context.Addresses.FindAsync(address.Id);

            // Get the user's ID from the claims in the JWT token
            var subClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // "sub" claim
            var userId = subClaim?.Value;


            if (dbAddress == null)
                return BadRequest("Address was not found!");

            if(int.Parse(userId) != dbAddress.UserId)
                return BadRequest("Not your address!");

            dbAddress.Name = address.Name;

            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Address>>> DeleteAddress(int id)
        {
            var dbAddress = await _context.Addresses.FindAsync(id);

            // Get the user's ID from the claims in the JWT token
            var subClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // "sub" claim
            var userId = subClaim?.Value;


            if (dbAddress == null)
                return BadRequest("Address was not found!");

            if (int.Parse(userId) != dbAddress.UserId)
                return BadRequest("Not your address!");


            if (dbAddress == null)
                return BadRequest("Address was not found!");

            _context.Addresses.Remove(dbAddress);
            await _context.SaveChangesAsync();

            return Ok(await _context.Addresses.ToListAsync());
        }

    }
}
