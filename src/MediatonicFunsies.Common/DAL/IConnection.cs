using StackExchange.Redis;

namespace MediatonicFunsies.Common.DAL
{
    public interface IConnection
    {
        IDatabase GetDatabase();
    }
}