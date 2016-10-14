

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using System.IO.Compression;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using DiscUtils.Sdi;

namespace Packer
{
   
    public partial class Form1 : Form
    {
        public struct DiscSizes
        {
            public static long DVD1 = 4700000000;
            public static long DVDDL = 8530000000;
        }

        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);
        string path_to = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\Split\\";
        string dest_path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Split\\";
        public Form1()
        {
            
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog2.RootFolder = Environment.SpecialFolder.MyComputer;
            listBox3.SelectionMode = SelectionMode.One;
            comboBox1.SelectedIndex = 1;
            
            listBox3.SetSelected(0, true);
            listBox4.SelectionMode = SelectionMode.One;
            splitContainer1.Panel2MinSize = 160;
            listBox4.SetSelected(0, true);
            splitContainer1.SplitterDistance = this.Width - splitContainer1.Panel2MinSize ;
            textBox1.Size = new System.Drawing.Size(153, 20);
            //folderBrowserDialog1.SelectedPath = @"D:\vghd";
            folderBrowserDialog2.SelectedPath = path_to + DateTime.Now.ToShortDateString() + "II" + DateTime.Now.ToShortTimeString().Replace(':', ';');
            textBox6.Text = folderBrowserDialog2.SelectedPath.ToString();
            Dr d = new Dr();
        }

        void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Potwierdź", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        List<Dr> d= new List<Dr>();
        List<long> e = new List<long>();
        //List<int> dsize = new List<int>();
        double size = 0;
        int current = 0;
        int current_adding = 0;

        long cap = 8530000000;
         
          int archive_count=0;

        private void walk(string root,bool penetrate=true){
            if (root != "")
            {
                DirectoryInfo di = new DirectoryInfo(root);
                FileInfo[] files = null;
                DirectoryInfo[] dirs = null;
                try
                {
                    files = di.GetFiles();
                    dirs = di.GetDirectories();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
                if (files != null)
                {
                    d.Insert(current, new Dr());
                    foreach (FileInfo fi in files)
                    {
                        d[current].add(fi.FullName, -1, fi.Length);
                        size += fi.Length;
                        d[current].size += fi.Length;
                    }
                }
                current++;
                if (dirs != null) foreach (DirectoryInfo di2 in dirs)
                    {
                        if (penetrate == true) walk(di2.FullName, penetrate);
                    }
            }

        }



        private void clearAll()
        {
            listBox1.Items.Clear();
            archive_count = 0;
            e.Clear();
            archive_count = 0;
            d.Clear();
            size = 0;
            current = 0;
            current_adding = 0;
        }
        private void divide()
        {

            int how_many = (int)Math.Ceiling((double)(size / cap));
            for (int a = 0; a < how_many; a++)
            {
                if (e.Count > 0)
                    e.Insert(e.Count, 0); //-1
                else
                    e.Insert(e.Count, 0);
            }
            for (int a00 = 0; a00 <= 1.5 * how_many; a00++)
            {                                      //dopuszczalny 50% narzut płyt ponad obliczony ze wzoru



                for (int a0 = 0; a0 < 50; a0++)                                                           // 50 prob dopchania
                {
                    for (int a = 0; a < d.Count; a++)
                    {
                        for (int b = 0; b < d[a].how_many(); b++)
                        {

                            if (e[current_adding] + d[a].elsize[b] < cap && d[a].added_to[b] == -1)    //element niedodany
                            {
                                d[a].added_to[b] = current_adding;
                                e[current_adding] += d[a].elsize[b];
                            }

                        }
                    }
                }

                current_adding++;                                                                        //po 5 probach bierze nastepna plyte i pakuje
                if (current_adding >= e.Count) e.Insert(e.Count, 0);
            }



            for (int a10 = 0; a10 < e.Count; a10++)
            {
                for (int a1 = 0; a1 < d.Count; a1++)
                {
                    for (int iter = 0; iter < d[a1].added_to.Count; iter++)
                    {
                        if (d[a1].added_to[iter] == a10)
                        {
                            listBox1.Items.Add(d[a1].ph[iter].Replace(folderBrowserDialog1.SelectedPath, ""));
                            if (d[a1].elsize[iter] > 1000000)
                                listBox2.Items.Add(((double)(d[a1].elsize[iter] / 1000000)).ToString("# ###.##"));
                            else
                            {
                                listBox2.Items.Add(((double)((double)d[a1].elsize[iter] / 1000000f)).ToString("0.###"));
                            }

                        }
                        if (d[a1].added_to[iter] == -1) { listBox1.Items.Add(":: BRAK   -> " + d[a1].ph[iter].Replace(folderBrowserDialog1.SelectedPath, "")); }


                    }
                }
                if (e[a10] >= 1) { archive_count++; listBox1.Items.Add("::: Płyta nr " + (a10) + " ma rozmiar " + ((double)((double)e[a10] / 1000000f)).ToString("# ###.##") + "        (" + ((double)((double)e[a10] / (double)cap) * (double)100).ToString("##.##") + "%)"); }

            }
            // e.Clear();
            // d.Clear();

        }
        private void process_tree()
        {
            if (folderBrowserDialog1.SelectedPath != "")
            {
                clearAll();
                walk(folderBrowserDialog1.SelectedPath);
                divide();
            }
        }

        private void button1_Click(object sender, EventArgs ea)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && folderBrowserDialog1.SelectedPath != null)
            {
                textBox5.Text = folderBrowserDialog1.SelectedPath;
                textBox6.Text = folderBrowserDialog2.SelectedPath;
                process_tree();
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

      
        private void button3_Click(object sender, EventArgs e)
        {
            string[] ss;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            int counter = 0;
            foreach (var f in listBox1.Items)
            {
                if (f.ToString()[0] == ':') counter++;
            }
            ss = new string[counter]; 
            //int cnt = 0;
            foreach (var f in listBox1.Items)
            {
                //System.IO.Directory.SetCurrentDirectory("D:\\");
                if (f.ToString()[0] == ':') {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(path_to);
                        if (!di.Exists) di.Create();
                    }
                    catch (Exception) { MessageBox.Show("Run the program as administrator or change folder"); }
                    FileInfo fi = new FileInfo(path_to + cnt + ".txt");
                    if (fi.Exists) fi.Delete();
                    richTextBox1.SaveFile(path_to + cnt + ".txt", RichTextBoxStreamType.PlainText);
                    richTextBox1.Text = "";
                    ss[cnt] = path_to + (cnt) + ".txt";
                    cnt++; }
                //else { ss[cnt] += " " + ; }
                else
                {
                    richTextBox1.AppendText(folderBrowserDialog1.SelectedPath  + f.ToString() + "\n");
                }
            }

            for (int a = 0; a < counter; a++)
            {
                Pack p = new Pack(ss[a], dest_path + "\\" + textBox2.Text + (a + 1));

              
            }


            cnt = counter;
            zrobione = new bool[cnt];

        }
        int cnt = 0;
        string name = "";
        bool[] zrobione;
        System.Windows.Forms.Timer[] timers;
        private void timerr_Tick(object sender, EventArgs ea)
        {
            int ktory = -1, count = 0;
            System.Windows.Forms.Timer t = (System.Windows.Forms.Timer)sender;

            foreach (System.Windows.Forms.Timer tim in timers)
            {
                if (t.GetHashCode() == tim.GetHashCode()) { ktory = count; break; }
                count++;
            }
        }
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
       /* private void wypal(int a)
        {
            //DODAC PETLE
            //int a = 1;
            ISOCreate.Program p = new ISOCreate.Program();
            string[] par = new String[] { folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + (a + 1)+".iso ", folderBrowserDialog2.SelectedPath + "\\" + textBox2.Text + (a +1)+ ".rar " };

            p.create(par);

           // foreach (System.Windows.Forms.Timer t in timers)
           // {
              // t.Enabled = true;                                       ////////// TODO
           // }
            
        }*/


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

       
        private void timer4_Tick(object sender, System.EventArgs e)
        {
            timers = new System.Windows.Forms.Timer[1];
            System.Windows.Forms.Timer t = (System.Windows.Forms.Timer)(sender);
            timers[0] = t;
            MessageBox.Show(t.GetHashCode().ToString()+"   "+timers[0].GetHashCode().ToString());
        }

        private void button2_Click(object sender, System.EventArgs e)
        {

            folderBrowserDialog2.ShowDialog();
            textBox5.Text = folderBrowserDialog1.SelectedPath;
            textBox6.Text = folderBrowserDialog2.SelectedPath;

        }
        int currently_burning=1;
        private void button6_Click(object sender, System.EventArgs e)
        {
            button6.Text = "Wypal płytę nr. "+currently_burning;
                iso_OnFinish(currently_burning);
                if (currently_burning >= this.archive_count) { currently_burning = -1; this.FormClosing -= new FormClosingEventHandler(Form1_FormClosing); }
            button6.Text = "Wypal płytę nr. "+ ++currently_burning;
            
            
        } 
        private void button7_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < archive_count; i++)
            {
                //wypal(i);                                                            /////////////////////////////////////////
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.SelectedPath != null )

             if (comboBox1.SelectedIndex == 0)
                          cap = DiscSizes.DVD1 ; // 4,3gib
            else if (comboBox1.SelectedIndex == 1)
                        cap = DiscSizes.DVDDL; //8,3gib

            if (folderBrowserDialog1.SelectedPath != null) process_tree();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(dest_path);
            }
            catch (Exception)
            {
                MessageBox.Show("Directory not yet created, wait a while for it to create");
            }
        }
    }
    public class Dr
    {
        public List<string> ph;
        public List<int> added_to;
        public List<long> elsize;
       public  long size;
        
        bool all_added;
        public Dr()
        {
            ph = new List<string>();
            added_to = new List<int>();
            elsize = new List<long>();
            size = 0;
            all_added = true;
        }
        public void add(string path1, int where,long size1)
        {
            if (ph.Count > 0)
            {
                ph.Insert(ph.Count , path1); //-1
                added_to.Insert(added_to.Count , where);//-1
                elsize.Insert(elsize.Count ,size1);//-1
                size += size1;
            }
            else
            {
                added_to.Insert(added_to.Count, where);
                ph.Insert(ph.Count, path1);
                elsize.Insert(elsize.Count,size1);
                size += size1;
            }
        }
        public void change(string path1, int new_where)
        {
            int index = ph.IndexOf(path1);
            added_to[index] = new_where;
        }
        public int how_many(){
            return this.added_to.Count;
        }
        
    }
    public class Fl
    {
        public FileInfo fi
        {
            get;
            set;
        }
        public string path
        {
            get;
            set;
        }

        public bool added
        {
            get;
            set;
        }
        public int added_to
        {
            get;
            set;
        }

        public Fl(string path1)
        {
            this.path = path1;
            fi = new FileInfo(path);
            added_to = -1;
            added = false;
        }

    }

    public class Pack
    {
        public static int number = 0;
        public double progress { get { return progr; } set { progr = value; } }
        string path_in;
        string path_out;
        string path_sizes;
        double progr;
        int number1;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public Pack(string path_in, string path_out)
        {
            number++;
            number1 = number;
            bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            this.path_in = path_in;
            this.path_out = path_out;
            t.Interval = 500;
            t.Enabled = true;
            t.Tick += new EventHandler(timer1_Tick);
            bw.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(path_out);
                progr = fi.Length;
            }
            catch (Exception) { }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            pack();
        }
        FileInfo[] files;
        BackgroundWorker bw;
        public void start() { }
        public void pack() {

            string[] lines = File.ReadAllLines(path_in);

            List<string> paths = new List<string>();
            files = new FileInfo[lines.Length];
            long cnt = 0;
            long totalsize = 0;
            foreach (string line in lines)
            {
                if (line.Length > 1) {
                    if (line[0] != ':')
                        //paths.Add(line);
                        files[cnt] = new FileInfo(line);
                    totalsize += files[cnt].Length;
                }
                cnt++;
            }

            DirectoryInfo di = new DirectoryInfo(path_out);
            if (di.Exists == false) di.Create();
            DirectoryInfo empty = new DirectoryInfo(Environment.SpecialFolder.MyDocuments + "\\" + "empty");
            if (empty.Exists == false) empty.Create();
            FileInfo fi = new FileInfo( path_out + "\\" + number1 + ".zip");
            if (fi.Exists) fi.Delete();
           // try
            //{
            //    ZipFile.CreateFromDirectory(empty.FullName, path_out + "\\" + number1 + ".zip");
            //}

            //try
           // {
            //    Directory.CreateDirectory(path_out + "Disc no " + number1);
            //}
           // catch (Exception) { }
            //FileInfo fi = new FileInfo(path_out + "\\" + number + ".zip");
            // if (fi.Exists == false)
            // fi.Create();
            //else
            // {
            //   fi.Delete();
            //   fi.Create();
            // }
            foreach (FileInfo fileToCompress in files)
            {
                FileInfo fi2 = new FileInfo(path_out + "\\" + fileToCompress.Name);
                if(fi2.Exists==false) //|| (fi2.LastWriteTimeUtc< fileToCompress.LastWriteTimeUtc))
                    fileToCompress.CopyTo(path_out  + "\\" + fileToCompress.Name);
            }
            DirectoryInfo save_to = Directory.GetParent(path_out);
            //try
            {
                Directory.CreateDirectory(save_to.FullName + "\\Split"+DateTime.Now.ToShortDateString().Replace('.','/')+" "+DateTime.Now.ToShortTimeString().Replace(':',';'));
                ZipFile.CreateFromDirectory(path_out , save_to.FullName + "\\Split"+DateTime.Now.ToShortDateString().Replace('.', '/') + " " + DateTime.Now.ToShortTimeString().Replace(':', ';') + "\\" + number1 + ".zip");

            }
            //catch (Exception)
            //{

            //}
                /*
                using (FileStream zipToOpen = new FileStream(path_out + "\\" + number + ".zip", FileMode.Open))
                {
                    using (System.IO.Compression.ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(fileToCompress.Name);
                        using (BinaryWriter writer = new BinaryWriter(readmeEntry.Open()))
                        {

                            var reader = fileToCompress.Open(FileMode.Open);
                            byte[] buffer = new byte[32768];
                            int read;
                            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                writer.Write(buffer, 0, read);
                            }
                           


                            //reader.Read(bytes, offset, bytes.GetLength())
                            //reader.Read(bytes, offset, (int)reader.Length);
                            // writer.Write(bytes, offset, (int)reader.Length);
                            //}
                            // ms.Close();
                            // ms.Dispose();
                            reader.Close();
                            }}
                                */

                    
                
            }



            /*using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {

                        using (ZipArchive compressionStream = new GZipStream(compressedFileStream,
                           CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);

                        }
                    }
                    FileInfo info = new FileInfo(this.path_out + "\\" + fileToCompress.Name + ".gz");

                }
                }
        */
        




    



            /*using (FileStream stream = File.Open(path_out, FileMode.Create))
            {
                GZipStream obj = new GZipStream(stream, CompressionMode.Compress);


            foreach (string path in paths)
            {
                byte[] bt = File.ReadAllBytes(path);
                obj.Write(bt, 0, bt.Length);
            }
                obj.Close();
                obj.Dispose();
            }

        }*/
        
    }




}

