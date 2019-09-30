using System.Threading;
using System.Threading.Tasks;

namespace MobileBackend.corMonitoring
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        int exicuteEverySeconds { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}