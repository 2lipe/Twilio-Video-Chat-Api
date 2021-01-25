using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Settings;
using Api.Domain.Interfaces;
using Api.Domain.ViewModels;
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;

namespace Api.Service
{
    public class VideoService : IVideoService
    {
        readonly TwilioSettings _twilioSettings;

        public VideoService(Microsoft.Extensions.Options.IOptions<TwilioSettings> options)
        {
            var twilioOptions = options.Value ?? throw new ArgumentNullException(nameof(options));

            _twilioSettings = twilioOptions;

            TwilioClient.Init(_twilioSettings.ApiKey, _twilioSettings.ApiSecret);
        }

        public string GetTwilioJwt(string identity)
            => new Token(_twilioSettings.AccountSid,
                        _twilioSettings.ApiKey,
                        _twilioSettings.ApiSecret,
                        identity ?? Guid.NewGuid().ToString(),
                        grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();

        async Task<RoomDetailsViewModel> GetRoomDetailsAsync(RoomResource room,
            Task<ResourceSet<ParticipantResource>> participantTask)
        {
            try
            {
                var participants = await participantTask;

                var result = new RoomDetailsViewModel
                {
                    Name = room.UniqueName,
                    MaxParticipants = room.MaxParticipants ?? 0,
                    ParticipantCount = participants.ToList().Count
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<RoomDetailsViewModel>> GetAllRoomsAsync()
        {
            try
            {
                var rooms = await RoomResource.ReadAsync();

                var tasks = rooms.Select(
                    room => GetRoomDetailsAsync(
                        room,
                        ParticipantResource.ReadAsync(
                            room.Sid,
                            ParticipantStatus.Connected)));

                var result = await Task.WhenAll(tasks);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
