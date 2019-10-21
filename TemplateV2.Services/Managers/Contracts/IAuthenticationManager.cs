using System.Threading.Tasks;

namespace TemplateV2.Services.Managers.Contracts
{
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Signs user into current http context. Note: this dehydrates the session.
        /// </summary>
        /// <param name="sessionGuid"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task SignIn(int sessionId);

        /// <summary>
        /// Signs user out of current http context and flushes session.
        /// </summary>
        /// <returns></returns>
        Task SignOut();
    }
}
