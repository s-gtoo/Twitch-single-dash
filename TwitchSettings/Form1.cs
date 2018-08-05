using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace TwitchSettings
{
    public partial class Form1 : Form
    {
        public string channelID_returned;
        public bool saved = false;
        public Form1()
        {
            InitializeComponent();

            this.channelID.Text = Properties.Settings.Default["channelName"].ToString();
            this.authToken.Text = Properties.Settings.Default["authToken"].ToString();
            this.clientID.Text = Properties.Settings.Default["clientID"].ToString();
            this._getChannelId();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.CenterToScreen();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!this.saved)
            {
                MessageBox.Show("First, you have to set up the application");
            }
            else
            {
                this.Request(
                    "https://api.twitch.tv/kraken/channels/" + this.channelID_returned,
                    "{\"channel\": {\"status\": \""+ this.status.Text + "\", \"game\": \"" + this.games.Text + "\"}}",
                    //{ channel: { '" + this.status.Text + "', '" + this.games.Text + "' } }",
                    "application/json",
                    "PUT");
                MessageBox.Show("Success");
            }
        }
        private bool _getChannelId()
        {
            if(this.channelID.Text.Length > 0)
            {
                dynamic twichReturn = JsonConvert.DeserializeObject(this.Get("https://api.twitch.tv/kraken/users?login=" + this.channelID.Text));
                if(twichReturn.users.Count > 0)
                {
                    this.channelID_returned = twichReturn.users[0]._id;
                    this.saved = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("Wrong settings (Can't get the user) #_getChannelId");
                    return false;
                }
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(this.channelID.TextLength > 0 && this.authToken.TextLength > 0 && this.clientID.TextLength > 0)
            {
                Properties.Settings.Default["channelName"] = this.channelID.Text;
                Properties.Settings.Default["authToken"] = this.authToken.Text;
                Properties.Settings.Default["clientID"] = this.clientID.Text;
                Properties.Settings.Default.Save();
                if (this._getChannelId())
                {
                    MessageBox.Show("Validation Success, Connected to Twitch API");
                }
            }
        }
        private string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            request.Accept = "application/vnd.twitchtv.v5+json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization: OAuth " + this.authToken.Text);
            request.Headers.Add("Client-ID: " + this.clientID.Text);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        private string Request(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = "application/vnd.twitchtv.v5+json";
            request.Headers.Add("Authorization: OAuth " + this.authToken.Text);
            request.Headers.Add("Client-ID: " + this.clientID.Text);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void channelID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
