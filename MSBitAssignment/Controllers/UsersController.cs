using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repository;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace MSBitAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _repository;

        public UsersController(UsersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> list =  await _repository.GetAllUsers();

            return list;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            User foundUser = await _repository.GetUser(id);
            if (foundUser == null)
            {
                return NotFound();
            }
            return Ok(foundUser);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user.FirstName.Length <= 0 || user.FirstName.Length >= 20
                || user.LastName.Length <= 0 || user.LastName.Length >= 80 
                || user.Password == null)
            {
                return BadRequest();
            }

            await _repository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }

            User affectedUser = await _repository.UpdateUserAsync(id, user);
            if (affectedUser == null)
            {
                return NotFound("The User couldn't be found.");
            }

            return Ok(affectedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if(id == null) return NotFound();
            
            bool deleted = await _repository.DeleteUserAsync(id);

            if(deleted) return Ok();
            else return NotFound();
        }

        [HttpGet("SearchUser")]
        public async Task<IEnumerable<User>> SearchUser([FromQuery]string querystring)
        {
            if(String.IsNullOrEmpty(querystring)) return null;

            var searchResult = await _repository.SearchUser(querystring);
            return searchResult;
        }
    }
}
