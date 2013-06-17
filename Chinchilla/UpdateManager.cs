using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.Threading;

namespace Chinchilla {
    class UpdateManager {
        private WebClient dc = new WebClient();
        private object wnd;
        
        public UpdateManager(object window) {
            wnd = window;
            dc.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(dc_DownloadFileCompleted);
        }

        public UpdateManager() {
            //wnd = window;
            dc.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(dc_DownloadFileCompleted);
        }



        public void autoupdate() {
           /* if (flag.ToString().Equals("start")) {
                ParameterizedThreadStart ts = new ParameterizedThreadStart(autoupdate);
                //char[] a = "wtf";
                Thread newThread = new Thread(ts);
                newThread.Start("wtf");
            } else {*/
                try {
                    dc.DownloadFileAsync(new Uri(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\version.cfg"), "check_version.cfg", "versionfile");

                } catch (System.Exception ex) {
                    MessageBox.Show("更新失败！", "Sorry");
                }
                try {
                    //File.Copy("check_version.txt", "version.txt", true);
                } catch { }
         
           // }
        }

        private void  dc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.UserState.Equals("versionfile")){
                bool b = checkVer("version.cfg", "check_version.cfg");
                    if (b) {
                        //no update
                    } else {
                        MessageBoxResult result = MessageBox.Show("是否立即更新?",
  "有新更新可用", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes) {
                            try {

                                Process[] thisProc = Process.GetProcessesByName ("Chinchilla") ;
                                if (thisProc.Length > 0) {
                                    for (int i = 0; i < thisProc.Length; i++) {
                                        try{
                                            thisProc[i].Kill();
                                        }
                                        catch{
                                        }
                                    }
                                }

                                File.Copy("Chinchilla.exe", "ChinchillaMirror.exe", true);
                                System.Diagnostics.Process.Start("ChinchillaMirror.exe", "update");        
                            } catch { }
                            //((Window2)wnd).Close();
                            Environment.Exit(0);
                        } else if (result == MessageBoxResult.No) {
                            // No code here
                        } else {
                            // Cancel code here
                        }
                        
                    }
            } else if (e.UserState.Equals("zipfile")) {
                UnZipDir("update.zip","","",true);
                //MessageBox.Show("success update");
                restartapp();
            }
        }

        private void restartapp(){
            //MessageBox.Show("restart");
            //wnd.Visibility = Visibility.Hidden;
            try
            {
                MessageBox.Show("更新成功", "Notice");
                System.Diagnostics.Process.Start("Chinchilla.exe");
            }
            catch (System.Exception ex)
            {
            	
            }
            ((Application)wnd).Shutdown();
        }

        public void updateAllFile() {
            /*if (File.Exists("update.zip")) {
                File.Delete("update.zip");
            }*/
            dc.DownloadFileAsync(new Uri(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\update.zip"), "update.zip", "zipfile");
        }

      
        private static bool checkVer(string localFileName, string downLoadFileName) {
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

            if (localver.Equals(downver)) {
                b = true;
            }

            return b;

        }
        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipFileName">要解压的 zip 文件</param>
        /// <param name="extractLocation">zip 文件的解压目录</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>

        private void UnZipDir(string zipFileName, string extractLocation, string password, bool overWrite) {

            string myExtractLocation = extractLocation;
            if (myExtractLocation == "")
                myExtractLocation = Directory.GetCurrentDirectory();
            if (!myExtractLocation.EndsWith(@"/"))
                myExtractLocation = myExtractLocation + @"/";
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFileName));
            s.Password = password;
            ZipEntry theEntry;

            while ((theEntry = s.GetNextEntry()) != null)//判断下一个zip 接口是否未空
                {
                string directoryName = "";
                string pathToZip = "";
                pathToZip = theEntry.Name;

                if (pathToZip != "")
                    directoryName = Path.GetDirectoryName(pathToZip) + @"/";
                string fileName = Path.GetFileName(pathToZip);
                Directory.CreateDirectory(myExtractLocation + directoryName);
                if (fileName != "") {
                    try {
                        if ((File.Exists(myExtractLocation + directoryName + fileName) && overWrite) || (!File.Exists(myExtractLocation + directoryName + fileName))) {
                            FileStream streamWriter = File.Create(myExtractLocation + directoryName + fileName);
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true) {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    } catch (Exception ex) {
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(ex.Message);
                    }
                }
            }
            s.Close();

        }
        /*private  static bool updateAllFile() {
            WebClient dc = new WebClient();
            dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\update.txt", "update.txt");
            StreamReader localSR = File.OpenText("update.txt");
            while (!localSR.EndOfStream) {
                String filename = localSR.ReadLine();
                dc.DownloadFile(@"\\172.22.137.242\易平台\02.团队\04.QA\EVA\" + filename, filename);
            }
            return true;
        }*/

        /*private void startMain() {
            Visibility = Visibility.Hidden;
            try {
                System.Diagnostics.Process.Start("Chinchilla.exe");
            } catch (System.Exception ex) {

            }
            Close();
        }*/
    }
}
