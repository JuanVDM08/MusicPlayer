using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Playlists;
using Windows.Storage;
using Windows.Storage.FileProperties;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario2_Display : Page
    {
        private MainPage rootPage = MainPage.Current;

        public Scenario2_Display()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Displays the playlist picked by the user in the FilePicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void PickPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            this.rootPage.NotifyUser("", NotifyType.StatusMessage);
            Playlist playlist = await this.rootPage.PickPlaylistAsync();

            if (playlist != null)
            {
                string result = "Songs in playlist: " + playlist.Files.Count.ToString() + "\n";

                foreach (StorageFile file in playlist.Files)
                {
                    MusicProperties properties = await file.Properties.GetMusicPropertiesAsync();
                    result += "\n";
                    result += "File: " + file.Path + "\n";
                    result += "Title: " + properties.Title + "\n";
                    result += "Album: " + properties.Album + "\n";
                    result += "Artist: " + properties.Artist + "\n";
                }

                this.OutputStatus.Text = result;
            }
        }
    }
}
