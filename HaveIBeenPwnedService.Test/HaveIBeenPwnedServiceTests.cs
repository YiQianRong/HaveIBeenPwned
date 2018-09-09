using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HaveIBeenPwned.Service;

namespace HaveIBeenPwned.Service.Test
{
    public class HaveIBeenPwnedServiceTests
    {
        [Fact, Trait("Category", "Integration")] 
        public async Task HasPasswordBeenPwned_WhenWeakPassword_ReturnsTrue()
        {
            HaveIBeenPwned.Service.HaveIBeenPwnedService service = GetClient();

            var pwnedPassword = "password";

            var resp = await service.HasPasswordBeenPwned(pwnedPassword);

            Assert.True(resp.isPwned, "Checking for Pwned password should return true");
        }

        private static HaveIBeenPwnedService GetClient()
        {
            var services = new ServiceCollection();
            services.AddPwnedPasswordHttpClient();
            var provider = services.BuildServiceProvider();

            //all called in one method to easily enforce timout

            var service = new HaveIBeenPwned.Service.HaveIBeenPwnedService(
                provider.GetService<IHttpClientFactory>().CreateClient(HaveIBeenPwned.Service.HaveIBeenPwnedService.DefaultName),
                MockHelpers.StubLogger<HaveIBeenPwned.Service.HaveIBeenPwnedService>());
            return service;
        }
    }
}
