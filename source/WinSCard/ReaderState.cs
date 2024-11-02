using System;
using System.Runtime.InteropServices;

namespace SCardSyncDemo.WinSCard
{
    /// <summary>
    /// The SCardReaderState structure is used by functions for tracking smart cards within readers.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ReaderState
    {
        /// <summary>
        /// Name of the reader being monitored.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ReaderName;

        /// <summary>
        /// Not used by the smart card subsystem. This member is used by the application.
        /// </summary>
        public IntPtr UserData;

        /// <summary>
        /// Current state of the reader, as seen by the application. This field can take any value of <see cref="Enums.ReaderStates"/>, in combination, as a bitmask.
        /// </summary>
        public int CurrentState;

        /// <summary>
        /// Current state of the reader, as known by the smart card resource manager. This field can take any value of <see cref="Enums.ReaderStates"/>, in combination, as a bitmask.
        /// </summary>
        public int EventState;

        /// <summary>
        /// Number of bytes in the returned ATR.
        /// </summary>
        public int AtrLength;

        /// <summary>
        /// ATR of the inserted card, with extra alignment bytes.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.U1)]
        public byte[] Atr;
    };
}
