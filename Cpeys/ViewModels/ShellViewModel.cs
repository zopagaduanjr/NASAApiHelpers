using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Cpeys.Model;
using Microsoft.CSharp.RuntimeBinder;

namespace Cpeys.ViewModels
{
    public class ShellViewModel : Screen
    {
        private BindableCollection<RootObject> _eventCategories = new BindableCollection<RootObject>();
        private event_ _selectedEvent;
        private List<category> _categories;

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

        public event_ SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                categories = value.categories;
                NotifyOfPropertyChange(() => SelectedEvent);
                NotifyOfPropertyChange(() => categories);
            }
        }

        public List<category> categories
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => categories);
            }
        }

        public async void Getter()
        {
            var Result = GetAsyncRootObject().Result;
            EventCategories.Add(Result);

        }
        public async Task<RootObject> GetAsyncRootObject()
        {
            string url = $"https://eonet.sci.gsfc.nasa.gov/api/v2.1/events";
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




    }
}
