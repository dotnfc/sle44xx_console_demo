using HidGlobal.OK.SampleCodes.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SCardSyncDemo.WinSCard
{
    public class ReaderManager
    {
        private static WinscardReader _instance;

        //public static ReaderManager Instance => _instance ?? (_instance = new ReaderManager());
        
        public static WinscardReader Instance => Get();

        public ReaderManager() {
            
        }

        public static void NotPrintMessage(string message) { }

        public static WinscardReader Get()
        {
            if (_instance == null)
            {
                _instance = new WinscardReader(NotPrintMessage); //  ConsoleWriter.Instance.PrintMessage);
            }

            return _instance;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="readerStates"></param>
        public static IReadOnlyList<ReaderState> GetStatusChange(int timeout, IReadOnlyList<ReaderState> readerStates)
        {
            var temporaryStructArray = readerStates.ToArray();

            Get().GetStatusChange(timeout, ref temporaryStructArray, temporaryStructArray.Length);

            return temporaryStructArray;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="readerNamesWithStatesDictionary"></param>
        /// <returns><see cref="T:HidGlobal.OK.Readers.Components.ReaderState" /></returns>
        public static IReadOnlyList<ReaderState> GetReaderState(IReadOnlyDictionary<string, ReaderStates> readerNamesWithStatesDictionary)
        {
            var list = new List<ReaderState>(readerNamesWithStatesDictionary.Count);
            list.AddRange(readerNamesWithStatesDictionary.Select(keyValuePair => new ReaderState
            {
                Atr = new byte[36],
                AtrLength = 36,
                CurrentState = (int)keyValuePair.Value,
                ReaderName = keyValuePair.Key,
            }));

            return GetStatusChange(1000, list);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="stateOfTheReader"></param>
        /// <returns><see cref="T:HidGlobal.OK.Readers.Components.ReaderState" /></returns>
        public static ReaderState GetReaderState(string reader, ReaderStates stateOfTheReader)
        {
            return GetReaderState(new Dictionary<string, ReaderStates> { { reader, stateOfTheReader } })[0];
        }
    }
}
