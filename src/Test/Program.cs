// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
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
            DateTime start = DateTime.Now;
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
                foreach (KeyValuePair<DateTime, float> r in values.Where(p => p.Key <= DateTime.Now.AddSeconds(-160)))
                    values.Remove(r.Key);

                diagram.Line.Values = values;
            };

            frame.Components.Add(diagram);
            frame.Components.Add(timer);
            frame.WaitForClose();
        }
    }
}