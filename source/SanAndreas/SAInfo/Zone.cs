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

namespace SanAndreas.SAInfo
{
    public class Zone
    {
        public Zone(string name, double[] points)
        {
            Name = name;
            Points = points;
        }

        public string Name { get; set; }
        public double[] Points { get; set; }

        public override bool Equals(object o)
        {
            try
            {
                return o is Zone && ((Zone) o).Name == Name;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(Zone left, Zone right)
        {
            try
            {
                return left.Equals(right);
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public static bool operator !=(Zone left, Zone right)
        {
            return !(left == right);
        }
    }
}