// MIT License
// 
// Copyright (c) 2024 dotnfc
// 

using System;

namespace SCardSyncDemo
{
    public class Utils
    {
        public static byte[] ExpandByteToEightBytes(byte input)
        {
            byte[] result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = (byte)((input & (1 << i)) >> i);
            }
            return result;
        }

        public static bool[] ExpandByteToEightBools(byte input)
        {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = (((input & (1 << i)) >> i) == 1);
            }
            return result;
        }

        public static byte CompressEightBoolsToByte(bool[] input, int offset)
        {
            if (input == null || input.Length < 8)
            {
                throw new ArgumentException("Input array must contain exactly 8 booleans.");
            }

            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                if (input[i + offset])
                {
                    result |= (byte)(1 << i);
                }
            }
            return result;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0, j = 0; i < numberChars; i += 2, j++)
                bytes[j] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ByteArrayToHexString(byte[] bytes, int length)
        {
            string hexString = BitConverter.ToString(bytes, 0, length);
            return hexString.Replace("-", "");
        }

        public static string ByteArrayToHexString(byte[] bytes, int offset, int length)
        {
            string hexString = BitConverter.ToString(bytes, offset, length);
            return hexString.Replace("-", "");
        }

        public static string byteAsString(byte[] bytes, int startIndex, int length, bool spaceInBetween)
        {
            byte[] newByte;

            if (bytes.Length < startIndex + length)
                Array.Resize(ref bytes, startIndex + length);

            newByte = new byte[length];
            Array.Copy(bytes, startIndex, newByte, 0, length);

            return byteAsString(newByte, spaceInBetween);
        }

        public static string byteAsString(byte[] tempBytes, bool spaceInBetween)
        {
            string tempString = string.Empty;
            int i;

            if (tempBytes == null)
                return "";

            for (i = 0; i < tempBytes.Length; i++)
            {
                tempString += string.Format("{0:X2}", tempBytes[i]);

                if (spaceInBetween)
                    tempString += " ";
            }

            return tempString;
        }

        public static int CountBits(byte value)
        {
            int count = 0;
            while (value != 0)
            {
                count += value & 1;
                value >>= 1;
            }
            return count;
        }
    }
}
