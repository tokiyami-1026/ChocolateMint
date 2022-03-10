namespace ChocolateMint.Service
{
    public interface IServiceParameter<TParameter>
    {
        void PreStartup(TParameter parameter);
    }
}
