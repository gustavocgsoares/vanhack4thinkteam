using Farfetch.Services.Web.Api.Tests.Base;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.Base
{
    [CollectionDefinition("TestContext")]
    public class TestCollection : ICollectionFixture<CollectionFixture>
    {
        public TestCollection()
        {
        }
    }
}
