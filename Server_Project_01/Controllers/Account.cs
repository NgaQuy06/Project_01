using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Server_Project_01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Account : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Xin chào người dùng!");
        }

        [HttpPost("login")] // /api/account/login
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var login = await Npg.Login(req.Username, req.Password);
                if (login != null)
                {
                    return Ok(login);
                }
                return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")] // /api/account/register
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                if (await Npg.UsernameExists(req.Username))
                {
                    return BadRequest("Tên đăng nhập đã tồn tại.");
                }

                if (await Npg.Register(req.Username, req.Password, req.Email))
                {
                    return Ok("Đăng ký thành công.");
                }

                return BadRequest("Đăng ký thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("logout")] // /api/account/logout
        //public IActionResult Logout()
        //{

        //}
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
