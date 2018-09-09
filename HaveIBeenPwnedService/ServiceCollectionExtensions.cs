using HaveIBeenPwned.Service;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering the <see cref="IHaveIBeenPwnedService"/> with the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IHaveIBeenPwnedService"/> and a named <see cref="HttpClient"/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="HttpClient"/> to configure.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IHaveIBeenPwnedService"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IHaveIBeenPwnedService"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IHaveIBeenPwnedService"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, string name, Action<HttpClient> configureClient)
        {
            return services.AddHttpClient<IHaveIBeenPwnedService, HaveIBeenPwnedService>(name, configureClient);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IHaveIBeenPwnedService"/> and an <see cref="HttpClient"/>
        /// named <see cref="HaveIBeenPwnedService.DefaultName"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IHaveIBeenPwnedService"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IHaveIBeenPwnedService"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IHaveIBeenPwnedService"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, Action<HttpClient> configureClient)
        {
            return services.AddPwnedPasswordHttpClient(HaveIBeenPwnedService.DefaultName, configureClient);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IHaveIBeenPwnedService"/> and an <see cref="HttpClient"/>
        /// named <see cref="HaveIBeenPwnedService.DefaultName"/> to use the public HaveIBeenPwned API 
        /// at "https://api.pwnedpasswords.com"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IHaveIBeenPwnedService"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IHaveIBeenPwnedService"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services)
        {
            return services.AddPwnedPasswordHttpClient(HaveIBeenPwnedService.DefaultName, client =>
            {
                client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
                client.DefaultRequestHeaders.Add("User-Agent", nameof(HaveIBeenPwnedService));
            });
        }
    }
}
