using System.Threading;
using System.Threading.Tasks;

namespace HaveIBeenPwned.Service
{
    /// <summary>
    /// A client for communicating with HasPasswordBeenPwned API
    /// </summary>
    public interface IHaveIBeenPwnedService
    {
        /// <summary>
        /// Checks if a provided password has appeared in a known data breach
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <param name="cancellationToken">An optional cancellation token</param>
        /// <returns></returns>
        Task<(bool isPwned, int frequency)> HasPasswordBeenPwned(string password, CancellationToken cancellationToken = default);
    }
}