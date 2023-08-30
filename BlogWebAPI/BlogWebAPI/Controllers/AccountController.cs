﻿using AutoMapper;
using BlogWebAPI.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    
    public class AccountController : Controller
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IMapper mapper;
        public AccountController(BlogApplicationContext context, IMapper _mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = context;
            mapper = _mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Register(string email, string password, string username)
        {
            var findResult = await _userManager.FindByEmailAsync(email);
            if (findResult!= null)
            {
                return Json("This user is already exists");
            }
            var user = new User
            {
                UserName = username,
                Email = email
            };
            var createResult = await _userManager.CreateAsync(user, password);
            var users = _userManager.Users.ToList();
            if (createResult.Succeeded) 
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callback = Url.Action("ConfirmEmail",
                    "Account",
                    new { userid = user.Id, token = token},
                    protocol: HttpContext.Request.Scheme);
                //сделать через DI
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(email, "Подтверждение", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callback}'> Подтвердить!!!!</a>");
                return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
            }
            return StatusCode(200);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest();
            }
            var users = _userManager.Users.ToList();
            var user = await _userManager.FindByIdAsync(userId); 
           
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return RedirectToAction("Blogs", "Blog");
            else
                return BadRequest();
        }
        [AllowAnonymous]
        [HttpPost]
        //public async Task<IActionResult> Login(string email, string password)
        //public async Task<IActionResult> Login([FromBody] SignInRequest request)
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            var email = request.Email;
            var password = request.Password;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }
            var normEmail = _userManager.NormalizeEmail(email);
            var user = await _userManager.FindByEmailAsync(normEmail);
            if (user == null)
            {
                return StatusCode(500);
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (result.Succeeded)
            {

                //var claims =await _signInManager.CreateUserPrincipalAsync(user);

                return Ok(new { isSuccess = true, message = "Success login" }) ;
                //return Ok(result);

            }
           
            return Unauthorized();
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
             await _signInManager.SignOutAsync();
            return RedirectToAction("Blogs", "Blog");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserClaims()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Unauthorized();
            //}
            //if (!_signInManager.IsSignedIn(claims))
            //{
            //    return Unauthorized();
            //}
            var user = this.User;
            var userClaims = User.Claims.Select(x => new { type = x.Type, value = x.Value }
            ).ToList();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return Ok(userClaims);
        }
        //public record UserClaim(string Type, string Value);
    }
    public record SignInRequest(string Email, string Password);

}
