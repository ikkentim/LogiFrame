// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
using System.Drawing;
using System.Linq;
using System.Net;
using Bitcoin.Properties;
using LogiFrame;
using LogiFrame.Components;
using Newtonsoft.Json;

namespace Bitcoin
{
    internal static class Program
    {
        private static readonly Label Label = new Label();
        private static readonly Timer Timer = new Timer();
        private static readonly Frame Frame = new Frame("Bitcoin", true, true);
        private static readonly string[] Currencies = { "USD", "EUR", "JPY", "CAD", "GBP", "CHF", "RUB", "AUD", "SEK", "DKK", "HKD", "PLN", "CNY", "SGD", "THB", "NZD", "NOK" };

        private static void Main()
        {
            //Setup the style of the Labels
            Label.AutoSize = true;
            Label.Font = new Font("Arial", 8f, FontStyle.Bold);
            Label.Location = new Location(0, 5);
            Label.Text = "";

            //Listen to the Tick-event of the Timer and set the interval
            Timer.Tick += delegate { Update(); };
            Timer.Interval = 60 * 1000;
            Timer.Enabled = true;

            Frame.Components.Add(Label);
            Frame.Components.Add(Timer);

            Frame.ButtonDown += delegate
            {
                //Update to the next currency in the array
                Settings.Default.Currency =
                    Currencies[(Currencies.ToList().IndexOf(Settings.Default.Currency) + 1)%Currencies.Length];
                Settings.Default.Save();

                Update();
            };
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
                string json = client.DownloadString("http://data.mtgox.com/api/2/BTC" + Settings.Default.Currency + "/money/ticker_fast");

                //Update the label
                Label.Text = "MT.GOX\n1 BTC : " +
                             ((dynamic) JsonConvert.DeserializeObject(json))["data"]["last_local"]["display_short"];
            }
            catch (Exception)
            {
                Label.Text = "MT.GOX\nFailed requesting exchange rate";
            }
        }
    }
}