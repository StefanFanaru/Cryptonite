using Humanizer.Inflections;

namespace Cryptonite.Infrastructure.Common
{
    public static class HumanizerInitializer
    {
        public static void Initialize()
        {
            Vocabularies.Default.AddPlural("User story", "User stories");
        }
    }
}