namespace Cryptonite.Core.Common
{
    public static class AppHelpers
    {
#pragma warning disable S3400
        public static bool IsDevelopment()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
#pragma warning restore  S3400
    }
}