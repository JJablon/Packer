﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Packer
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);

        public Form1()
        {
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog2.RootFolder = Environment.SpecialFolder.MyComputer;
            listBox3.SelectionMode = SelectionMode.One;

            listBox3.SetSelected(0, true);
            listBox4.SelectionMode = SelectionMode.One;

            listBox4.SetSelected(0, true);
            textBox1.Size = new System.Drawing.Size(153, 20);
            folderBrowserDialog1.SelectedPath = @"E:\[R.G. Mechanics] Lost - Via Domus";
            folderBrowserDialog2.SelectedPath = @"E:\Split\" + DateTime.Now.ToShortDateString() + "II" + DateTime.Now.ToShortTimeString().Replace(':', ';');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long size = 0, size_total = 0;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            folderBrowserDialog1.SelectedPath = null;
            bool[] dodane;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && folderBrowserDialog1.SelectedPath != null)
            {
                size = 0;
                label1.Text = folderBrowserDialog1.SelectedPath;
                label2.Text = folderBrowserDialog2.SelectedPath;
                DirectoryInfo di = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                long counter = 0;
                dodane = new bool[di.GetFiles().LongLength];
                List<FileInfo> ff = new List<FileInfo>();
                int res_counter = 0;
                bool flag = true;
                while (flag == true && res_counter < 20)
                {
                    res_counter++;
                    toolStripStatusLabel2.Text = res_counter + "";
                    flag = false;
                    foreach (bool b in dodane) if (b == false) flag = true; // choć 1 niedodany element - flaga true, dodawania dalej

                    for (int powt = 0; powt < 10; powt++)
                    {
                        counter = 0;
                        foreach (FileInfo fi in di.GetFiles())
                        {

                            // if (size + fi.Length < 4700000000 && dodane[counter] == false)// 4 700 mb
                            if (size + fi.Length < 750000000 && dodane[counter] == false)//470 mb
                            {
                                size += fi.Length;
                                dodane[counter] = true;
                                listBox1.Items.Add(fi.Name);

                                if (fi.Length > 1000000)
                                    listBox2.Items.Add(((double)(fi.Length / 1000000)).ToString("# ###.##"));
                                else
                                {
                                    listBox2.Items.Add(((double)((double)fi.Length / 1000000f)).ToString("0.###"));
                                }
                            }
                            else { }

                            if (powt == 0 && res_counter == 1) ff.Add(fi);
                            counter++;
                        }
                    }
                    if (size >= 1) { listBox1.Items.Add("::: rozmiar " + ((size / 1000000)).ToString("# ###.##")); listBox2.Items.Add(":::::::::::"); size_total += size; }
                    size = 0;

                }

                toolStripStatusLabel1.Text = ((size_total / 1000000)).ToString("# ###.##      ");
                int count = 0;
                foreach (bool b in dodane) { if (b == false) MessageBox.Show("Nie dodano pliku" + ff[count].Name); count++; } // choć 1 niedodany element - flaga true, dodawania dalej


            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] ss;

            int counter = 0;
            foreach (var f in listBox1.Items)
            {
                if (f.ToString()[0] == ':') counter++;
            }
            ss = new string[counter]; // 1 pusty na końcu?????????????????????????????????????????????????????????
            //int cnt = 0;
            foreach (var f in listBox1.Items)
            {
                if (f.ToString()[0] == ':') { richTextBox1.SaveFile("E:\\Split\\txt" + cnt + ".txt", RichTextBoxStreamType.PlainText); richTextBox1.Text = ""; ss[cnt] = "E:\\Split\\txt" + (cnt) + ".txt"; cnt++; }
                //else { ss[cnt] += " " + ; }
                else
                {
                    richTextBox1.AppendText(folderBrowserDialog1.SelectedPath + "\\" + f.ToString() + "\n");
                }
            }

            for (int a = 0; a < counter; a++)
            {
                DirectoryInfo di2 = new DirectoryInfo(folderBrowserDialog2.SelectedPath);
                if (di2.Exists == false)
                    di2.Create();




                string comd = " A -ri" + listBox4.SelectedIndex * 3 + "-m" + listBox3.SelectedIndex + " -hp" + textBox1.Text + " " + folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + (a + 1) + " @" + ss[a];

                //  Process.Start(comd);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = textBox3.Text + "\\WinRAR.exe";
                startInfo.Arguments = comd;
                Process.Start(startInfo);
            }


            cnt = counter;
            zrobione = new bool[cnt];
            //for (int c = 0; c < cnt;c++ ) zrobione[c] = true;
            name = textBox2.Text;
            timer3.Enabled = true;
        }
        int cnt = 0;
        string name = "";
        bool[] zrobione;
        /*void timer1_Tick(object sender, EventArgs e)
        {
           
                DirectoryInfo di2 = new DirectoryInfo(folderBrowserDialog2.SelectedPath);
                if (di2.Exists == true)
                {
                    foreach (FileInfo fi in di2)
                    {
                        if(fi.Length > 100000){
                            string comd = " /New:BootDisc /Media:CD";
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.FileName = textBox4.Text + "nero.exe"
                            startInfo.Arguments = comd;
                            Process.Start(startInfo);
                    }

                }




              
            
        }
        */
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Visible = !textBox1.Visible;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //wypal();

        }
        private void wypal(int a)
        {
            //DODAC PETLE
            //int a = 1;
            ISOCreate.Program p = new ISOCreate.Program();
            string[] par = new String[] { folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + (a + 1)+".iso ", folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + (a +1)+ ".rar " };

            p.create(par);
           // timer1.Enabled = true;
            timers[n].Enabled = true;
        }


        private void timer1_Tick(object sender, System.EventArgs e)
        {
            //dodac petle
            int a = 1;
            FileInfo fi = new FileInfo(folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + a + ".iso ");
            FileInfo fi2 = new FileInfo( folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + a + ".rar ");
     
            if(fi.Length >= fi2.Length){
                timer2.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, System.EventArgs e)
        {
         /*   //dodac petle
            int a = 1;
            FileInfo fi = new FileInfo(folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + a + ".iso ");
            FileInfo fi2 = new FileInfo( folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + a + ".rar ");
            if (fi.Length >= fi2.Length)
            {
                iso_OnFinish(1);
            }*/

        }

        /*
        //Initialization
        IsoFromMedia iso;
        iso = new IsoFromMedia();
        iso.OnFinish += new IsoEventHandler(iso_OnFinish);
        //iso.OnMessage += new IsoEventHandler(iso_OnMessage);
        //iso.OnProgress += new IsoEventHandler(iso_OnProgress);

        //Status status =
        //iso.CreateIsoFromMedia(folderBrowserDialog2.SelectedPath+"\\", @"E:\Files\test3.iso");
        iso.CreateIsoFromMedia("T:\\", @"E:\test4.iso");
//@"F:\", "E:\\test.iso");
            
       // if (status != Status.Running)
       // {
       //     iso.Stop();
            //Print error message
       // }
          


        //1, KOPIOWANIE PLIKU RAR DO PUSTEGO FOLDERU (1 PLIK - 1 FOLDER)
        //2, UTWORZENIE ALIASU KOMENDĄ POWŁOKI SYSTEMU
        //3, WYWOŁANIE ISO.CREATEFROMMEDIA
         * */
        

        private void iso_OnFinish(int a)
        {
            
            //string comd = " -w E:\\Files\\test.iso";
            string comd = " -w " + folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + a + ".iso";// /New:BootDisc /Media:CD";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = textBox4.Text + "\\" + "nero.exe";
            startInfo.Arguments = comd;
            Process.Start(startInfo);
        }

        private void timer3_Tick(object sender, System.EventArgs e)
        {
            
            //sprawdz okno winrara
            //jesli nie ma && checkbox3.checked == true (autowypalanie) then wypal()
            Process[] processlist = Process.GetProcesses();
            string str="";
            bool[] aktywne = new bool[cnt];
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.ProcessName == "WinRAR" && process.MainWindowTitle[0] == 'T' && process.MainWindowTitle[1] == 'w' && process.MainWindowTitle[2] == 'o')
                {
                    if(cnt <= 9){
                        aktywne[Int32.Parse(process.MainWindowTitle[process.MainWindowTitle.Length - 1].ToString())-1] = true;
                    }
                    //str+= "Process: "+ process.ProcessName +"ID:"+process.Id +" Window title:" +process.MainWindowTitle+"\n";
                }
            }
            //int cn=0;
            bool all_done = true;
            for (int cn = 0; cn < cnt; cn++)
            {
                if (aktywne[cn] == true) { all_done = false; } //aktywne, nie zrobione
                else
                { //zrobione[cn] == true;
                    Thread.Sleep(1000);
                    wypal(cn);
                }
            }
            if (all_done) timer3.Enabled = false;
            //MessageBox.Show(str);
             // IntPtr ptrFF = FindWindow(null, "Tworzę archiwum test.rar");
           // SetForegroundWindow(ptrFF);
                //SendKeys.SendWait("{F1}");
        }

        
    }
}
    /*
     * IsoFromMedia - Create an ISO image of a CD/DVD
     * ----------------------------------------------------------------
     * Copyright © 2009 Konstantin Gross
     * http://www.texturenland.de
     * http://blog.texturenland.de
    



    /// <summary>
    /// Allows to create ISO images from media (CD / DVD)
    /// </summary>
    public class IsoFromMedia
    {
        #region Variables
        /// <summary>
        /// BackgroundWorker for creating the ISO file
        /// </summary>
        BackgroundWorker bgCreator;

        /// <summary>
        /// FileStream for reading
        /// </summary>
        FileStream streamReader;

        /// <summary>
        /// FileStream for writing
        /// </summary>
        FileStream streamWriter;
        #endregion

        #region Constants
        /// <summary>
        /// 128 KB block size
        /// </summary>
        const int BUFFER = 0x20000;

        /// <summary>
        /// 4 GB maximum size per file on FAT32 file system
        /// </summary>
        const long LIMIT = 4294967296;
        #endregion

        #region Events
        /// <summary>
        /// Raised if progress value changes
        /// </summary>
        public event IsoEventHandler OnProgress;

        /// <summary>
        /// Raised when a message appears (errors, etc.)
        /// </summary>
        public event IsoEventHandler OnMessage;

        /// <summary>
        /// Raised when the file is finished
        /// </summary>
        public event IsoEventHandler OnFinish;
        #endregion

        #region Properties
        /// <summary>
        /// Path to the ISO file
        /// </summary>
        string PathToIso { get; set; }

        /// <summary>
        /// Size of the medium
        /// </summary>
        public long MediumSize { get; set; }

        /// <summary>
        /// Medium handle
        /// </summary>
        SafeFileHandle Handle { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public IsoFromMedia()
        {
            bgCreator = new BackgroundWorker();
            bgCreator.WorkerSupportsCancellation = true;
            bgCreator.DoWork += new DoWorkEventHandler(bgCreator_DoWork);
            bgCreator.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgCreator_RunWorkerCompleted);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the thread creating the ISO file
        /// </summary>
        void bgCreator_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                streamReader = new FileStream(Handle, FileAccess.Read, BUFFER);
                streamWriter = new FileStream(PathToIso, FileMode.Create, FileAccess.Write, FileShare.None, BUFFER);

                byte[] buffer = new byte[BUFFER];

                //Read buffer blocks from source and write them to the ISO file
                do
                {
                    if (bgCreator.CancellationPending)
                    {
                        e.Cancel = true;
                        Stop();
                        break;
                    }

                    streamReader.Read(buffer, 0, BUFFER);
                    streamWriter.Write(buffer, 0, BUFFER);

                    if (OnProgress != null)
                    {
                        //Progress in percent
                        int percent = Convert.ToInt32((streamWriter.Length * 100) / MediumSize);

                        EventIsoArgs eArgs = new EventIsoArgs(streamWriter.Position, percent);

                        OnProgress(eArgs);
                    }
                } while (streamReader.Position == streamWriter.Position);
            }
            catch (Exception ex)
            {
                if (OnMessage != null)
                {
                    EventIsoArgs eArgs = new EventIsoArgs("Error while creating the image: " + ex.Message);
                    OnMessage(eArgs);
                }
            }
            finally
            {
                if (OnFinish != null)
                {
                    EventIsoArgs eArgs = new EventIsoArgs(stopWatch.Elapsed);
                    OnFinish(eArgs);
                }
            }
        }

        /// <summary>
        /// When the file is finished
        /// </summary>
        void bgCreator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseAll();
        }

        /// <summary>
        /// Creates an ISO image from media (CD/DVD)
        /// </summary>
        /// <param name="source">CD/DVD</param>
        /// <param name="destination">Path where the ISO file is to be stored</param>
        /// <returns>
        /// Running = Creation in progress
        /// InvalidHandle = Invalid handle
        /// NoDevice = The source is not a medium (CD/DVD)
        /// NotEnoughMemory = Not enough disk space
        /// LimitExceeded = Source exceeds FAT32 maximum file size of 4 GB (4096 MB)
        /// NotReady = The device is not ready
        /// </returns>
        public IsoState CreateIsoFromMedia(string source, string destination)
        {
            //Is the device ready?
           // if (!new DriveInfo(source).IsReady)
               // return IsoState.NotReady;

            //Source CD/DVD
           // if (new DriveInfo(source).DriveType != DriveType.CDRom)
               // return IsoState.NoDevice;

            //Get medium size
            MediumSize = GetMediumLength(source);

            //Check disk space
            long diskSize = new DriveInfo(Path.GetPathRoot(destination)).AvailableFreeSpace;

            if (diskSize <= MediumSize)
                return IsoState.NotEnoughMemory;

            //Check capacity of > 4096 MB (NTFS)
            if (!CheckNTFS(destination) && MediumSize >= LIMIT)
                return IsoState.LimitExceeded;

            //Create handle
            Handle = Win32.CreateFile(source);

            if (!string.IsNullOrEmpty(destination))
                PathToIso = destination;

            //If invalid or closed handle
            if (Handle.IsInvalid || Handle.IsClosed)
                return IsoState.InvalidHandle;

            //Create thread to create the ISO file
            bgCreator.RunWorkerAsync();

            return IsoState.Running;
        }

        /// <summary>
        /// Aborts the creation of the image and deletes it
        /// </summary>
        public void Stop()
        {
            CloseAll();

            if (File.Exists(PathToIso))
                File.Delete(PathToIso);

            if (OnMessage != null)
            {
                EventIsoArgs e = new EventIsoArgs(@"Creation of the images was canceled");
                OnMessage(e);
            }
        }

        /// <summary>
        /// Closes all streams and handles and frees resources
        /// </summary>
        private void CloseAll()
        {
            if (bgCreator != null)
            {
                bgCreator.CancelAsync();
                bgCreator.Dispose();
            }

            if (streamReader != null)
            {
                streamReader.Close();
                streamReader.Dispose();
            }

            if (streamWriter != null)
            {
                streamWriter.Close();
                streamWriter.Dispose();
            }

            if (Handle != null)
            {
                Handle.Close();
                Handle.Dispose();
            }
        }

        /// <summary>
        /// Size of media (CD/DVD)
        /// </summary>
        /// <param name="drive">Source drive</param>
        /// <returns>Size in bytes</returns>
        private long GetMediumLength(string drive)
        {
            return new DriveInfo(drive).TotalSize;
        }

        /// <summary>
        /// Checks if filesystem is NTFS
        /// </summary>
        /// <param name="destination">Path to ISO file</param>
        /// <returns>True if NTFS</returns>
        private bool CheckNTFS(string destination)
        {
            return new DriveInfo(Path.GetPathRoot(destination)).DriveFormat == "NTFS" ? true : false;
        }
        #endregion
    }

    #region Enumeration
    /// <summary>
    /// Returns state of ISO creation
    /// </summary>
    public enum IsoState
    {
        /// <summary>
        /// Creation running
        /// </summary>
        Running = 1,
        /// <summary>
        /// Invalid handle
        /// </summary>
        InvalidHandle = -1,
        /// <summary>
        /// The source is no CD/DVD media
        /// </summary>
        NoDevice = -2,
        /// <summary>
        /// Not enough memory remaining
        /// </summary>
        NotEnoughMemory = -3,
        /// <summary>
        /// Source exceeds FAT32 maximum file size of 4 GB (4096 MB)
        /// </summary>
        LimitExceeded = -4,
        /// <summary>
        /// The device is not ready
        /// </summary>
        NotReady = -5
    }
    #endregion

    #region EventIsoArgs
    public delegate void IsoEventHandler(EventIsoArgs e);

    /// <summary>
    /// Contains additional data for the event
    /// </summary>
    public class EventIsoArgs : EventArgs
    {
        /// <summary>
        /// Already written bytes
        /// </summary>
        public long WrittenSize { get; private set; }

        /// <summary>
        /// Progress in percent
        /// </summary>
        public int ProgressPercent { get; private set; }

        /// <summary>
        /// Elapsed time
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; private set; }

        public EventIsoArgs(TimeSpan value)
            : base()
        {
            ElapsedTime = value;
        }

        public EventIsoArgs(long value, int percent)
            : base()
        {
            WrittenSize = value;
            ProgressPercent = percent;
        }

        public EventIsoArgs(string value)
            : base()
        {
            Message = value;
        }
    }
    #endregion

    #region Win32
    /// <summary>
    /// Provices required functionality
    /// </summary>
    internal class Win32
    {
        /// <summary>
        /// Read access
        /// </summary>
        static uint GENERIC_READ = 0x80000000;

        /// <summary>
        /// Indicates that subsequent opening operations are successful only when read access to the object is requested
        /// </summary>
        static uint FILE_SHARE_READ = 0x1;

        /// <summary>
        /// Opens the file. Fails if file not exists
        /// </summary>
        static uint OPEN_EXISTING = 0x3;

        /// <summary>
        /// Indicates that the file has no other attributes. This attribute is valid only if it is used alone.
        /// </summary>
        static uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        /// <summary>
        /// Returns handle that can be used to access a file or device in different ways
        /// </summary>
        /// <param name="lpFileName">Name of file or device to be opened</param>
        /// <param name="dwDesiredAccess">Access to requested file or device</param>
        /// <param name="dwShareMode">Requested share mode of file or device</param>
        /// <param name="lpSecurityAttributes">Pointer to a security attribute</param>
        /// <param name="dwCreationDisposition">An action, which is performed on a file or a device if it is present or not present</param>
        /// <param name="dwFlagsAndAttributes">File/device attribute, FILE_ATTRIBUTE_NORMAL is most frequently used</param>
        /// <param name="hTemplateFile">Handle to a template file</param>
        /// <returns>A handle to the device/file</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// Creates the handle of the media
        /// </summary>
        /// <param name="device">Media (CD/DVD)</param>
        /// <returns>Handle of media</returns>
        public static SafeFileHandle CreateFile(string device)
        {
            //Check how drive letter was entered
            //e.g. Z:\ -> Z: else change nothing
            string devName = device.EndsWith(@"\") ? device.Substring(0, device.Length - 1) : device;

            //Create handle
            IntPtr devHandle = CreateFile(string.Format(@"\\.\{0}", devName), GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero,
                OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

            return new SafeFileHandle(devHandle, true);
        }
    }
    #endregion
     * */
