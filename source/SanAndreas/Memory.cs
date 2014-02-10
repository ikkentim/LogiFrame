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
using System.Runtime.InteropServices;
using System.Text;

namespace SanAndreas
{
    /// <summary>
    /// Represents a certain part of memory in a specific process.
    /// </summary>
    public class Memory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Memory class.
        /// </summary>
        /// <param name="process">The process this Memory should read.</param>
        public Memory(Process process)
            : this(process, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Memory class.
        /// </summary>
        /// <param name="process">The process this Memory should read.</param>
        /// <param name="address">The address of this Memory within the process.</param>
        public Memory(Process process, int address)
        {
            if (process == null)
                throw new ArgumentNullException("process cannot be null.");

            Process = process;
            Address = address;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the process of this memory.
        /// </summary>
        public Process Process { get; private set; }

        /// <summary>
        /// Gets the address of this Memory.
        /// </summary>
        public int Address { get; private set; }

        /// <summary>
        /// Gets whether this Memory address is pointing anywhere.
        /// </summary>
        public bool IsPointing
        {
            get { return AsInteger() != 0; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the Memory where a certain module is starting.
        /// Returns null if not found.
        /// </summary>
        /// <param name="moduleName">The module whose Memory to find.</param>
        /// <returns>The Memory where a certain module is starting.</returns>
        public Memory GetModule(string moduleName)
        {
            try
            {
                return
                    Process.Modules.Cast<ProcessModule>()
                        .Where(
                            module =>
                                String.Compare(module.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase) == 0)
                        .Select(module => new Memory(Process, (int) module.BaseAddress))
                        .FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the Memory at the addess the current Memory is pointing to.
        /// </summary>
        /// <returns>The Memory at the address the current Memory is pointing to.</returns>
        public Memory GetPointingAddress()
        {
            return Process.HasExited ? null : new Memory(Process, AsInteger());
        }

        /// <summary>
        /// Returns the Memory at a certain address at a offset from the current Address.
        /// </summary>
        /// <param name="offset">The offset of the Memory from the current Address.</param>
        /// <returns></returns>
        public Memory GetOffsetAddress(int offset)
        {
            return Process.HasExited ? null : new Memory(Process, Address + offset);
        }

        /// <summary>
        /// Returns the Memory at a certain address in the Process.
        /// </summary>
        /// <param name="address">The address of the Memory.</param>
        /// <returns>The Memory at the given address.</returns>
        public Memory GetAddress(int address)
        {
            return Process.HasExited ? null : new Memory(Process, address);
        }

        #endregion

        #region Type converters

        /// <summary>
        /// Returns a integer from this Memory Address.
        /// </summary>
        /// <returns>A integer from this Memory Address.</returns>
        public int AsInteger()
        {
            return Process.HasExited ? 0 : BitConverter.ToInt32(MemoryAccess.Read(Process, Address, 4), 0);
        }

        /// <summary>
        /// Returns a float from this Memory Address.
        /// </summary>
        /// <returns>A float from this Memory Address.</returns>
        public float AsFloat()
        {
            return Process.HasExited ? 0.0f : BitConverter.ToSingle(MemoryAccess.Read(Process, Address, 4), 0);
        }

        /// <summary>
        /// Returns a byte from this Memory Address.
        /// </summary>
        /// <returns>A byte from this Memory Address.</returns>
        public byte AsByte()
        {
            return Process.HasExited ? (byte) 0 : MemoryAccess.Read(Process, Address, 1)[0];
        }

        /// <summary>
        /// Returns a short from this Memory Address.
        /// </summary>
        /// <returns>A short from this Memory Address.</returns>
        public short AsShort()
        {
            return Process.HasExited ? (short) 0 : BitConverter.ToInt16(MemoryAccess.Read(Process, Address, 2), 0);
        }

        /// <summary>
        /// Returns a string of a certain length from this Memory Address.
        /// </summary>
        /// <param name="length">The lenth of the string.</param>
        public string AsString(int length)
        {
            return AsString(length, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a string of a certain length from this Memory Address with a specific encoding.
        /// </summary>
        /// <param name="length">The lenth of the string.</param>
        /// <param name="encoding">The Encoding of the string.</param>
        /// <returns></returns>
        public string AsString(int length, Encoding encoding)
        {
            if (Process.HasExited)
                return String.Empty;

            var value = MemoryAccess.Read(Process, Address, length);

            var stringLength = value.ToList().FindLastIndex(b => b > 0);

            if (stringLength == 0)
                return String.Empty;

            Array.Resize(ref value, stringLength + 1);
            return encoding.GetString(value);
        }

        #endregion

        #region Type operators

        public static explicit operator int(Memory memory)
        {
            return memory == null ? 0 : memory.AsInteger();
        }

        public static explicit operator float(Memory memory)
        {
            return memory == null ? 0.0f : memory.AsFloat();
        }

        public static explicit operator byte(Memory memory)
        {
            return memory == null ? (byte) 0 : memory.AsByte();
        }

        public static explicit operator short(Memory memory)
        {
            return memory == null ? (short) 0 : memory.AsShort();
        }

        public static implicit operator Memory(Process process)
        {
            return process == null ? null : new Memory(process);
        }

        #endregion

        #region Modifying operators

        public static Memory operator ^(Memory memory, int address)
        {
            return memory == null ? null : memory.GetAddress(address);
        }

        public static Memory operator ^(Memory memory, string module)
        {
            return memory == null ? null : memory.GetModule(module);
        }

        public static Memory operator +(Memory memory, int offset)
        {
            return memory == null ? null : memory.GetOffsetAddress(offset);
        }

        public static Memory operator -(Memory memory, int offset)
        {
            return memory == null ? null : memory.GetOffsetAddress(-offset);
        }

        public static Memory operator ~(Memory memory)
        {
            return memory == null ? null : memory.GetPointingAddress();
        }

        #endregion

        #region Reference checks

        public static bool operator ==(Memory left, Memory right)
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

        public static bool operator !=(Memory left, Memory right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (!(obj is Memory))
                return false;

            var memory = obj as Memory;

            return Process.Id == memory.Process.Id && Address == memory.Address;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash*7) + Process.Id.GetHashCode();
                hash = (hash*7) + Address.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="T:System.Object"/>.</returns>
        public override string ToString()
        {
            return Process.ProcessName + "@0x" + Address.ToString("X");
        }

        #endregion

        #region Internal classes

        /// <summary>
        /// Contains methods for accessing memory of different processes.
        /// </summary>
        private static class MemoryAccess
        {
            [DllImport("kernel32.dll")]
            private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
                [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
                uint nSize, out int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,
                int dwSize, out int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            private static extern Int32 CloseHandle(IntPtr hProcess);


            public static byte[] Read(Process process, int address, int numOfBytes, out int bytesRead)
            {
                IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, process.Id);
                byte[] buffer = new byte[numOfBytes];
                ReadProcessMemory(hProc, new IntPtr(address), buffer, numOfBytes, out bytesRead);
                return buffer;
            }

            public static byte[] Read(Process process, int address, int numOfBytes)
            {
                int bytesRead;
                return Read(process, address, numOfBytes, out bytesRead);
            }

            /*
             * So far unused. Planning on being able to set the value of the current Memory.
             * 
            public static void Write(Process process, int address, byte[] bytes)
            {
                IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, process.Id);
                int numOfBytes;
                WriteProcessMemory(hProc, new IntPtr(address), bytes, (UInt32)bytes.LongLength, out numOfBytes);
                CloseHandle(hProc);
            }
            */
        }

        /// <summary>
        /// Represents a value of access level to a process.
        /// </summary>
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        #endregion
    }
}