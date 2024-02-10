using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] UserBaseWithPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await CreateUserAsync(model, "Employee");

                if (!result)
                {
                    return BadRequest("Failed to create an Employee. Check provided data.");
                }

                return Ok(new { Message = "Employee created successfuly.", model.UserName, model.Email, model.Password });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create an Employee. Internal server error.", Error = ex.Message });
            }
        }

        [HttpPost("CreateAdministrator")]
        public async Task<IActionResult> CreateAdministratorAsync([FromBody] UserBaseWithPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await CreateUserAsync(model, "Administrator");

                if (!result)
                {
                    return BadRequest("Failed to create an Administrator. Check provided data.");
                }

                return Ok(new { Message = "Administrator created successfuly.", model.UserName, model.Email, model.Password });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create an Administrator. Internal server error.", Error = ex.Message });
            }
        }

        private async Task<bool> CreateUserAsync(UserBaseWithPasswordDto model, string roleName)
        {
            if (!roleName.Equals("Employee", StringComparison.InvariantCultureIgnoreCase) && !roleName.Equals("Administrator", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Failed to create user. Invalid role name was provided.", nameof(roleName));
            }

            var employee = new IdentityUser { UserName = model.UserName, Email = model.UserName, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true };

            var result = await this._userManager.CreateAsync(employee, model.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }

            var confirmCode = await this._userManager.GenerateEmailConfirmationTokenAsync(employee);
            var resultConfirm = await this._userManager.ConfirmEmailAsync(employee, confirmCode);

            if (!resultConfirm.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }

            await this._userManager.AddToRoleAsync(employee, roleName);

            return true;
        }

        [HttpPut("EditEmployee")]
        public async Task<IActionResult> EditEmployee(string userId, [FromBody] UserBaseWithPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await this._userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User with specified UserId was not found.");
                }

                if (!await this._userManager.IsInRoleAsync(user, "Employee"))
                {
                    return BadRequest("Error: attempted to edit not an employee.");
                }

                var result = await UpdateUserAsync(userId, model);

                if (!result)
                {
                    return BadRequest("Failed to edit the employee. Check provided data.");
                }

                return Ok(new { Message = "Successfuly edit employee data.", model.UserName, model.Email, model.Password });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to edit an Employee. Internal server error.", Error = ex.Message });
            }
        }

        [HttpPut("EditAdministrator")]
        public async Task<IActionResult> EditAdministrator(string userId, UserBaseWithPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var currentUser = await this._userManager.GetUserAsync(User);
                var userToEdit = await this._userManager.FindByIdAsync(userId);

                if (userToEdit == null)
                {
                    return NotFound("User with specified UserId was not found.");
                }

                if (!await this._userManager.IsInRoleAsync(userToEdit, "Administrator"))
                {
                    return BadRequest("You can only edit users with the role of Administrator.");
                }

                if (userToEdit.Id == currentUser.Id)
                {
                    return BadRequest("You cannot edit your own account.");
                }

                var result = await UpdateUserAsync(userId, model);

                if (!result)
                {
                    return BadRequest("Failed to edit the employee. Check provided data.");
                }

                return Ok(new { Message = "Successfuly edit administrator data.", model.UserName, model.Email, model.Password });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to edit an Administrator. Internal server error.", Error = ex.Message });
            }
        }

        private async Task<bool> UpdateUserAsync(string userId, UserBaseWithPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ArgumentNullException.ThrowIfNull(user, nameof(userId));

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to update password. {string.Join(",", result.Errors)}");
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded;
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string? id)
        {
            try
            {

                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Failed to delete the user. Id cannot be null or empty.");
                }

                var currentUser = await this._userManager.GetUserAsync(User);
                var userToDelete = await this._userManager.FindByIdAsync(id);

                if (userToDelete == null)
                {
                    return NotFound("User with specified UserId was not found.");
                }

                if (userToDelete.Id == currentUser.Id)
                {
                    return BadRequest("You cannot delete your own account.");
                }

                var result = await this._userManager.DeleteAsync(userToDelete);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User deleted successfuly");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var users = this._userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("Roles")]
        public IActionResult GetRoles()
        {
            var roles = this._context.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userRoles = await this._userManager.GetRolesAsync(user);
            return Ok(userRoles);
        }
    }
}
