using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Chinchilla
{
    class DeviceInfoHelper
    {
        public static bool Connected
        {
            get
            {
                return isDeviceConnected();
            }
        }

        static bool isDeviceConnected()
        {
            //Not Imple
            return true;
        }

        public static Dictionary<string, string> GetPackageInfo()
        {
            String allpkginfo = "";
            String uid = "1001";
            Dictionary<string, string> pkginfo = new Dictionary<string, string>();

            if (!Connected)
            {
                return null;
            }

            Executecmd.ExecuteCommandSync("adb shell dumpsys package", out allpkginfo);

            /*
            FileStream fi = new FileStream(pkgfile, FileMode.Open);
            StreamReader m_streamReader = new StreamReader(fi);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = m_streamReader.ReadLine();
            */

            Regex reg = new Regex(@".*Package \[(\S+)\].*\s*userId=(\d*)");//Package ['(\S+)']
            MatchCollection mMatches = reg.Matches(allpkginfo);
            foreach (Match m in mMatches)
            {
                if (m.Groups.Count > 2 && Convert.ToInt32(m.Groups[2].ToString()) >10000)
                {
                    try
                    {
                        pkginfo.Add(m.Groups[1].ToString(), m.Groups[2].ToString());
                    }
                    catch (Exception)
                    {
                        //eat exception
                    }
                }
            }
            pkginfo.Add("system_server", "10000");
            /*
            while (strLine != null)
            {
                //Console.WriteLine(strLine);
                Match m = reg.Match(strLine);
                if (m.Groups.Count > 1)
                {
                    string packagename = m.Groups[1].ToString();
                    Regex uidreg = new Regex(@".*userId=(\d*)");
                    m_streamReader.ReadLine();
                    m = uidreg.Match(m_streamReader.ReadLine());
                    if (m.Groups.Count > 1)
                    {
                        uid = m.Groups[1].ToString();
                        pkginfo.Add(packagename, uid);
                    }
                }
                strLine = m_streamReader.ReadLine();
            }
             * */

            return pkginfo;
        }
    }
}
