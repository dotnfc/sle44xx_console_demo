/*****************************************************************************************
    (c) 2017-2018 HID Global Corporation/ASSA ABLOY AB.  All rights reserved.

      Redistribution and use in source and binary forms, with or without modification,
      are permitted provided that the following conditions are met:
         - Redistributions of source code must retain the above copyright notice,
           this list of conditions and the following disclaimer.
         - Redistributions in binary form must reproduce the above copyright notice,
           this list of conditions and the following disclaimer in the documentation
           and/or other materials provided with the distribution.
           THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
           AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
           THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
           ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
           FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
           (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
           LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
           ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
           (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
           THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*****************************************************************************************/
using SCardSyncDemo;
using SCardSyncDemo.AViatoR;
using SCardSyncDemo.WinSCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static HidGlobal.OK.SampleCodes.AViatoR.SynchronusI2C;


namespace HidGlobal.OK.SampleCodes.AViatoR
{
    static class ContactCardCommunicationSample
    {
        private static void PrintData(string title, string command, string response, string data)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"<-- {command}\n--> {response}\n{title}: {data}");
        }
        /// <summary>
        /// Establishes connection between given reader and a smart card and returns object implementing IReader interface 
        /// capable of interaction with a card, throws an exception if no card available.
        /// </summary>
        /// <param name="readerName">Reader name seen by smart card resource manager.</param>
        /// <returns></returns>
        private static WinscardReader Connect(string readerName)
        {
            ReaderState readerState = ReaderManager.GetReaderState(readerName, ReaderStates.Unaware);
            if (readerState.AtrLength > 0)
            {
                ReaderManager.Get().Connect(readerName);
                return ReaderManager.Get();
            }
            throw new Exception("No Smart Card Available in contact slot.");
        }
        public static void ReadMainMemory2WbpExample(string readerName)
        {
            try
            {
                var twoWireBusProtocol = new Synchronus2WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;

                // address of first byte to be read
                byte address = 0;
                byte notUsed = 0x00;
                string cardMemory = string.Empty;
                for (int i = 0; i < 256; i++)
                {
                    string command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.ReadMainMemory, address, notUsed);
                    string response = smartCardReader.OKTransmit(command);
                    if (response.StartsWith("9D01") && response.EndsWith("9000"))
                    {
                        string data = response.Substring(4, 2);
                        PrintData($"Read Main Memory, Address 0x{address:X2}", command, response, $"Value 0x{data}");
                        cardMemory += data;
                    }
                    else
                    {
                        PrintData($"Read Main Memory, Address 0x{address:X2}", command, response, "Error Response");
                    }
                    address++;
                }
                //Console.WriteLine($"\nMain Memory starting from address 0x20 to address 0xFF:\n{cardMemory}\n");
                Console.WriteLine($"\nMain Memory starting from address 0x00 to address 0xFF:\n");
                PrintByteArray(Utils.HexStringToByteArray(cardMemory), null, 0);

                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static bool getInput(ref string sInput)
        {
            string input = "";
            bool escapePressed = false;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    escapePressed = true;
                    break;
                }
                else if (keyInfo.KeyChar >= '0' && keyInfo.KeyChar <= '9' ||
                         (keyInfo.KeyChar >= 'A' && keyInfo.KeyChar <= 'F') ||
                         (keyInfo.KeyChar >= 'a' && keyInfo.KeyChar <= 'f') ||
                         (keyInfo.KeyChar == ' '))
                {
                    input += keyInfo.KeyChar;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1); // 删除最后一个字符
                }
            }

            if (escapePressed)
            {
                return true;
            }

            sInput = input;

            return false;
        }

        public static bool getUserPin(int length, ref byte[]arrayPINs)
        {
            bool ret;
            string sInput = "";

            string strDefault = (length == 2) ? "FFFF" : "FFFFFF";

            Console.WriteLine($"Please Input {length} PSC PIN(Default to {strDefault})：");

            while (true)
            {
                ret = getInput(ref sInput);
                if (ret) 
                {// user canceled
                    return true;
                }

                sInput.Replace(" ", "");
                arrayPINs = Utils.HexStringToByteArray(sInput);
                if ( (arrayPINs.Length == 0) || (arrayPINs.Length == length) )
                {
                    if (sInput.Length == 0)
                    {
                        arrayPINs = Utils.HexStringToByteArray(strDefault);
                    }
                    break;
                }
                else
                { 
                    Console.WriteLine($"Invalid PIN {sInput}, Retry.");
                    continue;
                }
            }

            return false;
        }

        public static void Verify2WbpExample(string readerName)
        {
            try
            {
                var twoWireBusProtocol = new Synchronus2WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;

                byte[] pins = null;
                if (!getUserPin(3, ref pins))
                {
                    VerifyUser2Wbp(smartCardReader, pins[0], pins[1], pins[2]);
                }

                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        public static void ReadProtectionMemory2WbpExample(string readerName)
        {
            try
            {
                var twoWireBusProtocol = new Synchronus2WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;

                byte notUsed = 0x00;

                string command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.ReadProtectionMemory, notUsed, notUsed);
                string response = smartCardReader.OKTransmit(command);
                if (response.StartsWith("9D04") && response.EndsWith("9000"))
                {
                    string data = response.Substring(4, 8);
                    PrintData("Read Protection Memory", command, response, $"Value 0x{data}");
                }
                else
                {
                    PrintData("Read Protection Memory", command, response, "Error Response");
                }
                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateMainMemory2WbpExample(string readerName)
        {
            try
            {
                var twoWireBusProtocol = new Synchronus2WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;
                // Following code is commented to prevent usage of incorrect Pin code, which can lead to blocking the synchronus card if done three times in row
                // Verification with correct pin code is necessary to write any data into card memory
                // Be advice not to use VerifyUser2Wbp command if correct pin code is not known 

                /*
                string pin = "FFFFFF";

                byte firstPinByte = byte.Parse(pin.Substring(0, 2), NumberStyles.HexNumber);
                byte secondPinByte = byte.Parse(pin.Substring(2, 2), NumberStyles.HexNumber);
                byte thirdPinByte = byte.Parse(pin.Substring(4, 2), NumberStyles.HexNumber);

                VerifyUser2Wbp(reader, firstPinByte, secondPinByte, thirdPinByte);
                //*/
                Random random = new Random();
                byte value = (byte)random.Next(0, 256);

                byte[] pins = null;
                if (!getUserPin(3, ref pins))
                {
                    VerifyUser2Wbp(smartCardReader, pins[0], pins[1], pins[2]);
                }

                byte address = 0x20;
                byte data = value;

                string command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.UpdateMainMemory, address, data);
                string response = smartCardReader.OKTransmit(command);
                PrintData($"Write Main Memory, address: 0x{address:X2}, data: 0x{data:X2}", command, response, response.Equals("9D009000") ? "Success" : "Error Response");

                address = 0x21;
                data = value;
                command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.UpdateMainMemory, address, data);
                response = smartCardReader.OKTransmit(command);
                PrintData($"Write Main Memory, address: 0x{address:X2}, data: 0x{data:X2}", command, response, response.Equals("9D009000") ? "Success" : "Error Response");

                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void VerifyUser2Wbp(WinscardReader smartCardReader, byte firstPinByte, byte secondPinByte, byte thirdPinByte)
        {
            var twoWireBusProtocol = new Synchronus2WBP();

            byte notUsed = 0x00;
            byte newErrorCounter;
            Console.WriteLine("User Verification");

            // Read Error Counter
            string command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.ReadSecurityMemory, notUsed, notUsed);
            string response = smartCardReader.OKTransmit(command);
            string currentErrorCounter = response.Substring(4, 2);
            PrintData("Read Error Counter", command, response, $"0x{currentErrorCounter}");

            // decrement counter
            switch (currentErrorCounter)
            {
                case "07":
                    newErrorCounter = 0x06;
                    break;
                case "06":
                    newErrorCounter = 0x04;
                    break;
                case "04":
                    newErrorCounter = 0x00;
                    break;
                default:
                    Console.WriteLine("Returned error counter is not correct or card is blocked");
                    return;
            }
            command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.UpdateSecurityMemory, 0x00, newErrorCounter);
            response = smartCardReader.OKTransmit(command);
            PrintData("Write new Error Counter", command, response, $"0x{newErrorCounter:X2}");

            // Compare verification data - first part
            command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.CompareVerificationData, 0x01, firstPinByte);
            response = smartCardReader.OKTransmit(command);
            PrintData("Compare verification data - first part", command, response, $"{firstPinByte:X2}");

            // Compare verification data - second part
            command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.CompareVerificationData, 0x02, secondPinByte);
            response = smartCardReader.OKTransmit(command);
            PrintData("Compare verification data - second part", command, response, $"{secondPinByte:X2}");

            // Compare verification data - third part
            command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.CompareVerificationData, 0x03, thirdPinByte);
            response = smartCardReader.OKTransmit(command);
            PrintData("Compare verification data - third part", command, response, $"{thirdPinByte:X2}");

            // Reset Error Counter
            command = twoWireBusProtocol.GetApdu(Synchronus2WBP.ControlByte.UpdateSecurityMemory, 0x00, 0xFF);
            response = smartCardReader.OKTransmit(command);
            PrintData("Reset Error Counter", command, response, "");
        }
        private static void VerifyUser3Wbp(WinscardReader smartCardReader, byte firstPinByte, byte secondPinByte)
        {
            var threeWireBusProtocol = new Synchronus3WBP();

            byte newErrorCounter;
            Console.WriteLine("User Verification");

            // Read Error Counter
            string command = threeWireBusProtocol.ReadErrorCounterApdu();
            string response = smartCardReader.OKTransmit(command);
            string currentErrorCounter = response.Substring(4, 2);
            PrintData("Read Error Counter", command, response, $"0x{currentErrorCounter}");

            // decrement counter
            switch (currentErrorCounter)
            {
                case "7F":
                    newErrorCounter = 0x7E;
                    break;
                case "7E":
                    newErrorCounter = 0x7C;
                    break;
                case "7C":
                    newErrorCounter = 0x78;
                    break;
                case "78":
                    newErrorCounter = 0x70;
                    break;
                case "70":
                    newErrorCounter = 0x60;
                    break;
                case "60":
                    newErrorCounter = 0x40;
                    break;
                case "40":
                    newErrorCounter = 0x00;
                    break;
                default:
                    Console.WriteLine("Returned error counter is not correct or card is blocked");
                    return;
            }
            command = threeWireBusProtocol.WriteErrorCounterApdu(newErrorCounter);
            response = smartCardReader.OKTransmit(command);
            PrintData("Write new Error Counter", command, response, $"0x{newErrorCounter:X2}");

            // Verify pin first byte
            command = threeWireBusProtocol.VerifyFirstPinByte(firstPinByte);
            response = smartCardReader.OKTransmit(command);
            PrintData("Verify first pin byte", command, response, $"{firstPinByte:X2}");

            // Verify pin second byte
            command = threeWireBusProtocol.VerifySecondPinByte(secondPinByte);
            response = smartCardReader.OKTransmit(command);
            PrintData("Verify second pin byte", command, response, $"{secondPinByte:X2}");

            // Reset Error Counter
            command = threeWireBusProtocol.ResetErrorCounterApdu();
            response = smartCardReader.OKTransmit(command);
            PrintData("Reset Error Counter", command, response, "");
        }
        public static void ReadMainMemory3WbpExample(string readerName)
        {
            try
            {
                var threeWireBusProtocol = new Synchronus3WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;

                // address of first byte to be read
                ushort address = 0x00;
                byte notUsed = 0x00;
                string cardMemory = string.Empty;
                bool[] attr = new bool[0x400];

                // read data from addresses 0x0000 - 0x03FF
                for (int i = 0x00; i < 0x0400; i++)
                {
                    string command = threeWireBusProtocol.GetApdu(Synchronus3WBP.ControlByte.Read9BitsDataWithProtectBit, address, notUsed);
                    string response = smartCardReader.OKTransmit(command);

                    if (response.StartsWith("9D02") && response.EndsWith("9000"))
                    {
                        string data = response.Substring(4, 2);
                        string protectionByte = response.Substring(6, 2);
                        string bitSet = protectionByte != "00" ? "not set" : "set";
                        PrintData($"Read Main Memory, Address 0x{address:X4}", command, response,
                            $"Value 0x{data}, {protectionByte} -> protection bit {bitSet}");
                        cardMemory += data;
                        attr[i] = protectionByte != "00" ? false : true;
                    }
                    else
                    {
                        PrintData($"Read Main Memory, Address 0x{address:X4}", command, response, "Error Response");
                    }
                    address++;
                }
                //Console.WriteLine($"\nMain Memory starting from address 0x0000 to address 0x03FF:\n{cardMemory}\n");
                Console.WriteLine($"\nMain Memory starting from address 0x0000 to address 0x03FF:\n");
                PrintByteArray(Utils.HexStringToByteArray(cardMemory), attr, 0);
                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static void Verify3WbpExample(string readerName)
        {

        }

        public static void UpdateMainMemory3WbpExample(string readerName)
        {
            try
            {
                var threeWireBusProtocol = new Synchronus3WBP();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;
                // Following code is commented to prevent usage of incorrect Pin code, which can lead to blocking the synchronus card if done several times in row
                // Verification with correct pin code is necessary to write any data into card memory
                // Be advice not to use VerifyUser2Wbp command if correct pin code is not known 
                /*
                string pin = "FFFF";

                byte firstPinByte = byte.Parse(pin.Substring(0, 2), NumberStyles.HexNumber);
                byte secondPinByte = byte.Parse(pin.Substring(2, 2), NumberStyles.HexNumber);

                VerifyUser3Wbp(reader, firstPinByte, secondPinByte);
                
                //*/
                byte[] pins = null;
                if (!getUserPin(2, ref pins))
                {
                    VerifyUser3Wbp(smartCardReader, pins[0], pins[1]);
                }

                Random random = new Random();
                byte value = (byte)random.Next(0, 256);

                ushort address = 0x0040;
                byte data = value;

                string command = threeWireBusProtocol.GetApdu(Synchronus3WBP.ControlByte.WriteAndEraseWithoutProtectBit, address, data);
                string response = smartCardReader.OKTransmit(command);
                PrintData($"Write Main Memory, address: 0x{address:X4}, data: 0x{data:X2}", command, response, response.Equals("9D009000") ? "Success" : "Error Response");

                address = 0x0041;
                data = value;
                command = threeWireBusProtocol.GetApdu(Synchronus3WBP.ControlByte.WriteAndEraseWithoutProtectBit, address, data);
                response = smartCardReader.OKTransmit(command);
                PrintData($"Write Main Memory, address: 0x{address:X4}, data: 0x{data:X2}", command, response, response.Equals("9D009000") ? "Success" : "Error Response");


                smartCardReader.Disconnect(CardDisposition.Unpower);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////
        //- I2C Samples
        public static void ReadI2CExampleAt24C0X(string readerName)
        {
            try
            {
                var i2c = new SynchronusI2C();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintColorByte(bool showColor, bool isProtected, string strByte, ConsoleColor colorDefault)
        {
            if (showColor)
            {
                if (isProtected)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write(strByte);
                Console.ForegroundColor = colorDefault;
            }
            else
            {
                Console.Write(strByte);
            }
        }

        public static void PrintByteArray(byte[] array, bool[] arr_attr = null, uint offset = 0)
        {
            int bytesPerLine = 16;
            int totalBytes = array.Length;
            int address = 0;
            bool showColor = arr_attr != null;
            ConsoleColor colorDefault = Console.ForegroundColor;

            for (int i = 0; i < totalBytes; i += bytesPerLine)
            {
                // 打印地址
                Console.Write($"{address + offset:X4}: ");

                // 打印16个十六进制值
                for (int j = i; j < i + bytesPerLine && j < totalBytes; j++)
                {
                    // Console.Write($"{array[j]:X2} ");
                    PrintColorByte(showColor, arr_attr[j], $"{array[j]:X2} ", colorDefault);
                }

                // 打印空格以对齐ASCII部分
                for (int j = 0; j < (bytesPerLine - (totalBytes - i)) * 3; j++)
                {
                    Console.Write("   ");
                }

                // 打印ASCII字符
                for (int j = i; j < i + bytesPerLine && j < totalBytes; j++)
                {
                    char ch = (char)array[j];
                    if (ch < 32 || ch > 126)
                    {
                        // Console.Write(".");
                        PrintColorByte(showColor, arr_attr[j], ".", colorDefault);
                    }
                    else
                    {
                        // Console.Write(ch);
                        PrintColorByte(showColor, arr_attr[j], ch.ToString(), colorDefault);
                    }
                }

                Console.WriteLine();
                address += bytesPerLine;
            }
        }

        public static void ReadI2CExampleAt24CXX(string readerName, byte cardType)
        {
            try
            {
                var i2c = new SynchronusI2C();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;
                 
                I2cCardInfo card = AT24CXX.Instance.getCardByType(cardType);
                Console.WriteLine($"Read Example for {card.Name}, {card.Size} Bytes.");

                byte bytesToRead = (byte)card.PageSize;
                byte[] rapdu = null;
                List<byte> CardData = new List<byte>();

                for (int i = 0; i < card.Size; i += bytesToRead)
                {
                    string command = i2c.GetReadCommandApdu((MemorySize)card.Size, i, bytesToRead);
                    bool result = smartCardReader.Transmit(command, ref rapdu);
                    if (!result)
                        throw new InvalidOperationException("Transmit failed.");

                    if (rapdu.Length < 6)
                        throw new InvalidOperationException("Response too short.");

                    if (rapdu[0] == 0x9D)
                    {// 9D xx data  
                        CardData.AddRange(rapdu.Skip(2).Take(rapdu.Length - 2));
                    }
                    else if (rapdu[0] == 0xBD)
                    {// BD xx A2 yy data
                        CardData.AddRange(rapdu.Skip(4).Take(rapdu.Length - 4));
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported Tag.");
                    }
                }

                smartCardReader.Disconnect(CardDisposition.Unpower);

                PrintByteArray(CardData.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void WriteI2CExampleAt24CXX(string readerName, byte cardType)
        {
            try
            {
                var i2c = new SynchronusI2C();
                WinscardReader smartCardReader = Connect(readerName);

                if (!smartCardReader.IsConnected)
                    return;

                I2cCardInfo card = AT24CXX.Instance.getCardByType(cardType);
                Console.WriteLine($"Write Example for {card.Name}, {card.Size} Bytes.");

                byte bytesToWrite = (byte)card.PageSize;
                byte[] rapdu = null;
                byte[] byteArray = new byte[bytesToWrite];
                Random random = new Random();
                byte value = (byte)random.Next(0, 256);

                for (int i = 0; i < byteArray.Length; i++)
                {
                    byteArray[i] = value; // byte类型范围是0-255
                }
                string strData = Utils.ByteArrayToHexString(byteArray, 0, byteArray.Length);
                
                Console.WriteLine($"Data Pattern {strData} ");

                for (int i = 0; i < card.Size; i += bytesToWrite)
                {
                    string command = i2c.GetWriteCommandApdu((MemorySize)card.Size, i, bytesToWrite, strData);
                    bool result = smartCardReader.Transmit(command, ref rapdu);
                    if (!result)
                        throw new InvalidOperationException("Transmit failed.");

                    if (rapdu[0] == 0x9D)
                    {// 9D xx data  
                    }
                    else if (rapdu[0] == 0xBD)
                    {// BD xx A2 yy data
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported Tag.");
                    }
                    Thread.Sleep(10); // 芯片写入需要时间
                }

                smartCardReader.Disconnect(CardDisposition.Unpower);

                //PrintByteArray(byteArray);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
