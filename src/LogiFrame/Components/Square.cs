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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable square.
    /// </summary>
    public class Square : Component
    {
        private bool _fill;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is filled.
        /// </summary>
        public bool IsFilled
        {
            get { return _fill; }
            set { SwapProperty(ref _fill, value); }
        }

        /// <summary>
        /// Renders this instance to a <see cref="Snapshot" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            var result = new Snapshot(Size);

            if (IsFilled)
            {
                for (int x = 0; x < Size.Width; x++)
                    for (int y = 0; y < Size.Height; y++)
                        result.SetPixel(x, y, true);
            }
            else
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    result.SetPixel(x, 0, true); //Top
                    result.SetPixel(x, Size.Height - 1, true); //Bottom
                }
                for (int y = 0; y < Size.Height; y++)
                {
                    result.SetPixel(0, y, true); //Left
                    result.SetPixel(Size.Width - 1, y, true); //Right
                }
            }
            return result;
        }
    }
}