namespace LoadBalancing.App.SampleProviders
{
    public class NeverAvailableProvider : SampleProvider
    {
        public NeverAvailableProvider(string id) : base(id, false)
        {
        }
    }
}
