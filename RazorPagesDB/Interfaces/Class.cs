namespace RazorPagesDB.Interfaces
{
    public abstract class IDbInitializer
    {
        public abstract void Initialize(IServiceProvider serviceProvider);
    }
}

