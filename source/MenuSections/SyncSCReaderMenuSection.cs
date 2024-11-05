using HidGlobal.OK.SampleCodes.AViatoR;
using HidGlobal.OK.SampleCodes.MenuSections;
using SCardSyncDemo.AViatoR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SCardSyncDemo.MenuSections
{
    public class SyncSCReaderMenuSection : IMenuSection
    {
        private readonly IMenuItem _rootMenuItem;

        public IMenuItem RootMenuItem
        {
            get { return _rootMenuItem; }
        }

        public SyncSCReaderMenuSection(string readerName, string serialNumber)
        {
            var description = string.IsNullOrWhiteSpace(serialNumber)
                ? $"{readerName}"
                : $"{readerName}\nSerial Number: {serialNumber}";

            _rootMenuItem = new MenuItem(description);

            var synchronusContactCardExamples = _rootMenuItem.AddSubItem("Synchronus Contact Card Examples");
            {
                var synchronus2wbp = synchronusContactCardExamples.AddSubItem("2WBP Example(SLE 4452/42/FM4442)");
                {
                    synchronus2wbp.AddSubItem("Read Main Memory", () => ContactCardCommunicationSample.ReadMainMemory2WbpExample(readerName));
                    synchronus2wbp.AddSubItem("Read Protection Memory", () => ContactCardCommunicationSample.ReadProtectionMemory2WbpExample(readerName));
                    synchronus2wbp.AddSubItem("Verify", () => ContactCardCommunicationSample.Verify2WbpExample(readerName));
                    synchronus2wbp.AddSubItem("Update Main Memory", () => ContactCardCommunicationSample.UpdateMainMemory2WbpExample(readerName));
                }

                var synchronus3wbp = synchronusContactCardExamples.AddSubItem("3WBP Example(SLE 4418/28/FM4428)");
                {
                    synchronus3wbp.AddSubItem("Read Memory(With Protect Bits)", () => ContactCardCommunicationSample.ReadMainMemory3WbpExample(readerName));
                    synchronus3wbp.AddSubItem("Verify", () => ContactCardCommunicationSample.Verify3WbpExample(readerName));
                    synchronus3wbp.AddSubItem("Update Main Memory", () => ContactCardCommunicationSample.UpdateMainMemory3WbpExample(readerName));
                }

                var synchronusI2c = synchronusContactCardExamples.AddSubItem("I2C Example(AT24C01/02...)");
                {
                    foreach (var card in AT24CXX.Instance)
                    {
                        synchronusI2c.AddSubItem($"Read Example for {card.Name}", () => 
                            ContactCardCommunicationSample.ReadI2CExampleAt24CXX(readerName, card.Type));
                        synchronusI2c.AddSubItem($"Write Example for {card.Name}", () => 
                            ContactCardCommunicationSample.WriteI2CExampleAt24CXX(readerName, card.Type));
                    }

                    //synchronusI2c.AddSubItem("Read Example with AT24C16", () => ContactCardCommunicationSample.ReadI2CExampleAt24C16(readerName));
                    //synchronusI2c.AddSubItem("Write Example with AT24C16", () => ContactCardCommunicationSample.WriteI2CExampleAt24C16(readerName));
                    //synchronusI2c.AddSubItem("Read Example with AT24C128", () => ContactCardCommunicationSample.ReadI2CExampleAt24C128(readerName));
                    //synchronusI2c.AddSubItem("Write Example with AT24C128", () => ContactCardCommunicationSample.WriteI2CExampleAt24C128(readerName));
                }
            }
        }
    }
}

