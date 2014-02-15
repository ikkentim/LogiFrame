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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using LogiFrame;
using LogiFrame.Components;
using LogiFrame.Components.Book;
using Newtonsoft.Json;

namespace Bitonic
{
    internal class Program
    {
        public Program()
        {
            var frame = new Frame("Bitonic", true, true);
            var exchangePage = new ExchangePage();
            var buyPage = new GraphPage("Buy graph");
            var sellPage = new GraphPage("Sell graph");

            var book = new Book(frame)
            {
                MenuButton = 3,
                Pages =
                {
                    exchangePage,
                    buyPage,
                    sellPage
                }
            };
            book.SwitchTo<ExchangePage>();

            var timer = new Timer
            {
                Interval = 60*1000,
                Enabled = true
            };

            timer.Tick += (sender, e) =>
            {
                try
                {
                    WebClient client = new WebClient();
                    decimal sellrate =
                        Convert.ToDecimal(
                            ((dynamic)
                                JsonConvert.DeserializeObject(
                                    client.DownloadString("https://bitonic.nl/json/sell?part=offer&check=btc&btc=1")))[
                                        "euros"]);
                    decimal buyrate =
                        Convert.ToDecimal(
                            ((dynamic)
                                JsonConvert.DeserializeObject(
                                    client.DownloadString(
                                        "https://bitonic.nl/json/?part=rate_convert&check=btc&btc=1&method=ideal")))[
                                            "euros"]);
                    buyPage.ReportRate(buyrate);
                    sellPage.ReportRate(sellrate);
                    exchangePage.ReportRate(buyrate, sellrate);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };

            timer.OnTick(EventArgs.Empty);

            frame.Components.Add(timer);
            frame.WaitForClose();
        }


        private static void Main()
        {
            new Program();
        }

        private class ExchangePage : Page
        {
            private readonly Label _label;

            public ExchangePage()
            {
                Components.Add(_label = new Label
                {
                    Location = new Location(2, 2),
                    AutoSize = true,
                    Font = new Font("Arial", 8f, FontStyle.Bold),
                    Text = "Loading exchange rate..."
                });
            }

            public void ReportRate(decimal buy, decimal sell)
            {
                _label.Text = string.Format("Bitonic\nSELL: {0:0.00}\nBUY: {1:0.00}", Math.Round(sell, 2),
                    Math.Round(buy, 2));
            }

            public override string GetName()
            {
                return "Exchange rate";
            }

            protected override PageIcon GetPageIcon()
            {
                return new PageIcon(new Component[]
                {
                    new Label
                    {
                        Location = new Location(1, 0),
                        AutoSize = true,
                        Text = "E",
                        Font = new Font("Arial", 10f, FontStyle.Bold),
                        UseCache = true
                    }
                });
            }
        }

        private sealed class GraphPage : Page
        {
            private readonly Diagram<DateTime, decimal> _diagram;
            private readonly Label _label;
            private readonly DateTime _startTime = DateTime.Now;
            private readonly string _title;

            public GraphPage(string title)
            {
                _title = title;
                Components.Add(_diagram = new Diagram<DateTime, decimal>
                {
                    Size = Size,
                    XAxisLabel = (low, high) => low.ToString("HH:mm") + "-" + high.ToString("HH:mm"),
                    YAxisLabel = (low, high) => string.Format("{0:0}-{1:0}", Math.Round(low, 2), Math.Round(high, 2))
                });
                Components.Add(_label = new Label
                {
                    Location = new Location(Size.Width + 1, 0),
                    HorizontalAlignment = Alignment.Right,
                    VerticalAlignment = Alignment.Top,
                    Font = new Font("Arial", 7f),
                    Transparent = true,
                    TopEffect = true,
                    AutoSize = true,
                });
                _diagram.Line.XAxisConverter = axisObject => (int) (axisObject - _startTime).TotalSeconds;
                _diagram.Line.YAxisConverter = axisObject => (int) Math.Round(axisObject*100);
                _diagram.Line.MinYAxis = Math.Floor;
                _diagram.Line.MaxYAxis = Math.Ceiling;
            }

            public void ReportRate(decimal rate)
            {
                foreach (var r in _diagram.Line.Values.Where(p => p.Key < DateTime.Now.AddHours(-6)).Select(p => p.Key))
                    _diagram.Line.Values.Remove(r);
                _label.Text = string.Format("{1}:{0:0.00}", Math.Round(rate, 2),
                    _title.First().ToString(CultureInfo.InvariantCulture));
                _diagram.Line.Values.Add(DateTime.Now, rate);
            }

            public override string GetName()
            {
                return _title;
            }

            protected override PageIcon GetPageIcon()
            {
                return new PageIcon(new Component[]
                {
                    new Label
                    {
                        Location = new Location(1, 0),
                        AutoSize = true,
                        Text = _title.First().ToString(CultureInfo.InvariantCulture),
                        Font = new Font("Arial", 10f, FontStyle.Bold),
                        UseCache = true
                    }
                });
            }
        }
    }
}