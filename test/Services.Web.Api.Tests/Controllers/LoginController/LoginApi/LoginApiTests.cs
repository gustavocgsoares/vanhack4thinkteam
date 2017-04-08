using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Security;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.Services.Web.Api.Tests.Base;
using FluentAssertions;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.LoginController.LoginApi
{
    [Collection("TestContext")]
    public class LoginApiTests : Configuration
    {
        public LoginApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Login")]
        [Trait("Controller", "Login")]
        [Fact]
        public async Task Should_return_the_link_to_get_logged_user_data()
        {
            ////Given
            var email = GivenAValidEmail();
            var password = GivenAValidPassword();

            ////When
            var response = await WhenRequestingTheLoginApiAsync(email, password);
            var result = await WhenGetContentAsModelAsync<LoginModel>(response);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(1);
            result.Links.First().Href.Should().StartWith("http://localhost/v1/users/");
            result.Links.First().Method.Should().Be(Method.GET);
            result.Links.First().Relations.Should().Be("user_by_id");
        }
    }
}
