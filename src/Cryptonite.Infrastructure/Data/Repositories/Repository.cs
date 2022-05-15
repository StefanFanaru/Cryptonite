namespace Cryptonite.Infrastructure.Data.Repositories
{
    public class Repository : EfRepository<CryptoniteContext>
    {
        public Repository(CryptoniteContext context) : base(context)
        {
        }
    }
}