using RuS.Application.Models.Chat;
using RuS.Application.Responses.Identity;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuS.Application.Interfaces.Chat;

namespace RuS.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
    }
}