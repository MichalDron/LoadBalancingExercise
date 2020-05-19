using System.Threading.Tasks;

namespace LoadBalancing.Providers.Abstractions
{
    public interface IProvider
    {
        string Id { get; }

        Task<string> get();

        bool check();
    }
}
