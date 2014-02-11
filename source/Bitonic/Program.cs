using System;
using System.Drawing;
using System.Net;
using LogiFrame;
using LogiFrame.Components;
using Newtonsoft.Json;

namespace Bitonic
{
    internal static class Program
    {
        private static readonly Label Label = new Label();
        private static readonly Timer Timer = new Timer();
        private static readonly Frame Frame = new Frame("Bitonic", true, true);

        private static void Main()
        {
            //Setup the style of the Labels
            Label.AutoSize = true;
            Label.Font = new Font("Arial", 8f, FontStyle.Bold);
            Label.Location = new Location(0, 2);
            Label.Text = "";

            //Listen to the Tick-event of the Timer and set the interval
            Timer.Tick += delegate { Update(); };
            Timer.Interval = 60 * 1000;
            Timer.Enabled = true;

            Frame.Components.Add(Label);
            Frame.Components.Add(Timer);

            Frame.ButtonDown += (sender, e) => Update();
            Update();

            //Let the current thread wait untill the Frame is Closed by disposure. (Frame.Dispose, or process ended)
            Frame.WaitForClose();
        }

        private static void Update()
        {
            try
            {
                //Request the exchange rate
                WebClient client = new WebClient();
                string sell = client.DownloadString("https://bitonic.nl/json/sell?part=offer&check=btc&btc=1");
                string buy = client.DownloadString("https://bitonic.nl/json/?part=rate_convert&check=btc&btc=1&method=ideal");

                //Update the label
                Label.Text = "Bitonic\nSELL: " +
                             ((dynamic)JsonConvert.DeserializeObject(sell))["euros_formatted"] + "\nBUY: " +
                             ((dynamic)JsonConvert.DeserializeObject(buy))["euros_formatted"]
                             ;
            }
            catch (Exception)
            {
                Label.Text = "Bitonic\nFailed requesting exchange rate";
            }
        }
    }
}
