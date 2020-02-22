using DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UsersRepository
    {
        private readonly UsersContext _context;
        private readonly PasswordHasher<User> _userPasswordHasher;
        public UsersRepository(UsersContext context)
        {
            _context = context;
            _userPasswordHasher = new PasswordHasher<User>();
        }

        private string UserPasswordHasher(User user, string password)
        {
            return _userPasswordHasher.HashPassword(user, password);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var allUsers = await _context.User.ToListAsync();
            return allUsers;
        }

        public async Task<User> GetUser(int id)
        {
            User user = await _context.User.FirstOrDefaultAsync(e => e.Id == id);
            return user;
        }

        public async Task<int> AddUserAsync(User user)
        {
            string hasedPassword = UserPasswordHasher(user, user.Password);
            user.Password = hasedPassword;

            _context.User.Add(user);
            int rowsAffected = 0;
            rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var userToUpdate = await GetUser(id);

            if (userToUpdate == null) return null;

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Gender = user.Gender;
            userToUpdate.Email = user.Email;

            if (user.Password != null)
            {
                string hasedPassword = UserPasswordHasher(userToUpdate, user.Password);
                userToUpdate.Password = hasedPassword;
            }

            try
            {
                await _context.SaveChangesAsync();
                return userToUpdate;
            } catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> DeleteUserAsync(int? id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null) return false;

            try
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> SearchUser(string querystring)
        {
            var searchResult = await _context.User.Where(user => user.FirstName.Contains(querystring)
            || user.LastName.Contains(querystring)
            || user.Email.Contains(querystring)).ToListAsync();

            return searchResult;
        }
    }
}
