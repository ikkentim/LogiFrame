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
using System.ComponentModel;

namespace LogiFrame
{
    /// <summary>
    ///     Represents errors that occur during a connection with a lcd device.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        public ConnectionException(int error) : base("failed to connect to device", new Win32Exception(error))
        {
        }
    }
}