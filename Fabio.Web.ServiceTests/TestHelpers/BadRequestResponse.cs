using System.Collections.Generic;

namespace Fabio.Web.ServiceTests.TestHelpers
{
    public class BadRequestResponse
    {
        public Dictionary<string, string[]> Errors { get; set; }
    }
}