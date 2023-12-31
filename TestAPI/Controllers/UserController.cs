﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TestAPI.Data;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: Controller 
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
                return user;
            return NotFound();
        }

    }
}
