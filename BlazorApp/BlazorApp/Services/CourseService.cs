using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BlazorApp.Services
{
    public class CourseService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                         NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider,
                         MessageService messageService)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly MessageService _messageService = messageService;

        public string? MessageText { get; set; }
		public string? MessageType { get; set; }
		public string? ReturnUrl { get; set; }
		public async Task JoinCourseAsync(string courseId)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                MessageText = "User not logged in!";
                MessageType = "danger";
                return;
            }

            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                MessageText = "User ID not found!";
                MessageType = "danger";
                return;
            }

            try
            {
                var userCourse = new UserCourses
                {
                    UserId = userId,
                    CourseId = courseId
                };
                var existingUserCourse = await _context.UserCourses
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

                if (existingUserCourse == null)
                {
                    _context.UserCourses.Add(userCourse);
                    await _context.SaveChangesAsync();

                    // Send confirmation
                    await _messageService.SendMessageAsync("message_request", user.Identity.Name!);

                    MessageText = "Your request to join sent successfully! Soon you will get a confirmation message in your email!";
                    MessageType = "success";
                }else
                {
                    MessageText = "Your already registrated to this course!";
                    MessageType = "warning";
                }
                
            }
            catch (Exception ex)
            {
                MessageText = "Something went wrong, please try later!";
                MessageType = "danger";
                Console.WriteLine(ex.ToString());
            }
        }

		public async Task<List<string>> GetUserCourseIdsAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user == null || !user.Identity!.IsAuthenticated)
			{
				MessageText = "User not logged in!";
				MessageType = "danger";
                return null!;
			}

			var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			try
			{
				var courseIds = await _context.UserCourses
					.Where(uc => uc.UserId == userId)
					.Select(uc => uc.CourseId)
					.ToListAsync();

				return courseIds;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving user course IDs: {ex}");
			}
			return null!;
		}
		
    }
}
