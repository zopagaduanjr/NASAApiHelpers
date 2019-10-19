using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cpeys.Model;

namespace NasaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiHelper.InitializeClient();
            var z = NASAEpicProcessor.GetLatestImages().Result;
        }
    }

        public class NASAEpicProcessor
        {
            public static async Task<RootObject> GetLatestImages()
            {
                string url = $"https://eonet.sci.gsfc.nasa.gov/api/v2.1/events";
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var EPICImages = await response.Content.ReadAsAsync<RootObject>();
                        return EPICImages;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

    

    public class EPICImage
        {
            public DateTime Date { get; set; }
            public string Image { get; set; }
            public string Caption { get; set; }
            public NASACoords Centroid_coordinates { get; set; }
            public Position Dscovr_j2000_Position { get; set; }
            public Position Lunar_j2000_Position { get; set; }
            public Position Sun_j2000_Position { get; set; }
            public NASACoords Coords { get; set; }
        }

        public class NASACoords
        {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }

        public class Position
        {
            public string X { get; set; }
            public string Y { get; set; }
            public string Z { get; set; }
        }


    
}
