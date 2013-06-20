using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace Chinchilla {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        Window2 window2;
        protected override void OnStartup(StartupEventArgs e) {
            if (e.Args.Length > 0) {
                //window2.Show;
                //window2.checkupdate();
                //window2.showEx(e.Args[0]);
                //MessageBox.Show("mirror window");

                Process[] thisProc = Process.GetProcessesByName ("Chinchilla") ;
                if (thisProc.Length > 0) {
                    for (int i = 0; i < thisProc.Length; i++) {
                        try{
                            if (thisProc[i].CloseMainWindow())
                                thisProc[i].Kill();
                        }
                        catch{
                        }
                    }
                }
                UpdateManager um = new UpdateManager(this);
                um.updateAllFile();
                //MessageBox.Show(e.Args[0]);  
            }          
            else {
                //normal start
                window2 = new Window2();
                window2.Show();
                //MessageBox.Show(e.Args[0]);
                //MessageBox.Show("normal window");
            }
        }  
    }
}
