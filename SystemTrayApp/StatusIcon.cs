using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using SystemTrayApp.Properties;

namespace SystemTrayApp
{
	/// <summary>
	/// 
	/// </summary>
	class StatusIcon : IDisposable
	{
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;
        HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public StatusIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();

            client = new HttpClient();
            client.BaseAddress = new Uri("http://candle:8080");
            client.DefaultRequestHeaders.Add("User-Agent", "Anything");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = GetIcon("");
			ni.Text = "Status Icon";
			ni.Visible = true;

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();

            Timer MyTimer = new Timer();
            MyTimer.Interval = (1000); // 1 second
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();

        }

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
			// Handle mouse button clicks.
			if (e.Button == MouseButtons.Left)
			{
				// Start Windows Explorer.
				Process.Start("explorer", null);
			}
		}


        private void MyTimer_Tick(object sender, EventArgs e)
        {

            String text = "";
            var response = client.GetAsync("xabcdDistanceRest/getFormingXabcds?noCounts=true&type=1").Result;
            if (response.IsSuccessStatusCode) {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;

                JObject o = JObject.Parse(responseString);
                JToken acme = o.SelectToken("$.count");
                int result = Int32.Parse(acme.ToString());

                if (result > 0) {
                    text = text + result;

                    ni.Icon = GetIcon(text);
                    ni.Text = text;

                    if (result > 88) {
                        ni.Icon = GetIcon("88");
                    }
                }
            }
            
        }


        public static System.Drawing.Icon GetIcon(string text)
        {
            //Create bitmap, kind of canvas
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(32, 32);

            Icon icon = Resources.Status;
            System.Drawing.Font drawFont = new System.Drawing.Font("Calibri", 18, FontStyle.Regular);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.DrawIcon(icon, 0, 0);
            graphics.DrawString(text, drawFont, drawBrush, 0, 0);

            //To Save icon to disk
            bitmap.Save("icon.ico", System.Drawing.Imaging.ImageFormat.Icon);

            Icon createdIcon = Icon.FromHandle(bitmap.GetHicon());

            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();

            return createdIcon;
        }
    }
}