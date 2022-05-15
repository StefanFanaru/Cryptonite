using Castle.Core.Internal;

namespace Cryptonite.IntegrationTests.Helpers
{
    public class Route
    {
        private readonly string _controller;
        private readonly string _version;

        public Route(string controller, string version = "v1.0")
        {
            _controller = controller;
            _version = version;
        }

        public string BuildEndpointRoute(string endpoint = null, string query = null)
        {
            return
                $"api/{_version}/{_controller}{(endpoint.IsNullOrEmpty() ? "" : $"/{endpoint}")}{(query.IsNullOrEmpty() ? "" : $"?{query}")}";
        }
    }
}