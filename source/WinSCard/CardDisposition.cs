using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCardSyncDemo.WinSCard
{
    public enum CardDisposition
    {
        [Description("No action.")]
        Leave,
        [Description("Reset the card")]
        Reset,
        [Description("Unpower the card")]
        Unpower,
        [Description("Eject the card")]
        Eject
    }
}
