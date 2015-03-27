using Experiment.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using Windows.Phone.Management;
using Transl8;
using Windows.UI.Core;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;
using Windows.Media.SpeechRecognition;

/*Notes on using Translator API:
How Windows Phone 8.1 affects developers http://www.jayway.com/2014/04/03/windows-phone-8-1-for-developers/
1)  Dispatcher.BeginInvoke( () => {}); is replaced by
    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => {});
    and System.Threading.Thread.Sleep(); is replaced by
    await Task.Delay(TimeSpan.FromSeconds(doubleValue));
2)  HTTPUtility.UrlEncode is replaced by WebUtility.UrlEncode
3)  poststream.CLose() is replaced by poststream.Dispose();
4) using Windows.Media.SpeechSynthesis instead of Windows.Phone.Speech.Synthesis
5) InstalledVoices.All is replaced by SpeechSyntheizer.AllVoices
7) speech.SetVoice(voices.ElementAt(0)) replaced by speech.Voice = voices.ElementAt(0);
6) Refer http://blogs.msdn.com/b/thunbrynt/archive/2014/04/15/windows-phone-8-1-for-developers-text-to-speech.aspx for more on speech
 */



// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
namespace Transl8
{
    public class AdmAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }
}
namespace Experiment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Translator : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string strLngTo = "ja";
        private static string strTextToTranslate = "";
        string strhelp = "&from=en&to=";
        string speaklanguage = "ja-JP";
        int voicegender = 0;

        public Translator()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            this.navigationHelper.OnNavigatedFrom(e);
            strLngTo = "ja";
            strTextToTranslate = "";
            strhelp = "&from=en&to=";
            speaklanguage = "ja-JP";
            voicegender = 0;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        #endregion


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProgressRing.IsIndeterminate = true;
            //Bar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            strTextToTranslate = txtToTrans.Text; //Sets the Text box text to the string to translate
            // STEP 1: Create the request for the OAuth service that will
            // get us our access tokens.
            String strTranslatorAccessURI =
                  "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            System.Net.WebRequest req = System.Net.WebRequest.Create(strTranslatorAccessURI);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            // Important to note -- the call back here isn't that the request was processed by the server
            // but just that the request is ready for you to do stuff to it (like writing the details)
            // and then post it to the server.
            IAsyncResult writeRequestStreamCallback =
              (IAsyncResult)req.BeginGetRequestStream(new AsyncCallback(RequestStreamReady), req);
            


        }
        private void RequestStreamReady(IAsyncResult ar)
        {
            // STEP 2: The request stream is ready. Write the request into the POST stream
            string clientID = "MyTranslatorPro";
            string clientSecret = "MyTranslatorProIITK1234";
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", WebUtility.UrlEncode(clientID), WebUtility.UrlEncode(clientSecret));
            // note, this isn't a new request -- the original was passed to beginrequeststream, so we're pulling a reference to it back out. It's the same request 
            

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)ar.AsyncState;
            // now that we have the working request, write the request details into it
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strRequestDetails);
            System.IO.Stream postStream = request.EndGetRequestStream(ar);
            postStream.Write(bytes, 0, bytes.Length);
            postStream.Dispose();
            // now that the request is good to go, let's post it to the server
            // and get the response. When done, the async callback will call the
            // GetResponseCallback function
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            //ProgressRing.IsIndeterminate = false;
            
        }
        private void GetResponseCallback(IAsyncResult ar)
        {
            // STEP 3: Process the response callback to get the token
            // we'll then use that token to call the translator service
            // Pull the request out of the IAsynch result
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            //    // The request now has the response details in it (because we've called back having gotten the response
            //    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            //}
            //catch(Exception)
            //{

            //}
            // Using JSON we'll pull the response details out, and load it into an AdmAccess token class (defined earlier in this module)
            // IMPORTANT (and vague) -- despite the name, in Windows Phone, this is in the System.ServiceModel.Web library,
            // and not the System.Runtime.Serialization one -- so you will need to have a reference to System.ServiceModel.Web
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
                // The request now has the response details in it (because we've called back having gotten the response
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new
                System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
                AdmAccessToken token =
                  (AdmAccessToken)serializer.ReadObject(response.GetResponseStream());

                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Net.WebUtility.UrlEncode(strTextToTranslate) + strhelp + strLngTo;
                System.Net.WebRequest translationWebRequest = System.Net.HttpWebRequest.Create(uri);
                // The authorization header needs to be "Bearer" + " " + the access token
                string headerValue = "Bearer " + token.access_token;
                translationWebRequest.Headers["Authorization"] = headerValue;
                // And now we call the service. When the translation is complete, we'll get the callback
                IAsyncResult writeRequestStreamCallback = (IAsyncResult)translationWebRequest.BeginGetResponse(new AsyncCallback(translationReady), translationWebRequest);
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ProgressRing.IsIndeterminate = false);
            }
            catch (Exception ex)
            {
                MessageBox("No Network Connectivity");
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ProgressRing.IsIndeterminate = false);
            }
        }
        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }  
        private async void translationReady(IAsyncResult ar)
        {
            // STEP 4: Process the translation
            // Get the request details
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            // Get the response details
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            // Read the contents of the response into a string
            System.IO.Stream streamResponse = response.GetResponseStream();
            System.IO.StreamReader streamRead = new System.IO.StreamReader(streamResponse);
            string responseString = streamRead.ReadToEnd();
            // Translator returns XML, so load it into an XDocument
            // Note -- you need to add a reference to the System.Linq.XML namespace
            System.Xml.Linq.XDocument xTranslation =
              System.Xml.Linq.XDocument.Parse(responseString);
            string strTest = xTranslation.Root.FirstNode.ToString();
            
            // We're not on the UI thread, so use the dispatcher to update the UI
            //Deployment.Current.Dispatcher.BeginInvoke(() => lblTranslatedText.Text = strTest);
            //CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => lblTranslatedText.Text = strTest);
            
            //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => lblTranslatedText.Text = strTest);
        }

        private async void btnSpeak_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filterLanguage = "";
                SpeechSynthesizer speech = new SpeechSynthesizer();
                filterLanguage = speaklanguage;
                // Query for a voice that speaks Japanese.
                IEnumerable<VoiceInformation> voices = from voice in SpeechSynthesizer.AllVoices
                                                       where voice.Language == filterLanguage
                                                       select voice;

                // Set the voice as identified by the query.
                speech.Voice = voices.ElementAt(voicegender);

                // Count in Japanese.
                //await speech.SpeakTextAsync(lblTranslatedText.Text);
                var voiceStream = await speech.SynthesizeTextToStreamAsync(lblTranslatedText.Text);


                var player = new MediaElement();
                player.SetSource(voiceStream, voiceStream.ContentType);
                player.Play();
            }
            catch
            {
                MessageBox("Ensure that the languages Japanese and English US are installed in Settings=>Speech=>Speech Languages");
            }
            
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            voicegender = 1;
        }

        private void Female_Checked(object sender, RoutedEventArgs e)
        {
           voicegender = 0;
        }

        private async void Accent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filterLanguage = "";
                SpeechSynthesizer speech = new SpeechSynthesizer();
                filterLanguage = "ja-JP";
                // Query for a voice that speaks Japanese.
                IEnumerable<VoiceInformation> voices = from voice in SpeechSynthesizer.AllVoices
                                                       where voice.Language == filterLanguage
                                                       select voice;

                // Set the voice as identified by the query.
                speech.Voice = voices.ElementAt(voicegender);

                // Count in Japanese.
                //await speech.SpeakTextAsync(lblTranslatedText.Text);
                var voiceStream = await speech.SynthesizeTextToStreamAsync(txtToTrans.Text);
                var player = new MediaElement();
                player.SetSource(voiceStream, voiceStream.ContentType);
                player.Play();
            }
            catch
            {
                MessageBox("Ensure that the languages Japanese is installed in Settings=>Speech=>Speech Languages");
            }
        }
        // State maintenance of the Speech Recognizer
        private async void SpeechRecog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SpeechRecognizer sr = new SpeechRecognizer();       //Creates a speech recognizer
                ProgressRing.IsIndeterminate = true;
                await sr.CompileConstraintsAsync();                 //Loads compile constraints!!!Important
                SpeechRecognitionResult result = await sr.RecognizeAsync();     //Starts recognition
                //if (result.Confidence == SpeechRecognitionConfidence.High || result.Confidence == SpeechRecognitionConfidence.Medium)
                //    lblTranslatedText.Text = result.Text;
                //    else
                //    lblTranslatedText.Text = "Huh?";
                ProgressRing.IsIndeterminate = false;
                txtToTrans.Text = result.Text;               //Get translated text into box
                sr.Dispose(); 
            }
            catch(Exception ex)
            {
                ProgressRing.IsIndeterminate = false;
                MessageBox("Speech recognition not enabled. Do so by going to Settings=>Speech=>Enable Speech Recognition. Also ensure that Speech Language in Settings=>Speech is set to English(United Kingdom)");
            }
               
        }

        private async void Change_Click(object sender, RoutedEventArgs e)
        {
            //string input = "i eat rice";
            //string languagePair = "en|bn";

            //string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
            //webClient.Encoding = System.Text.Encoding.UTF8;
            //string result = webClient.DownloadString(url);
            //result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
            //result = result.Substring(result.IndexOf(">") + 1);
            //result = result.Substring(0, result.IndexOf("</span>"));
            //WebRequest request = WebRequest.Create(
            //  "http://www.contoso.com/default.html");
            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //// Get the response.
            //WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //// Get the stream containing content returned by the server.
            //Stream dataStream = response.GetResponseStream();
            //// Open the stream using a StreamReader for easy access.
            //StreamReader reader = new StreamReader(dataStream);
            //// Read the content.
            //string responseFromServer = reader.ReadToEnd();
            //// Display the content.
            //// Clean up the streams and the response.
            //reader.Dispose();
            //response.Dispose();
            //Uri uri = new Uri("https://translate.google.com/#en/ja/hello");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //WebRequest request = WebRequest.Create("https://translate.google.com/#en/ja/hello");
            //request.Method = "GET"; // <-- ** You're putting textToTranslate into the query string so there's no need to use POST. **
            //request.ContentType = "application/x-www-form-urlencoded";

            // ** Commenting out the bit that writes the post data to the request stream **

            //// Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;
            //// Get the request stream.
            //Stream dataStream = request.GetRequestStream();
            //// Write the data to the request stream.
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //// Close the Stream object.
            //dataStream.Close();

            // Get the response.
            //WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            //Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            //string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.
            //reader.Close();
            //dataStream.Close();
            //response.Close();
            if (strLngTo == "ja")
            {
                strLngTo = "en";
                strhelp = "&from=ja&to=";
                LanguageIndicator.Text = "Enter the text in Japanese";
                speaklanguage = "en-US";
            }
            else
            {
                strLngTo = "ja";
                strhelp = "&from=en&to=";
                LanguageIndicator.Text = "Enter the text in English";
                speaklanguage = "ja-JP";
            }

            
        }

        private async void btnSpeak_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                string filterLanguage = "";
                SpeechSynthesizer speech = new SpeechSynthesizer();
                filterLanguage = speaklanguage;
                // Query for a voice that speaks Japanese.
                IEnumerable<VoiceInformation> voices = from voice in SpeechSynthesizer.AllVoices
                                                       where voice.Language == filterLanguage
                                                       select voice;

                // Set the voice as identified by the query.
                speech.Voice = voices.ElementAt(voicegender);

                // Count in Japanese.
                //await speech.SpeakTextAsync(lblTranslatedText.Text);
                var voiceStream = await speech.SynthesizeTextToStreamAsync(lblTranslatedText.Text);


                var player = new MediaElement();
                player.SetSource(voiceStream, voiceStream.ContentType);
                player.Play();
            }
            catch
            {
                MessageBox("In Settings=>Speech=>Speech Language download Japanese");
            }
            
        }

        private void B1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CountryInfo));
        }

        private void B2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Maps));
        }

        private void B3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Translator));
        }

        
    }
}
