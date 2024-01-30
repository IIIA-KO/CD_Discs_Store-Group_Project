using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest("Passwords do not match.");
                }

                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };

                var result = await this._userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var confirmCode = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                var resultConfirm = await this._userManager.ConfirmEmailAsync(user, confirmCode);

                if (!resultConfirm.Succeeded)
                {
                    return BadRequest(resultConfirm.Errors);
                }

                await this._userManager.AddToRoleAsync(user, "Client");

                await this._signInManager.SignInAsync(user, false);
                return Ok(new { Message = "Registration successful", UserName = user.UserName, Email = user.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during registration. {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var result = await this._signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok("Login successful");
                }
                else
                {
                    return BadRequest("Invalid login attempt");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during login. {ex.Message}");
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await this._signInManager.SignOutAsync();
                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during logout. {ex.Message}");
            }
        }

        [HttpGet("Users")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUsers()
        {
            var users = this._userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("Roles")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetRoles()
        {
            var roles = this._context.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpGet("GetUserRoles/{userId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await this._userManager.GetRolesAsync(user);
            return Ok(userRoles);
        }

        [HttpPost("AssignRoles/{userId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] List<string> roles)
        {
            try
            {
                var requestingUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == requestingUserId)
                {
                    return BadRequest("You are not allowed to manipulate your own account.");
                }

                var user = await this._userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var userRoles = await this._userManager.GetRolesAsync(user);
                if (userRoles.Contains("Administrator"))
                {
                    return BadRequest("You cannot change roles of other Administrators");
                }

                await this._userManager.RemoveFromRolesAsync(user, userRoles);

                if (roles == null || roles.Count == 0)
                {
                    roles = new List<string> { "Client" };
                }

                var result = await this._userManager.AddToRolesAsync(user, roles);

                return result.Succeeded
                    ? Ok($"Roles assigned successfully for user {user.UserName}")
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during role assignment. {ex.Message}");
            }
        }

        [HttpDelete("DeleteAccount")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var user = await this._userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await this._userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    await this._signInManager.SignOutAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during account deletion. {ex.Message}");
            }
        }
    }
}
