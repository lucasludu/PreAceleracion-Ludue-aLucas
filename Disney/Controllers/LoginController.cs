using Disney.Auth.Request;
using Disney.Auth.Response;
using Disney.SendGrid;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Microsoft.AspNetCore.Mvc;

namespace Disney.Controllers
{
    [Route("auth/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiUserDisney")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uowork;

        public LoginController(IUserService userService, IUnitOfWork uowork)
        {
            _userService = userService;
            _uowork = uowork;
        }

        /// <summary>
        /// User Authentication
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginRequest userRequest)
        {
            var response = _userService.Login(userRequest);
            if (response == null)
            {
                return Unauthorized();
            }
            var token = _userService.GetToken(response);
            return Ok(new
            {
                token = token,
                user = response
            });
        }

        /// <summary>
        /// New User Registration
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterRequest userRequest)
        {
            if (_uowork.UserRepo.UserExists(userRequest.Email.ToLower()))
            {
                return BadRequest("Existing Email");
            }
            UserResponse response = _userService.Register(userRequest, userRequest.Password);
            await EmailSender.SendWelcomeEmail(userRequest.Email);
            return Ok(response);
        }

    }
}
