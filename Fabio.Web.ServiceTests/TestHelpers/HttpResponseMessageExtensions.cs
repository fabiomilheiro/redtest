using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Xunit.Sdk;

namespace Fabio.Web.ServiceTests.TestHelpers
{
    public static class HttpResponseMessageExtensions
    {
        public static void ShouldHaveStatusCode(
            this HttpResponseMessage response,
            HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode != expectedStatusCode)
            {
                throw new XunitException(
                    $"Expected a status code '{expectedStatusCode}' but got '{response.StatusCode}' with body:" +
                    $"{Environment.NewLine}{response.Content.ReadAsStringAsync().Result}");
            }
        }

        public static void ShouldHaveBody<TObject>(
            this HttpResponseMessage response,
            TObject expectation)
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;

            TObject result;

            try
            {
                result = Deserialize<TObject>(responseBody);
            }
            catch (JsonException)
            {
                throw new XunitException(
                    "The response body does not contain a response with the expected format:" +
                    $"{Environment.NewLine}{responseBody}");
            }

            result.Should().BeEquivalentTo(expectation);
        }

        public static void ShouldHaveError(
            this HttpResponseMessage response,
            string key,
            string errorMessage)
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;

            BadRequestResponse badRequestResponse;

            try
            {
                badRequestResponse = Deserialize<BadRequestResponse>(responseBody);
            }
            catch (JsonException)
            {
                throw new XunitException(
                    "The response body does not contain a response with the expected errors format:" +
                    $"{Environment.NewLine}{responseBody}");
            }

            var found = badRequestResponse
                .Errors
                .Any(error =>
                    string.Equals(error.Key, key, StringComparison.OrdinalIgnoreCase)
                    && error.Value.Any(m => m.Contains(errorMessage, StringComparison.OrdinalIgnoreCase)));

            if (!found)
            {
                throw new XunitException(
                    $"The error message ('{key}': '{errorMessage}') was not found." +
                    $"Response: {Environment.NewLine}{responseBody}");
            }
        }

        private static TObject Deserialize<TObject>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonSerializer
                .Deserialize<TObject>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }
    }
}