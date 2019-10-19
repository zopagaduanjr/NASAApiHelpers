using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Cpeys.Model;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Cpeys.ViewModels
{
    public class ShellViewModel : Screen
    {
        private BindableCollection<RootObject> _eventCategories = new BindableCollection<RootObject>();
        private event_ _selectedEvent;
        private category _categories;
        private BindableCollection<event_> _events = new BindableCollection<event_>();
        private List<decimal> _coordinate;
        private EarthObject _earthObject;

        public ShellViewModel()
        {
            ApiHelper.InitializeClient();
            Task.Run(()=> Getter());
        }

        public BindableCollection<RootObject> EventCategories
        {
            get => _eventCategories;
            set
            {
                _eventCategories = value;
                NotifyOfPropertyChange(() => EventCategories);
            }
        }

        public BindableCollection<event_> Events
        {
            get => _events;
            set
            {
                _events = value;
                NotifyOfPropertyChange(() => Events);
            }
        }

        public event_ SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                category = value.categories.FirstOrDefault();
                Coordinate = value.geometries.FirstOrDefault().coordinates.ToList();
                EarthObject = null;

                NotifyOfPropertyChange(() => category);
                NotifyOfPropertyChange(() => Coordinate);
                NotifyOfPropertyChange(() => EarthObject);
                NotifyOfPropertyChange(() => SelectedEvent);
            }
        }

        public category category
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => category);
            }
        }

        public List<Decimal> Coordinate
        {
            get => _coordinate;
            set
            {
                _coordinate = value;
                NotifyOfPropertyChange(() => Coordinate);
            }
        }

        public EarthObject EarthObject
        {
            get => _earthObject;
            set
            {
                _earthObject = value;
                NotifyOfPropertyChange(() => EarthObject);
            }
        }

        public async void Getter()
        {
            var Result = GetAsyncRootObject().Result;
            EventCategories.Add(Result);
            var z = Result.events.Count;
            Events = new BindableCollection<event_>(Result.events);
        }
        public async Task<RootObject> GetAsyncRootObject()
        {
            string url = $"https://eonet.sci.gsfc.nasa.gov/api/v2.1/events?limit=141";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var foo = await response.Content.ReadAsAsync<RootObject>();
                    return foo;
                }
                else
                {
                    throw new Exception("No Internet");
                }
            }
        }


        public async void Imager()
        {
            var Result = await Task.Run(() => GetAsyncEarthObject());
            EarthObject = Result;
        }

        public async Task<EarthObject> GetAsyncEarthObject()
        {
            var latitude = SelectedEvent.geometries.FirstOrDefault().coordinates.FirstOrDefault();
            var longitude = SelectedEvent.geometries.FirstOrDefault().coordinates.LastOrDefault();
            string templateurl =
                string.Format(
                    @"https://api.nasa.gov/planetary/earth/imagery/?lat={0}&lon={1}&api_key=21ZTyMt5zcoQugv8e81myVDun9Mdxn1s6a7CfiCv",latitude,longitude);
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(templateurl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var foo = await response.Content.ReadAsAsync<EarthObject>();
                        return foo;
                }
                else
                {
                    var foo =  new EarthObject();
                    foo.url = @"F:\school\5th year\NASASpaceApps\NASAApiHelpers\Cpeys\Media\2488756.jpg";
                    return foo;
                }
            }
        }


        


    }
}
