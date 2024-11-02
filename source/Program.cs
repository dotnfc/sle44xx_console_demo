using HidGlobal.OK.SampleCodes.MenuSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCardSyncDemo
{
    internal class Program
    {
        private static IMenuItem _rootMenu = new MenuItem("SyncVendor Samples", true);
        private static IMenuSection _smartCardReadersSection = new SmartCardReadersMenuSection(SmartCardReadersMenuFactory.Instance);


        static void Main(string[] args)
        {
            _rootMenu.AddSubItem(_smartCardReadersSection.RootMenuItem);

            _rootMenu.Execute();
        }
    }
}
