using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playlists;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class Scenario1_Create : Page
    {
        private MainPage rootPage = MainPage.Current;

        public Scenario1_Create()
        {
            this.InitializeComponent();
        }

        async private void PickAudioButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = MainPage.CreateFilePicker(MainPage.audioExtensions);
            IReadOnlyList<StorageFile> files = await picker.PickMultipleFilesAsync();

            if (files.Count > 0)
            {
                Playlist playlist = new Playlist();

                foreach (StorageFile file in files)
                {
                    playlist.Files.Add(file);
                }

                StorageFolder folder = KnownFolders.MusicLibrary;
                string name = "Sample";
                NameCollisionOption collisionOption = NameCollisionOption.ReplaceExisting;
                PlaylistFormat format = PlaylistFormat.WindowsMedia;

                try
                {
                    StorageFile savedFile = await playlist.SaveAsAsync(folder, name, collisionOption, format);
                    this.rootPage.NotifyUser(savedFile.Name + " was created and saved with " + files.Count + " files.", NotifyType.StatusMessage);
                }
                catch (Exception error)
                {
                    rootPage.NotifyUser(error.Message, NotifyType.ErrorMessage);
                }
            }
            else
            {
                rootPage.NotifyUser("No files picked.", NotifyType.ErrorMessage);
            }
        }
    }
}
