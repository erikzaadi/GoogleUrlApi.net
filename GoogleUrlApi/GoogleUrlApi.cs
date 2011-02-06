using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;

namespace GoogleUrlApi
{
    public class GoogleUrlApi
    {
        private const string BaseGoogleUrl = "https://www.googleapis.com/urlshortener/v1/url?key={0}{1}";

        public string GoogleApiKey { get; set; }

        public GoogleUrlApi() { }
        public GoogleUrlApi(string googleApiKey) { GoogleApiKey = googleApiKey; }

        public string Shorten(string LongUrl)
        {
            using (var webClient = new WebClient())
            {

                var jsonString = DynamicToJson.Serialize(new { longUrl = LongUrl });

                var url = GetApiUrl("&longUrl=" + LongUrl);

                webClient.Headers.Add("Content-Type", "application/json");

                var result = webClient.UploadString(url, jsonString);

                dynamic jsonObj = DynamicToJson.Parse(result);

                return jsonObj.id as string;
            }
        }

        public string Expand(string ShortUrl)
        {
            return ExpandFull(ShortUrl).LongUrl;
        }

        public GoogleShortUrlStatus ExpandFull(string ShortUrl)
        {
            using (var webClient = new WebClient())
            {
                var url = GetApiUrl("&shortUrl=" + ShortUrl);

                var result = webClient.DownloadString(url);

                dynamic jsonObj = DynamicToJson.Parse(result);

                return new GoogleShortUrlStatus
                {
                    Id = jsonObj.id as string,
                    Kind = jsonObj.kind as string,
                    LongUrl = jsonObj.longUrl as string,
                    Status = jsonObj.status as string
                };
            }
        }

        public GoogleShortUrlAnalytics GetAnalytics(string ShortUrl)
        {
            using (var webClient = new WebClient())
            {

                var url = GetApiUrl(string.Format("&shortUrl={0}&projection=FULL", ShortUrl));

                var result = webClient.DownloadString(url);

                dynamic jsonObj = DynamicToJson.Parse(result);

                return new GoogleShortUrlAnalytics
                {
                    Id = jsonObj.id as string,
                    Kind = jsonObj.kind as string,
                    LongUrl = jsonObj.longUrl as string,
                    Status = jsonObj.status as string,
                    Created = DateTime.Parse(jsonObj.created as string),
                    Details = GetAnalyticsDetails(jsonObj.analytics)
                };
            }
        }

        private string GetApiUrl(string toAppend)
        {
            if (string.IsNullOrWhiteSpace(GoogleApiKey))
                throw new MissingFieldException("GoogleApiKey is missing");
            return string.Format(BaseGoogleUrl, GoogleApiKey, toAppend);
        }

        private GoogleShortUrlAnalyticsDetails GetAnalyticsDetails(dynamic jsonObj)
        {
            return new GoogleShortUrlAnalyticsDetails
            {
                AllTime = GetAnalyticsDetailsFull(jsonObj.allTime),
                Month = GetAnalyticsDetailsFull(jsonObj.month),
                Week = GetAnalyticsDetailsFull(jsonObj.week),
                Day = GetAnalyticsDetailsFull(jsonObj.day),
                TwoHours = GetAnalyticsDetailsFull(jsonObj.twoHours)
            };
        }

        private GoogleShortUrlAnalyticsDetailsFull GetAnalyticsDetailsFull(dynamic jsonObj)
        {
            return new GoogleShortUrlAnalyticsDetailsFull
            {
                ShortUrlClicks = int.Parse(jsonObj.shortUrlClicks as string),
                LongUrlClicks = int.Parse(jsonObj.longUrlClicks as string),
                Referrers = GetCountIdPairs(jsonObj.referrers),
                Browsers = GetCountIdPairs(jsonObj.browsers),
                Countries = GetCountIdPairs(jsonObj.countries),
                Platforms = GetCountIdPairs(jsonObj.platforms)
            };
        }

        private IEnumerable<GoogleCountIDPair> GetCountIdPairs(dynamic jsonObj)
        {
            var toReturn = new List<GoogleCountIDPair>();
            if (jsonObj == null)
                return toReturn;
            foreach (var obj in jsonObj as IEnumerable<dynamic>)
            {
                toReturn.Add(new GoogleCountIDPair { Count = int.Parse(obj.count as string), Id = obj.id as string });
            }

            return toReturn;
        }
    }
}
