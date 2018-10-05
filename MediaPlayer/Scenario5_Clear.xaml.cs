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
    public sealed partial class Scenario5_Clear : Page
    {
        private MainPage rootPage = MainPage.Current;

        public Scenario5_Clear()
        {
            this.InitializeComponent();
        }

        async private void PickPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlist = await rootPage.PickPlaylistAsync();

            if (playlist != null)
            {
                playlist.Files.Clear();

                if (await rootPage.TrySavePlaylistAsync(playlist))
                {
                    this.rootPage.NotifyUser("Playlist cleared.", NotifyType.StatusMessage);
                }
            }
        }
    }
}
