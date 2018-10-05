using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playlists;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario4_Remove : Page
    {
        private MainPage rootPage = MainPage.Current;

        public Scenario4_Remove()
        {
            this.InitializeComponent();
        }

        async private void PickPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlist = await rootPage.PickPlaylistAsync();

            if (playlist != null)
            {
                if (playlist.Files.Count > 0)
                {
                    playlist.Files.RemoveAt(playlist.Files.Count - 1);

                    if (await rootPage.TrySavePlaylistAsync(playlist))
                    {
                        rootPage.NotifyUser("The last item in the playlist was removed.", NotifyType.StatusMessage);
                    }
                }
                else
                {
                    rootPage.NotifyUser("No items in playlist.", NotifyType.ErrorMessage);
                }
            }
        }
    }
}
