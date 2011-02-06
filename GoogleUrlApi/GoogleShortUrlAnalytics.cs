using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleUrlApi
{
    public class GoogleShortUrlAnalytics : GoogleShortUrlStatus
    {
        public DateTime Created { get; set; }
        public GoogleShortUrlAnalyticsDetails Details { get; set; }
    }

    public class GoogleShortUrlAnalyticsDetails
    {
        public GoogleShortUrlAnalyticsDetailsFull AllTime { get; set; }
        public GoogleShortUrlAnalyticsDetailsFull Month { get; set; }
        public GoogleShortUrlAnalyticsDetailsFull Week { get; set; }
        public GoogleShortUrlAnalyticsDetailsFull Day { get; set; }
        public GoogleShortUrlAnalyticsDetailsFull TwoHours { get; set; }
    }

    public class GoogleShortUrlAnalyticsDetailsFull
    {
        public int ShortUrlClicks { get; set; }
        public int LongUrlClicks { get; set; }
        public IEnumerable<GoogleCountIDPair> Referrers { get; set; }
        public IEnumerable<GoogleCountIDPair> Countries { get; set; }
        public IEnumerable<GoogleCountIDPair> Browsers { get; set; }
        public IEnumerable<GoogleCountIDPair> Platforms { get; set; }
    }

    public class GoogleCountIDPair
    {
        public int Count { get; set; }
        public string Id { get; set; }
    }
}
