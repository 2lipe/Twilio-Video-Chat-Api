using System.Threading.Tasks;
using Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoChatController : ControllerBase
    {
        readonly IVideoService _videoService;

        public VideoChatController(IVideoService videoService)
            => _videoService = videoService;

        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var result = new JsonResult(new { token = _videoService.GetTwilioJwt(User.Identity.Name) });

            return result;
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var result = new JsonResult(await _videoService.GetAllRoomsAsync());

            return result;
        }
    }
}
