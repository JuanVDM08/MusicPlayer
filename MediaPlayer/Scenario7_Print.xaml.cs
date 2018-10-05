using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;
using MediaPlayer.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario7_Print : Page
    {
        public Scenario7_Print()
        {
            this.InitializeComponent();
        }
        private PrintManager printManager { get; set; }
        public PrintDocument printDocument { get; set; }
        private IPrintDocumentSource printDocumentSource { get; set; }
        private List<Page> printPreviewPages { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.printManager = PrintManager.GetForCurrentView();
            this.printManager.PrintTaskRequested += PrintJobTaskRequested;

            this.printDocument = new PrintDocument();
            this.printDocumentSource = this.printDocument.DocumentSource;
            this.printDocument.Paginate += Paginate;
            this.printDocument.GetPreviewPage += GetPreviewPage;
            this.printDocument.AddPages += AddPages;

            btnPrint.IsEnabled = false;

            base.OnNavigatedTo(e);
        }

        private void AddPages(object sender, AddPagesEventArgs e)
        {
            this.printDocument.AddPage(this.myGridView);
            this.printDocument.AddPagesComplete();
        }

        private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            //this.printDocument.AddPage(this.myGridView);
            //this.printDocument.AddPagesComplete();
            //this.printDocument.SetPreviewPage(e.PageNumber, this.myGridView); // prints one page, see below attempts to print > 1

            SoundFile item = this.myGridView.Items[e.PageNumber - 1] as SoundFile;

            Image i = new Image();

            i.Source = item.Image;

            printDocument.SetPreviewPage(e.PageNumber, i);

            printDocument.SetPreviewPageCount(SoundFileManager.soundFiles.Count(), PreviewPageCountType.Final);
        }

        private void Paginate(object sender, PaginateEventArgs e)
        {
            this.printDocument.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void PrintJobTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var printJob = args.Request.CreatePrintTask("Print", PrintJobSourceRequested);
            printJob.Completed += PrintJobCompleted;
        }

        private async void PrintJobCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            if (args.Completion == PrintTaskCompletion.Failed)
            {
                ContentDialog NoPrintJobDone = new ContentDialog()
                {
                    Title = "Print Error",
                    Content = "\nSomething Ugly happened, sorry we can't print",
                    PrimaryButtonText = "OK"
                };
                await NoPrintJobDone.ShowAsync();
            }
        }

        private void PrintJobSourceRequested(PrintTaskSourceRequestedArgs args)
        {
            args.SetSource(this.printDocumentSource);
        }

        private async void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.FileTypeFilter.Add(".pdf");
            filePicker.FileTypeFilter.Add(".txt");

            var file = await filePicker.PickSingleFileAsync();

            GetPdfFIle(file);
        }

        private async void GetPdfFIle(StorageFile file)
        {
            try
            {

                SoundFileManager.soundFiles = new List<SoundFile>();
                if (file == null)
                {
                    throw new ArgumentException("No files were selected");
                }


                PdfDocument pdfDocument = await PdfDocument.LoadFromFileAsync(file);

                for (uint i = 0; i < pdfDocument.PageCount; i++)
                {
                    var page = pdfDocument.GetPage(i);

                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        BitmapImage myImage = new BitmapImage();
                        await page.RenderToStreamAsync(stream);
                        await myImage.SetSourceAsync(stream);

                        SoundFileManager.soundFiles.Add(
                            new SoundFile()
                            {
                                Name = $"Page {i}",
                                Image = myImage,
                                Page = page
                            });
                    }
                }
                myGridView.ItemsSource = SoundFileManager.soundFiles;
                btnPrint.IsEnabled = true;
            }
            catch (ArgumentNullException ex)
            {
                var ErrorMsg = new ContentDialog()
                {
                    Title = "No file selected error",
                    Content = $"\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await ErrorMsg.ShowAsync();
            }
            catch (Exception ex)
            {
                var ErrorMsg = new ContentDialog()
                {
                    Title = "No file selected error",
                    Content = $"\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await ErrorMsg.ShowAsync();
            }
        }

        private async void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            List<SoundFile> sFiles = SoundFileManager.soundFiles.ToList();

            if (PrintManager.IsSupported())
            {
                try
                {
                    await PrintManager.ShowPrintUIAsync();
                }
                catch (Exception ex)
                {
                    ContentDialog ErrorOnPrinting = new ContentDialog()
                    {
                        Title = "Error while printing",
                        Content = $"\n {ex.Message}",
                        PrimaryButtonText = "OK"
                    };
                    await ErrorOnPrinting.ShowAsync();
                }
            }
            else
            {
                ContentDialog ErrorOnPrinting = new ContentDialog()
                {
                    Title = "Printer capability not support",
                    Content = $"\n You need to enable printing first",
                    PrimaryButtonText = "OK"
                };
                await ErrorOnPrinting.ShowAsync();
            }
        }

        private void myGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // add code to select changed item
        }
    }
    
}
