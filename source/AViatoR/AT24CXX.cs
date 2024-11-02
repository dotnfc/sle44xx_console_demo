using SCardSyncDemo.WinSCard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCardSyncDemo.AViatoR
{
    public struct I2cCardInfo
    {
        public byte Type;       // card type: SC_I2C_TYPE_XX
        public ushort PageSize; // card page size
        public byte DevAddr;    // i2c dev-addr
        public byte NbrAddr;    // eep address length in bytes
        public uint Size;       // memory size
        public string Name;
    }

    public class AT24CXX : IEnumerable
    {
        private readonly I2cCardInfo[] _cards;

        private static AT24CXX _instance;

        public static AT24CXX Instance => _instance ?? (_instance = new AT24CXX());

        public enum CardType
        {
            AT24C01A = 0x0D,
            AT24C02 = 0x0E,
            AT24C04 = 0x0F,
            AT24C08 = 0x10,
            AT24C16 = 0x11,
            AT24C164 = 0x12,
            AT24C32 = 0x13,
            AT24C64 = 0x14,
            AT24C128 = 0x15,
            AT24C256 = 0x16,
            AT24CS128 = 0x17,
            AT24CS256 = 0x18,
            AT24C512 = 0x19,
            AT24C1024 = 0x1A,
        };

        public AT24CXX() 
        {
            var initData = new[]
            {
                (0x0D, 8, 0x50, 1, 128, "AT24C01A"),
                (0x0E, 8, 0x50, 1, 256, "AT24C02"),
                (0x0F, 16, 0x50, 1, 512, "AT24C04"),
                (0x10, 16, 0x50, 1, 1024, "AT24C08"),
                (0x11, 16, 0x50, 1, 2048, "AT24C16"),
                (0x12, 16, 0x50, 1, 2048, "AT24C164"),
                (0x13, 32, 0x50, 2, 4096, "AT24C32"),
                (0x14, 32, 0x50, 2, 8192, "AT24C64"),
                (0x15, 64, 0x50, 2, 16384, "AT24C128"),
                (0x16, 64, 0x50, 2, 32768, "AT24C256"),
                (0x17, 64, 0x50, 2, 16384, "AT24CS128"),
                (0x18, 64, 0x50, 2, 32768, "AT24CS256"),
                (0x19, 128, 0x50, 2, 65536, "AT24C512"),
                (0x1A, 256, 0x50, 2, 131072, "AT24C1024")
            };

            // 使用LINQ的Select方法来初始化I2cCardInfo数组
            _cards = initData.Select(data => new I2cCardInfo
            {
                Type = (byte)data.Item1,
                PageSize = (ushort)data.Item2,
                DevAddr = (byte)data.Item3,
                NbrAddr = (byte)data.Item4,
                Size = (uint)data.Item5,
                Name = data.Item6
            }).ToArray();
        }

        public int length()
        {
            return _cards.Length;
        }

        public I2cCardInfo this[int index]
        {
            get
            {
                if (index < 0 || index >= _cards.Length)
                    throw new IndexOutOfRangeException("Index is out of range.");
                return _cards[index];
            }
        }

        public I2cCardInfo getCardByType(byte type)
        {
            foreach (var card in _cards)
            {
                if (type == (byte)card.Type)
                {
                    return card;
                }
            }

            throw new IndexOutOfRangeException("card type is illegal.");
        }

        // 实现 IEnumerable<I2cCardInfo> 接口
        public IEnumerator<I2cCardInfo> GetEnumerator()
        {
            foreach (var card in _cards)
            {
                yield return card;
            }
        }

        // 实现 IEnumerable 接口
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
