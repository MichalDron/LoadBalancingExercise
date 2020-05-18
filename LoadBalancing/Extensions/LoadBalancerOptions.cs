using System;
using LoadBalancing.Algorithms.Abstractions;
using LoadBalancing.Algorithms.RandomInvocation;

namespace LoadBalancing.Extensions
{
    public class LoadBalancerOptions
    {
        public LoadBalancerOptions()
        {
            SetDefaultInvocationAlgorithm();
        }

        private void SetDefaultInvocationAlgorithm()
        {
            InvocationAlgorithmType = typeof(RandomInvocationAlgorithm);
        }

        internal Type InvocationAlgorithmType { get; private set; }

        public void SetInvocationAlgorithm<T>() where T : IInvocationAlgorithm
        {
            InvocationAlgorithmType = typeof(T);
        }
    }
}
