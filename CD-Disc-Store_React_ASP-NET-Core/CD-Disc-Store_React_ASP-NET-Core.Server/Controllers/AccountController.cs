using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels.Account;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IClientRepository _clientRepository;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IClientRepository clientRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._clientRepository = clientRepository;
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

                var user = new IdentityUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true };

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

                var client = new Client
                {
                    UserId = user.Id,
                    Address = model.Address,
                    City = model.City,
                    BirthDay = model.BirthDay,
                    MarriedStatus = model.MarriedStatus,
                    Sex = model.Sex,
                    HasChild = model.HasChild,
                };

                await this._clientRepository.AddAsync(client);

                return Ok(new { Message = "Registration successful", UserName = user.UserName, Email = user.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during registration. {ex.Message}");
            }
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] EditViewModel model)
        {
            try
            {
                var user = await this._userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (!ValidateUserInfo(model))
                {
                    return BadRequest(ModelState);
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                var result = await this._userManager.UpdateAsync(user);

                return result.Succeeded
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during changing username. {ex.Message}");
            }
        }

        private bool ValidateUserInfo(EditViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || model.UserName.Length > 50)
            {
                return false;
            }

            var phoneAttribute = new PhoneAttribute();
            if (!phoneAttribute.IsValid(model.PhoneNumber))
            {
                ModelState.AddModelError("Contact Phone", "Invalid phone number format.");
                return false;
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(model.Email))
            {
                ModelState.AddModelError("Contact Mail", "Invalid email format.");
                return false;
            }

            return true;
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

        [HttpDelete("DeleteAccount")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var user = await this._userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var relatedClient = await this._clientRepository.GetByUserIdAsync(user.Id);
                await this._clientRepository.DeleteAsync(relatedClient.Id);

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
            catch (DatabaseOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during account deletion. {ex.Message}");
            }
        }
    }
}
