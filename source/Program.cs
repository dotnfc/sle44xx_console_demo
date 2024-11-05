using HidGlobal.OK.SampleCodes.MenuSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCardSyncDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMenuSection rootMenu;
            rootMenu = new SmartCardReadersMenuSection(SmartCardReadersMenuFactory.Instance);
            rootMenu.RootMenuItem.Execute();
        }
    }
}
