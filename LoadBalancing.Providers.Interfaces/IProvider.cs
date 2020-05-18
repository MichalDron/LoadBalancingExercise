namespace LoadBalancing.Providers.Abstractions
{
    public interface IProvider
    {
        string Id { get; }

        string get();

        bool check();
    }
}
