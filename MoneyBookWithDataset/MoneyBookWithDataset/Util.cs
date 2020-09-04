using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MoneyBookWithDataset
{
    /// <summary>
    /// 자주사용하는명령
    /// </summary>
    public static class Util
    {

        //지정한 드라이브문자의 남은 공간을 체크함
        /// <summary>
        /// 디스크공간확인
        /// </summary>
        /// <remarks>남은 디스크의 공간을 확인 합니다</remarks>
        public static double GetFreeSpace(string driveletter)
        {
            if (driveletter.StartsWith("\\") || driveletter.StartsWith("/")) return -1;
            try
            {
                var di = new System.IO.DriveInfo(driveletter);
                var freespace = di.TotalFreeSpace;
                var totalspace = di.TotalSize;
                var freeSpaceRate = (freespace * 1.0 / totalspace) * 100.0;
                return freeSpaceRate;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Today WorkWeek
        /// </summary>
        public static int GetWorkWeek()
        {
            return GetWorkWeek(DateTime.Now);
        }

        /// <summary>
        /// 지정일자의 WorkWeek
        /// </summary>
        public static int GetWorkWeek(DateTime date)
        {
            var dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
            var date1 = date;
            var cal = dfi.Calendar;
            var week = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            return week;
        }

        public static string MakeFilePath(params string[] param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in param)
            {
                if (sb.Length > 0 && sb.ToString().EndsWith("\\") == false) sb.Append("\\");
                sb.Append(item.Replace("/", "\\"));
            }
            var retval = sb.ToString().Replace("/", "\\").Replace("\\\\", "\\");
            return retval.ToString();
        }

        public static string MakeFTPPath(params string[] param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in param)
            {
                if (sb.Length > 0 && sb.ToString().EndsWith("/") == false) sb.Append("/");
                sb.Append(item.Replace("\\", "/"));
            }
            var retval = sb.ToString().Replace("//", "/");
            return retval.ToString();
        }


        /// <summary>
        /// Radian To Degree
        /// </summary>
        public static double rad2Deg(double source)
        {
            return (source * 1.0) * 180.0 / Math.PI;
        }
        /// <summary>
        /// degree to radian
        /// </summary>
        /// <remarks>각도 변환(Degree -&gt; Radian)</remarks>
        public static double deg2rad(double source)
        {
            return (source * 1.0) * Math.PI / 180.0;
        }
        /// <summary>
        /// 문자를 HEX 문자로 변경합니다. 각 HEX문자 사이에는 공백이 없습니다.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCharHexString(string input)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char b in input.ToCharArray())
                sb.Append(((byte)b).ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// 16진수형태의 문자를 일반 문자열로 반환한니다.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringFromHexString(string input)
        {
            int wordCount = (int)(input.Length / 2);
            int n = (int)(input.Length % 2);
            if (n != 0) return "X:Length Error";

            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wordCount; i++)
            {
                string hexstr = input.Substring(i * 2, 2);
                try
                {
                    byte ascvalue = Convert.ToByte(hexstr, 16);
                    sb.Append((char)ascvalue);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            //두자리씩 끊어서 16진수로 변환하고 ASC값을 누적한다.
            return sb.ToString();
        }


        /// <summary>
        /// 현재 프로그램이 개인용위치에서 실행중인가(클릭원스의경우)
        /// </summary>
        /// <returns></returns>
        public static Boolean isLocalApplication()
        {
            var localpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Util.CurrentPath.StartsWith(localpath);
        }

        /// <summary>
        /// 문자열 배열을 CSV라인으로 변환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MakeCSVString(params string[] param)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            Boolean first = true;
            foreach (var data in param)
            {
                if (first == false) sb.Append(',');
                else first = false;
                sb.Append(ToCSVString(data));
            }
            return sb.ToString();
        }


        /// <summary>
        /// CSV데이터포맷으로 버퍼를 반환합니다. 문자열내에 , 가 있는 데이터는 쌍따옴표로 구분합니다.
        /// </summary>
        /// <remarks>CSV데이터포맷으로 버퍼를 반환합니다. 문자열내에 , 가 있는 데이터는 쌍따옴표로 구분합니다.</remarks>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] GetCSVBuffer(string line)
        {
            List<string> buffer = new List<string>();
            if (line.Trim() == "") return buffer.ToArray();

            System.Text.StringBuilder sb = new StringBuilder();
            bool findsig = false;
            var charbuf = line.ToCharArray();
            char pchar = '\0';
            bool findCommaString = false;
            foreach (var c in charbuf)
            {
                if (c == ',')
                {
                    if (findsig) sb.Append(c);  //대상에 콤마가 잇으므로 게소 누적한다.
                    else if (findCommaString == false)
                    {
                        //데이터를 분리해줘야함
                        buffer.Add(sb.ToString());
                        sb.Clear();
                        findsig = false;
                    }
                    else findCommaString = false;
                }
                else if (c == '\"')
                {
                    if (pchar == ',')
                    {
                        if (!findsig) findsig = true;
                        else findsig = false; //완료된 경우이다.
                    }
                    else if (!findsig)
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        buffer.Add(sb.ToString());
                        sb.Clear();
                        findsig = false;
                        findCommaString = true;
                    }
                    //if (!findsig) findsig = true;
                    //else sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
                pchar = c;
            }
            //if(sb.Length > 0)
            //{
            buffer.Add(sb.ToString());
            //}
            return buffer.ToArray();
        }

        /// <summary>
        /// 아두이노의 Map 명령 (Scale 조정)
        /// </summary>
        public static double map(double source, int in_min, int in_max, int out_min, int out_max)
        {
            return (source - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }


        #region "MessageBox"
        /// <summary>
        /// 메세지박스(정보)
        /// </summary>
        public static void MsgI(string m)
        {
            MessageBox.Show(m, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 메세지박스(오류)
        /// </summary>
        public static void MsgE(string m)
        {
            MessageBox.Show(m, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 메세지박스(질문)
        /// </summary>
        public static DialogResult MsgQ(string m)
        {
            DialogResult dlg = MessageBox.Show(m, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dlg;
        }

        #endregion

        /// <summary>
        /// 컨텍스트메뉴 태그 읽기
        /// </summary>
        public static object getTagFromContextMenu(object sender)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                ContextMenuStrip calendarMenu = menuItem.Owner as ContextMenuStrip;

                if (calendarMenu != null)
                {
                    var controlSelected = calendarMenu.SourceControl as Control;
                    return controlSelected.Tag;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 버그리포트저장
        /// </summary>
        public static void SaveBugReport(string content, string subdirName = "BugReport")
        {
            try
            {
                var path = CurrentPath + subdirName;
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                var file = path + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".txt";
                System.IO.File.WriteAllText(file, content, System.Text.Encoding.UTF8);
            }
            catch
            {
                //nothing
            }
        }

        /// <summary>
        /// 실행위치반환
        /// </summary>
        /// <remarks>현재실행중인폴더를 반환합니다.</remarks>
        public static string CurrentPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        /// <summary>
        /// 콤마와 줄바꿈등을 제거합니다.
        /// </summary>
        /// <returns></returns>
        public static string ToCSVString(string src)
        {
            if (src == null) return string.Empty;
            string strdata = strdata = src.Replace("\r", "").Replace("\n", "");
            if (strdata.IndexOf(',') != -1)
            {
                strdata = "\"" + strdata + "\"";    //180618 콤마가들어가는 csv 처리
            }
            return strdata;
        }

        /// <summary>
        /// 프로그램 실행
        /// </summary>
        public static Boolean RunProcess(string file, string arg = "")
        {
            var fi = new System.IO.FileInfo(file);
            if (!fi.Exists) return false;
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(file);
            si.Arguments = arg;
            prc.StartInfo = si;
            prc.Start();
            return true;
        }

        #region "convert"
        /// <summary>
        /// 숫자데이터를 문자로(;로 구분)
        /// </summary>
        public static string RectToStr(Rectangle rect)
        {
            return string.Format("{0};{1};{2};{3}", rect.X, rect.Y, rect.Width, rect.Height);
        }
        /// <summary>
        /// 숫자데이터를 문자로(;로 구분)
        /// </summary>
        public static string RectToStr(RectangleF rect)
        {
            return string.Format("{0};{1};{2};{3}", rect.X, rect.Y, rect.Width, rect.Height);
        }
        /// <summary>
        /// 숫자데이터를 문자로(;로 구분)
        /// </summary>
        public static string PointToStr(Point pt)
        {
            return string.Format("{0};{1}", pt.X, pt.Y);
        }
        /// <summary>
        /// 숫자데이터를 문자로(;로 구분)
        /// </summary>
        public static string PointToStr(PointF pt)
        {
            return string.Format("{0};{1}", pt.X, pt.Y);
        }
        /// <summary>
        /// 문자를 숫자데이터로
        /// </summary>
        public static Rectangle StrToRect(string str)
        {
            if (str.IsEmpty() || str.Split(';').Length != 4) str = "0;0;0;0";
            var roibuf1 = str.Split(';');
            return new System.Drawing.Rectangle(
                int.Parse(roibuf1[0]),
                int.Parse(roibuf1[1]),
                int.Parse(roibuf1[2]),
                int.Parse(roibuf1[3]));
        }
        /// <summary>
        /// 문자를 숫자데이터로
        /// </summary>
        public static RectangleF StrToRectF(string str)
        {
            if (str.IsEmpty() || str.Split(';').Length != 4) str = "0;0;0;0";
            var roibuf1 = str.Split(';');
            return new System.Drawing.RectangleF(
                float.Parse(roibuf1[0]),
                float.Parse(roibuf1[1]),
                float.Parse(roibuf1[2]),
                float.Parse(roibuf1[3]));
        }
        /// <summary>
        /// 문자를 숫자데이터로
        /// </summary>
        public static Point StrToPoint(string str)
        {
            if (str.IsEmpty() || str.Split(';').Length != 2) str = "0;0";
            var roibuf1 = str.Split(';');
            return new System.Drawing.Point(
                int.Parse(roibuf1[0]),
                int.Parse(roibuf1[1]));
        }
        /// <summary>
        /// 문자를 숫자데이터로
        /// </summary>
        public static PointF StrToPointF(string str)
        {
            if (str.IsEmpty() || str.Split(';').Length != 2) str = "0;0";
            var roibuf1 = str.Split(';');
            return new System.Drawing.PointF(
                float.Parse(roibuf1[0]),
                float.Parse(roibuf1[1]));
        }
        #endregion

        #region "NIC"

        /// <summary>
        /// NIC 카드 조회
        /// </summary>
        /// <remarks>지정된 nic카드가 현재 목록에 존재하는지 확인한다.</remarks>
        /// <returns></returns>
        public static Boolean ExistNIC(string NICName)
        {
            if (string.IsNullOrEmpty(NICName)) return false;
            foreach (string NetName in NICCardList())
            {
                if (NetName.ToLower() == NICName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// NIC카드 비활성화(관리자권한필요)
        /// </summary>
        /// <param name="NicName"></param>
        public static Boolean NICDisable(string NICName)
        {
            //해당 nic 가 현재 목록에 존재하는지 확인한다.

            string cmd = "interface set interface " + NICName + " disable";
            Process prc = new Process();
            ProcessStartInfo si = new ProcessStartInfo("netsh", cmd);
            si.WindowStyle = ProcessWindowStyle.Hidden;
            prc.StartInfo = si;
            prc.Start();

            ////목록에서 사라질때까지 기다린다.
            DateTime SD = DateTime.Now;
            Boolean timeout = false;
            while ((true))
            {

                bool FindNetwork = false;
                foreach (string NetName in NICCardList())
                {
                    if (NetName == NICName.ToLower())
                    {
                        FindNetwork = true;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }

                if (!FindNetwork)
                    break; // TODO: might not be correct. Was : Exit While

                System.Threading.Thread.Sleep(1000);
                TimeSpan ts = DateTime.Now - SD;
                if (ts.TotalSeconds > 10)
                {
                    timeout = true;
                    break; // TODO: might not be correct. Was : Exit While
                }
            }
            return !timeout;
        }

        /// <summary>
        /// NIC카드 목록
        /// </summary>
        public static List<String> NICCardList()
        {
            List<String> Retval = new List<string>();
            foreach (System.Net.NetworkInformation.NetworkInterface Net in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (Net.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                {
                    Retval.Add(Net.Name.ToUpper());
                }
            }
            return Retval;
        }

        /// <summary>
        /// NIC카드 활성화(관리자권한필요)
        /// </summary>
        /// <param name="NicName"></param>
        public static Boolean NICEnable(string NICName)
        {
            string cmd = "interface set interface " + NICName + " enable";
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo("netsh", cmd);
            si.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            prc.StartInfo = si;
            prc.Start();


            ////목록에생길떄까지 대기
            DateTime SD = DateTime.Now;
            while ((true))
            {

                bool FindNetwork = false;
                foreach (string NetName in NICCardList())
                {
                    if (NetName.ToLower() == NICName.ToLower())
                    {
                        FindNetwork = true;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }

                if (FindNetwork)
                    break; // TODO: might not be correct. Was : Exit While

                System.Threading.Thread.Sleep(1000);
                TimeSpan ts = DateTime.Now - SD;
                if (ts.TotalSeconds > 10)
                {
                    return false;
                }
            }

            ////결이 완료될떄까지 기다린다.
            SD = DateTime.Now;
            while ((true))
            {

                bool FindNetwork = false;
                foreach (System.Net.NetworkInformation.NetworkInterface Net in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (Net.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.GigabitEthernet &&
                        Net.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Ethernet) continue;
                    if (Net.Name.ToLower() == NICName.ToLower())
                    {
                        //string data = Net.GetIPProperties().GatewayAddresses[0].ToString();

                        if (Net.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                        {

                            FindNetwork = true;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                if (FindNetwork)
                    return true;

                System.Threading.Thread.Sleep(1000);
                TimeSpan ts = DateTime.Now - SD;
                if (ts.TotalSeconds > 10)
                {
                    return false;
                }
            }

        }

        #endregion

        /// <summary>
        /// 탐색기로열기
        /// </summary>
        public static void RunExplorer(string arg)
        {
            if (arg.StartsWith("http://") || arg.StartsWith("https://"))
            {
                System.Diagnostics.Process.Start(arg);
            }
            else
            {
                System.Diagnostics.ProcessStartInfo si = new ProcessStartInfo("explorer");
                si.Arguments = arg;
                System.Diagnostics.Process.Start(si);
            }

        }

        #region "watchdog"
        /// <summary>
        /// 와치독실행(와치독실행파일필요)
        /// </summary>
        public static void WatchDog_Run()
        {
            System.IO.FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "WatchCat.exe");
            if (!fi.Exists) return;
            var Exist = CheckExistProcess("watchcat");
            if (Exist) return;
            RunProcess(fi.FullName);
        }

        /// <summary>
        /// 실행프로세스검색
        /// </summary>
        /// <remarks>해당 프로세스가 현재 실행중인지 확인 합니다</remarks>
        public static Boolean CheckExistProcess(string ProcessName)
        {
            foreach (var prc in System.Diagnostics.Process.GetProcesses())
            {
                if (prc.ProcessName.StartsWith("svchost")) continue;
                if (prc.ProcessName.ToUpper() == ProcessName.ToUpper()) return true;
            }
            return false;
        }
        #endregion

        #region "web function"
        /// <summary>
        /// URL에서 문자 읽기
        /// </summary>
        /// <remarks>URL로부터 문자열을 수신합니다.</remarks>
        /// <param name="url"></param>
        /// <param name="isError"></param>
        /// <returns></returns>
        public static string GetStrfromurl(string url, out Boolean isError)
        {
            isError = false;
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Timeout = 60000;
                request.ReadWriteTimeout = 60000;

                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = request.GetResponse() as HttpWebResponse;
                var txtReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = txtReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                isError = true;
                result = ex.Message.ToString();
            }
            return result;
        }

        #endregion

    }
}
