using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Your_video_has_x_views
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string getVideoLikes(string url)
        {
            WebClient wc = new WebClient();
            string resp = wc.DownloadString(url);
            richTextBox2.Text = resp;
            int a1 = resp.IndexOf("interactionCount") + 27;
            string b1 = resp.Substring(a1);
            string b2 = b1.Substring(0, b1.IndexOf("\""));
            int count = Convert.ToInt32(b2);
            return count.ToString("N0");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string titleToReplace = textBox5.Text.Replace("#viewCount#", getVideoLikes(textBox1.Text));

                WebClient wc = new WebClient();

                for (int i = 0; i < richTextBox1.Lines.Count(); i++)
                {
                    string toFilt = richTextBox1.Lines[i];
                    string a = toFilt.Substring(0, toFilt.IndexOf(":"));
                    string b = toFilt.Substring(toFilt.IndexOf(":") + 2);
                    if (a.ToLower() == "content-length" || b.ToLower() == "keep-alive")
                    {}
                    else
                    {
                        wc.Headers[a] = b;
                    }
                }

                string data = textBox6.Text;
                int a1 = data.IndexOf("newTitle");
                string b1 = data.Substring(a1);
                string b2 = b1.Substring(0, b1.IndexOf("shouldSegment"));
                string toRepl = b2;
                data = data.Replace(toRepl, "newTitle\":\"" + titleToReplace + "\",\"");
                wc.UploadData(textBox2.Text, "POST",Encoding.ASCII.GetBytes(data));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            try
            {
                string titleToReplace = textBox5.Text.Replace("#viewCount#", getVideoLikes(textBox1.Text));

                WebClient wc = new WebClient();

                for (int i = 0; i < richTextBox1.Lines.Count(); i++)
                {
                    string toFilt = richTextBox1.Lines[i];
                    string a = toFilt.Substring(0, toFilt.IndexOf(":"));
                    string b = toFilt.Substring(toFilt.IndexOf(":") + 2);
                    if (a.ToLower() == "content-length" || b.ToLower() == "keep-alive")
                    {
                        //MessageBox.Show(a + " : NOT IN");
                    }
                    else
                    {
                        wc.Headers[a] = b;
                    }
                }

                string data = textBox6.Text;
                int a1 = data.IndexOf("newTitle");
                string b1 = data.Substring(a1);
                string b2 = b1.Substring(0, b1.IndexOf("shouldSegment"));
                string toRepl = b2;
                data = data.Replace(toRepl, "newTitle\":\"" + titleToReplace + "\",\"");
                wc.UploadString(textBox2.Text, "POST", data);
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value * 1000;
            if (button1.Text == "Start")
            {
                button1.Text = "Stop";
                timer1.Start();
            }
            else
            {
                button1.Text = "Start";
                timer1.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(getVideoLikes("https://www.youtube.com/watch?v=IHENIg8Se7M"));
        }
    }
}
