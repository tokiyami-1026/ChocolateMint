namespace ChocolateMint.Service
{
    public abstract class ServiceBase
    {
        public virtual void Shutdown() { }

        public virtual void Startup() { }
    }
}
