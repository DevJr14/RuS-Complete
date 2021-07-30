using RuS.Application.Responses.Identity;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuS.Application.Interfaces.Chat;
using RuS.Application.Models.Chat;

namespace RuS.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}