using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string? UserProfileId { get; set; }
		public virtual UserProfile? UserProfile { get; set; }
		public string? UserAddressId { get; set; }
		public virtual UserAddress? UserAddress { get; set; }
	}


}
