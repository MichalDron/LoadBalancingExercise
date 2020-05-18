namespace LoadBalancing.App.SampleProviders
{
    public class AlwaysAvailableProvider : SampleProvider
    {
        public AlwaysAvailableProvider(string id) : base(id, true)
        {
        }
    }
}
