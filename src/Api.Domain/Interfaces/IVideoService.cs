using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.ViewModels;

namespace Api.Domain.Interfaces
{
    public interface IVideoService
    {
        string GetTwilioJwt(string identity);
        Task<IEnumerable<RoomDetailsViewModel>> GetAllRoomsAsync();
    }
}
