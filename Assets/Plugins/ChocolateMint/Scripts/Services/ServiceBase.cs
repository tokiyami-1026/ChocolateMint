using ChocolateMint.Common;

namespace ChocolateMint.Service
{
    public abstract class ServiceBase : IUpdateHandler
    {
        public virtual void Shutdown() { }

        public virtual void Update() { }

        public virtual void Startup() { }
    }
}
