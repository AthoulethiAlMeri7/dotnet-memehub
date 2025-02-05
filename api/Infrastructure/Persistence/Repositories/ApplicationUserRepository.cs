using System.Linq.Expressions;
using API.Domain.Interfaces;
using API.Domain.Models;
using API.Infrastructure.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private async Task PopulateRolesAsync(ApplicationUser? user)
        {
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Roles = roles.ToList();
            }
        }

        private async Task PopulateRolesAsync(IEnumerable<ApplicationUser> users)
        {
            foreach (var user in users)
            {
                await PopulateRolesAsync(user);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var users = await _userManager.Users
            .Where(u => u.IsDeleted == false)
            .ToListAsync();
            await PopulateRolesAsync(users);
            return users;
        }

        public async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
            await PopulateRolesAsync(user);
            return user;
        }

        public async Task<ApplicationUser> AddAsync(ApplicationUser user, string password)
        {
            user.OnPersist();
            var result = await _userManager.CreateAsync(user, password);

            return user;
        }




        public async Task<IdentityResult> UpdateAsync(ApplicationUser updatedUser)
        {
            updatedUser.OnUpdate();
            _context.Attach(updatedUser); // Attach the user to the context
            return await _userManager.UpdateAsync(updatedUser);

        }

        public async Task<IdentityResult> DeleteAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
            if (user != null)
            {
                user.PreSoftDelete();
                _context.Attach(user);
                return await _userManager.UpdateAsync(user);
            }
            return IdentityResult.Failed();
        }

        public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.IsDeleted == false);
            await PopulateRolesAsync(user);
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetByEmailAsync(string email)
        {
            var users = await _userManager.Users
            .Where(u => u.Email == email && u.IsDeleted == false)
            .ToListAsync();
            await PopulateRolesAsync(users);
            return users;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser?> GetByAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(predicate);
            await PopulateRolesAsync(user);
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetByFilterAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            var users = await _userManager.Users.Where(predicate).ToListAsync();
            await PopulateRolesAsync(users);
            return users;
        }

        //add role to user
        public async Task<IdentityResult> AddRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            await PopulateRolesAsync(users);
            return users;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}