using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cpeys.Model
{
    public class Entity
    {
    }
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }

    public class RootObject
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public List<event_> events { get; set; }


    }
    public class event_
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public List<category> categories { get; set; }
        public List<source> sources { get; set; }
        public List<geometry> geometries { get; set; }

    }
    public class category
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    public class source
    {
        public string id { get; set; }
        public string url { get; set; }
    }
    public class geometry
    {
        public string date { get; set; }
        public string type { get; set; }
        public decimal[] coordinates { get; set; }
    }

    public class EarthObject
    {
        public decimal cloud_score { get; set; }
        public string date { get; set; }
        public string id { get; set; }
        public string url { get; set; }

    }
}
