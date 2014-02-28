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
using System.Linq;
using LogiFrame;
using LogiFrame.Components;

namespace Test
{
    internal static class Program
    {
        private static void Main()
        {
            var frame = new Frame("LogiFrame test application", false, false, true, true);
            var start = DateTime.Now;
            var cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
            var diagram = new Diagram<DateTime, float>
            {
                Size = Frame.LCDSize,
                XAxisLabel = (min, max) => "",
                YAxisLabel = (min, max) => max.ToString("#")
            };
            diagram.Line.XAxisConverter = axisObject => (int) Math.Round((axisObject - start).TotalSeconds);
            diagram.Line.YAxisConverter = axisObject => (int) Math.Round(axisObject*100);
            diagram.Line.MinYAxis = axisObject => 0;
            diagram.Line.MaxYAxis = axisObject => axisObject < 50 ? (axisObject < 25 ? 25 : 50) : 100;
            diagram.Line.MinXAxis = axisObject => DateTime.Now.AddSeconds(-160);
            diagram.Line.MaxXAxis = axisObject => DateTime.Now;

            var timer = new Timer
            {
                Enabled = true,
                Interval = 500
            };
            timer.Tick += (sender, args) =>
            {
                var values = new DiagramDataCollection<DateTime, float>(diagram.Line.Values)
                {
                    {DateTime.Now, cpuCounter.NextValue()}
                };
                foreach (var r in values.Where(p => p.Key <= DateTime.Now.AddSeconds(-160)))
                    values.Remove(r.Key);

                diagram.Line.Values = values;
            };

            frame.Components.Add(diagram);
            frame.Components.Add(timer);
            frame.WaitForClose();
        }
    }
}