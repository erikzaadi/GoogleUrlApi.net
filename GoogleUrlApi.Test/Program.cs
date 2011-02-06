using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleUrlApi.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var googleShorter = new GoogleUrlApi("PUT_YOUR_LONG_KEY_HERE");

            var urlToPlayWith = "http://www.google.com";

            var shortUrl = googleShorter.Shorten(urlToPlayWith);

            var longUrl = googleShorter.Expand(shortUrl);

            var longUrlDetails = googleShorter.ExpandFull(shortUrl);

            var analytics = googleShorter.GetAnalytics(shortUrl);

        }
    }
}
