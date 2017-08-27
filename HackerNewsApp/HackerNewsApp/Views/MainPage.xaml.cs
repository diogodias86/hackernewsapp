using HackerNewsApp.Api;
using HackerNewsApp.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HackerNewsApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private HackerNewsApi _apiClient;
        private bool _isLoading = false;

        public MainPage()
        {
            InitializeComponent();

            _apiClient = new HackerNewsApi();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadStories();
        }

        private async Task LoadStories()
        {
            try
            {
                IsLoading = true;

                listView.ItemsSource = await _apiClient.GetStoriesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var story = e.SelectedItem as HackerNewsStory;

            try
            {
                if (string.IsNullOrEmpty(story.Url))
                    return;

                Device.OpenUri(new Uri(story.Url));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
    }
}
