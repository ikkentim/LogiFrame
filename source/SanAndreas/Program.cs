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

using LogiFrame;
using LogiFrame.Components.Book;
using SanAndreas.Pages;

namespace SanAndreas
{
    internal class Program
    {
        public Program()
        {
            var frame = new Frame("GTA: San Andreas", true, true, true, true)
            {
                UpdatePriority = UpdatePriority.Normal
            };
            var book = new Book(frame) {MenuButton = 3};
            book.Pages.Add(new Detector());
            book.Pages.Add(new OnFoot());
            book.Pages.Add(new InVehicle());
            book.SwitchTo<OnFoot>();

            frame.WaitForClose();
        }

        private static void Main()
        {
            new Program();
        }
    }
}