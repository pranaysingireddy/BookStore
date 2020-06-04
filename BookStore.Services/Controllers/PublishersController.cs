﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using BookStore.Services.Repos;

namespace BookStore.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly BookStoreContext _context;
        private readonly IAuthRepo _authRepo;

        public PublisherController(BookStoreContext context, IAuthRepo authRepo)
        {
            _context = context;
            _authRepo = authRepo;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            //var token = Request.Headers["Authorization"];
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var userDetails = _authRepo.GetUserFromAccessToken(accessToken).Result;


            return await _context.Publisher.ToListAsync();
        }

        // GET: api/Publishers/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            //var token = Request.Headers["Authorization"];
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var userDetails = _authRepo.GetUserFromAccessToken(accessToken).Result;



            var publisher = await _context.Publisher.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Publishers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publisher.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publisher.Any(e => e.PubId == id);
        }
    }
}
