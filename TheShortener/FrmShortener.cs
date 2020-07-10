using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheShortener
{
    public partial class FrmShortener : Form
    {
        public FrmShortener()
        {
            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals(string.Empty))
                return;
            string longUrl = textBox1.Text.Trim();
            BitlyAPI.BitlyAPI api = new BitlyAPI.BitlyAPI();
            textBox2.Text = await api.ShortenAsync(longUrl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var urlText = textBox2.Text.Split(' ');
            var urlToClipboard = urlText[0];
            Clipboard.SetText(urlToClipboard);
            //Clipboard.SetText(textBox2.Text);
            textBox2.Text = urlToClipboard + " ---> It has been copied to your clipboard!";
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Select();
        }
    }
}
