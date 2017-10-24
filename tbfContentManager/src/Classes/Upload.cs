using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using Renci.SshNet;
using MessageBox = System.Windows.MessageBox;


namespace tbfContentManager.Classes
{
    public class Upload
    {
        static MessageWindow msgWin;

        public static void UploadSFTPFile(Window owner, string host, string username,
        string password, string sourcefile, string destinationpath, int port)
        {
            // Overlay
            msgWin = new MessageWindow(owner, true);
            msgWin.Title = "Upload";
            msgWin.UpperText = "Upload in progress.";
            msgWin.LowerText = "Please wait...";
            msgWin.pb1.Visibility = Visibility.Visible;

            msgWin.Show();

            // Run Upload on background thread
            Task.Run(() => UploadInOtherThread(host, username, password, sourcefile, destinationpath, port));
            
        }

        static private void UploadInOtherThread(string host, string username,
        string password, string sourcefile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory(destinationpath);

                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                {
                    client.BufferSize = 4 * 1024;

                    // Set progress bar maximum on foreground thread
                    msgWin.pb1.Invoke(delegate { msgWin.pb1.Maximum = (int)fs.Length; });

                    client.UploadFile(fs, Path.GetFileName(sourcefile), UpdateProgresBar);
                }
                client.Disconnect();
            }

            msgWin.Invoke(delegate { msgWin.preventClose = false; });
            msgWin.Invoke(delegate { msgWin.Close(); } );

            MessageBox.Show("Die Datei wurde erfolgreich hochgeladen!", "Hochgeladen", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static private void UpdateProgresBar(ulong uploaded)
        {
            // Update progress bar on foreground thread
            msgWin.pb1.Invoke(delegate { msgWin.pb1.Value = (int)uploaded; });
        }
    }
}
