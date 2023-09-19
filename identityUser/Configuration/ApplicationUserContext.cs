using identityUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace identityUser.Configuration
{
	public class ApplicationUserContext : IdentityUserContext<IdentityUser>
	{
		public ApplicationUserContext(DbContextOptions<ApplicationUserContext> options) : base(options)
		{ 
		
		}

		public DbSet<Make> Makes { get; set; }
		public DbSet<Model> Models { get; set; }
		public DbSet<BodyType> BodyTypes { get; set; }

	}
}
