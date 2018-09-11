﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HaveIBeenPwned.Service
{
    /// <inheritdoc />
    public class HaveIBeenPwnedService : IHaveIBeenPwnedService
    {
        /// <summary>
        /// The default name used to register the typed HttpClient with the <see cref="IServiceCollection"/>
        /// </summary>
        public const string DefaultName = "HaveIBeenPwnedService";

        HttpClient _client;
        ILogger<HaveIBeenPwnedService> _logger;

        /// <summary>
        /// Create a new instance of <see cref="HaveIBeenPwnedService"/>
        /// </summary>
        public HaveIBeenPwnedService(HttpClient client, ILogger<HaveIBeenPwnedService> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<(bool isPwned, int frequency)> HasPasswordBeenPwned(string password, CancellationToken cancellationToken = default)
        {
            var sha1Password = SHA1Util.SHA1HashStringForUTF8String(password);
            var sha1Prefix = sha1Password.Substring(0, 5);
            var sha1Suffix = sha1Password.Substring(5);
            try
            {
                HttpResponseMessage response = await _client.GetAsync("range/" + sha1Prefix, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    // Response was a success. Check to see if the SAH1 suffix is in the response body.
                    var result = await Contains(response.Content, sha1Suffix);
                    if (result.isPwned)
                    {
                        _logger.LogDebug("HaveIBeenPwned API indicates the password has been pwned");
                    }
                    else
                    {
                        _logger.LogDebug("HaveIBeenPwned API indicates the password has not been pwned");
                    }
                    return result;
                }
                _logger.LogWarning("Unexepected response from API: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Pwned Password API. Assuming password is not pwned");
            }
            return (false, 0);
        }

        internal static async Task<(bool isPwned, int frequency)> Contains(HttpContent content, string sha1Suffix)
        {
            using (var streamReader = new StreamReader(await content.ReadAsStreamAsync()))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    var segments = line.Split(':');
                    if (segments.Length == 2
                        && string.Equals(segments[0], sha1Suffix, StringComparison.OrdinalIgnoreCase)
                        && int.TryParse(segments[1], out var count))
                    {
                        return (true, count);
                    }
                }
            }

            return (false, 0);

        }

    }
}
