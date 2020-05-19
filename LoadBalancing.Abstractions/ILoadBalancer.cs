using System.Threading.Tasks;

namespace LoadBalancing.Abstractions
{
    public interface ILoadBalancer
    {
        Task<string> get();
    }
}
