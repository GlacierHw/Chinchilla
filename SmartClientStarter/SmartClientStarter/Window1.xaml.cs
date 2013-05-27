using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;

namespace SmartClientStarter
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        //FtpDownLoader fdl = null;
        public Window1()
        {
            InitializeComponent();
            //fdl = new FtpDownLoader();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WebClient dc = new WebClient();
            try{
                dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\version.txt", "check_version.txt");
                bool b = this.checkVer("version.txt", "check_version.txt");
                if (b){               
                }
                else{
                    dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\update_info.txt", "update_info.txt");
                    StreamReader localSR = File.OpenText("update_info.txt");
                    MessageBox.Show(localSR.ReadToEnd(), "有新版本可用！");

                    updateAllFile();

                    MessageBox.Show("更新完成！","提示");
                }
            } catch (System.Exception ex) {
                MessageBox.Show("更新失败！", "Sorry");
            }
                try {
                    File.Copy("check_version.txt", "version.txt", true);
                } catch { }
            try{
                startMain();             
            }
            catch (System.Exception ex){
            	
            }
            
        }

        private bool checkVer(string localFileName, string downLoadFileName)
        {
            bool b = false;
            if (!File.Exists(localFileName)) {
               return false;
            }
            StreamReader localSR = File.OpenText(localFileName);
            StreamReader downSR = File.OpenText(downLoadFileName);

            string localver = localSR.ReadLine();
            string downver = downSR.ReadLine();

            localSR.Close();
            downSR.Close();

            if (localver.Equals(downver))
            {
                b = true;
            }

            return b;

        }

        private bool updateAllFile()
        {
            WebClient dc = new WebClient();
            dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\update.txt", "update.txt");
            StreamReader localSR = File.OpenText("update.txt");
            while (!localSR.EndOfStream)
            {
                String filename = localSR.ReadLine();
                dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\" + filename, filename);
            }
            return true;
        }

        private void startMain()
        {
            this.Visibility = Visibility.Hidden;
            try
            {
                System.Diagnostics.Process.Start("Chinchilla.exe");
            }
            catch (System.Exception ex)
            {
            	
            }
            this.Close();
        }
    }
}
