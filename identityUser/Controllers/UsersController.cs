﻿using identityUser.Models;
using identityUser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace identityUser.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly JwtService _jwtService;


		public UsersController(
			UserManager<IdentityUser> userManager, JwtService jwtService
		)
		{
			_userManager = userManager;
			_jwtService = jwtService;
		}

		// POST: api/Users
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _userManager.CreateAsync(
				new IdentityUser() { UserName = user.UserName, Email = user.Email },
				user.Password
			);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			user.Password = null;
			return Created("", user);
		}

		// GET: api/Users/username
		[HttpGet("{username}")]
		public async Task<ActionResult<User>> GetUser(string username)
		{
			IdentityUser user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				return NotFound();
			}

			return new User
			{
				UserName = user.UserName,
				Email = user.Email
			};
		}

		// POST: api/Users/BearerToken
		[HttpPost("BearerToken")]
		public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Bad credentials");
			}

			var user = await _userManager.FindByNameAsync(request.UserName);

			if (user == null)
			{
				return BadRequest("Bad credentials");
			}

			var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

			if (!isPasswordValid)
			{
				return BadRequest("Bad credentials");
			}

			var token = _jwtService.CreateToken(user);

			return Ok(token);
		}
	}
}
