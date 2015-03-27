using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Multimedia;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Experiment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Naruto.Play();
            //var url = await YouTube.GetVideoUriAsync("3ACRcBTk2Ec", YouTubeQuality.Quality720P);
            //var abc = await YouTube.GetVideoUriAsync("EKyirtVHsK0", YouTubeQuality.Quality480P);
            //if (url != null)
            //{
            //    Naruto.Source = url.Uri;
            //    Naruto.Play();
            //}
            //MediaElement abc = new MediaElement();
            //abc.Source = new Uri("Songs/Naruto.mp3", UriKind.RelativeOrAbsolute);
            //abc.Play();
            //Frame.Navigate(typeof(Page1));  
        }

        private void Proceed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page1));
        }

    }
}
