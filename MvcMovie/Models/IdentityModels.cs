using Microsoft.AspNet.Identity.EntityFramework;


namespace MvcMovie.Models
{

	public class ApplicationUser : IdentityUser
	{
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext() : base("DefaultConnection") { }
	}

}
