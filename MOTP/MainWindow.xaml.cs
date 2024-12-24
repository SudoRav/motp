using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using MOTP.View;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MOTP
{

    public partial class MainWindow : Window
    {
        static readonly string googleCientId = "974181248114-2u0idur9gmlnv6jumt2lfo1b8e2kikmf.apps.googleusercontent.com";
        static readonly string googleLientSecret = "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";

        private static UserCredential Login(string googleClientId, string googleClientSecret)
        {
            try
            {
                ClientSecrets secrets = new ClientSecrets()
                {
                    ClientId = googleClientId,
                    ClientSecret = googleClientSecret
                };

                return GoogleWebAuthorizationBroker.AuthorizeAsync(secrets,
                    new[] { "https://www.googleapis.com/auth/drive.readonly" },
                    "user",
                    CancellationToken.None).Result;
            }
            catch { Environment.Exit(0); return null; }
        }

        public MainWindow()
        {
            InitializeComponent();

            if (Stat.Settings.timeEnd == DateTime.MinValue)
                Stat.Settings.timeEnd = DateTime.Now.AddDays(2);
        }

        public void ActiveCheck(DateTime now)
        {
            if (now <= Stat.Settings.timeEnd)
                return;

            MessageBox.Show("Время активно сеанса превышено, приложение завершит свою работу немедленно.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Stop);
            Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (MessageBox.Show("Закрыть текущее окно?\nЭто приведёт к безвозвратной потере внесённых данных!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                e.Cancel = false;

                Environment.Exit(0);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //new Home().FoneAutoSave();
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    string contFile = GetContentStreamFile("motppas.txt");

        //    if(contFile == "" || contFile == null)
        //    {
        //        Properties.Settings.Default.crntpassword = "";
        //        Properties.Settings.Default.loadpassword = "";
        //    }

        //    Properties.Settings.Default.loadpassword = contFile;
        //    Properties.Settings.Default.Save();

        //    //FoneApp(120000);
        //}

        //async void FoneApp(int timesleep)
        //{
        //    await Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            Thread.Sleep(timesleep);
        //            Properties.Settings.Default.loadpassword = GetContentStreamFile("motppas.txt");
        //            Properties.Settings.Default.Save();
        //        }
        //    });
        //}

        //public static string GetContentStreamFile(string nameFile)
        //{
        //    UserCredential credential = Login(googleCientId, googleLientSecret);

        //    using (DriveService driveService = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = credential }))
        //    {
        //        var listFilesRequest = driveService.Files.List().Execute();

        //        foreach (var file in listFilesRequest.Files)
        //        {
        //            if (file.Name == nameFile)
        //            {
        //                Debug.WriteLine($">>>{file.Id}<<<");
        //                try
        //                {
        //                    var request = driveService.Files.Get(file.Id);
        //                    var strm = new MemoryStream();

        //                    request.MediaDownloader.ProgressChanged +=
        //                    progress =>
        //                    {
        //                        switch (progress.Status)
        //                        {
        //                            case DownloadStatus.Downloading:
        //                                {
        //                                    Debug.WriteLine(progress.BytesDownloaded);
        //                                    break;
        //                                }
        //                            case DownloadStatus.Completed:
        //                                {
        //                                    Debug.WriteLine("Download complete.");
        //                                    break;
        //                                }
        //                            case DownloadStatus.Failed:
        //                                {
        //                                    Debug.WriteLine("Download failed.");
        //                                    break;
        //                                }
        //                        }
        //                    };
        //                    request.Download(strm);

        //                    var result = ConvertMemoryStreamToString(strm);
        //                    strm.Close();

        //                    return result;
        //                }
        //                catch
        //                {
        //                    Debug.WriteLine("Credential Not found");
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}

        //static string ConvertMemoryStreamToString(MemoryStream memoryStream)
        //{
        //    // Reset the position of the MemoryStream
        //    memoryStream.Position = 0;

        //    // Read the MemoryStream and convert to string
        //    using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
        //    {
        //        return reader.ReadToEnd();
        //    }
        //}

        public bool SetTBCol(TextBox TB, bool result, Brush brushError = null, Brush selError = null, Brush bruhStandart = null, Brush selStandart = null)
        {
            if (brushError == null)
                brushError = Brushes.DarkRed;
            if (selError == null)
                selError = Brushes.Red;

            if (bruhStandart == null)
                bruhStandart = (Brush)new BrushConverter().ConvertFrom("#FFABADB3");
            if (selError == null)
                selStandart = (Brush)new BrushConverter().ConvertFrom("#FF0078D7");


            if (result)
            {
                TB.BorderBrush = brushError;
                TB.SelectionBrush = (Brush)new BrushConverter().ConvertFrom("#FFD70000");

                return true;
            }
            else
            {
                TB.BorderBrush = bruhStandart;
                TB.SelectionBrush = selStandart;

                return false;
            }
        }
    }
}
